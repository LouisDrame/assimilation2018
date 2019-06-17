using Photon.Pun;
using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public float power; /*!< Puissance courante du véhicule, est initialisée à la valeur prise par minPower en début de jeu */
    public float maxPower; /*!< Puissance maximum courante du véhicule, peut être modifiée par les bonus, la puissance courante du véhicule ne peut pas dépasser cette valeur */
    public float baseMaxPower; /*!< Puissance max, utilisée pour garder une trace de la puissance maximum du véhicule en dehors des bonus */
    public float minPower; /*!< Puissance minimale utilisée quand le véhicule démarre, la valeur de puissance courante diminue vers cette valeur dès que le joueur n'accélère pas (Touche Z) */
    public float basMinPower; 
    public float accelerationValueFloat = 30f; /*!< Valeur d'accélération */
    public float turnpower = 2; /* Force de rotation utilisée par les méthode Right et Left pour faire tourner le véhicule */
    public float friction = 3; /* Friction appliquée aux mouvements du véhicule */
    public bool isBandit; /* Permet de savoir si l'objet est un bandit ou un policier, valeur modifiée dans les prefabs */
    public bool inputBlocked; /* Utilisé pour bloquer les mouvements des joueurs avant le début de partie ou en fin de partie */
    private Rigidbody rigidbody; /* référence au rigidbody du véhicule, c'est à lui que sont ajoutées les forces utilsiées pour les déplacements */
    private bool inverse = false; /* Utilisé pour les power up, permet d'inverses les commandes gauche/droite du véhicule */
    private bool powerUp = false; /* Utilsié pour savoir si le véhicule est sous l'effet d'un power up */
    private float backPower = 200; /* Force utilisée pour les marche-arrière du véhicule */
    private bool gameEnded = false; //Vérification rapide pour savoir si la collision avec le policer a déjà été détectée, dans ce cas la détection de collision de s'effectue plus

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameObject.Find("Trail").GetComponent<TrailRenderer>().emitting = true;
            rigidbody = GetComponent<Rigidbody>();
            gameObject.GetComponent<CameraWork>().OnStartFollowing();
            power = minPower;
        }

    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //Gestion des inputs 
            // Z --> avancer
            // S --> reculer
            // Q --> tourner à gauche
            // D --> tourner à droite
            if (!inputBlocked)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Down();
                }
                if (Input.GetKey(KeyCode.Z))
                {
                    Up();
                    power += accelerationValueFloat * Time.fixedDeltaTime;
                    if (power >= maxPower)
                    {
                        power = maxPower;
                    }
                }
                //Diminution de la puissance lorsque Z n'est pas enfoncée
                else
                {
                    power -= accelerationValueFloat * 2 * Time.fixedDeltaTime;
                    if (power <= minPower)
                    {
                        power = minPower;
                    }
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    Left();
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Right();
                }
            }
        }
    }

    //Fonctions de déplacement
    //
    //
    public void Up()
    {
        rigidbody.AddForce(transform.forward * power);
        rigidbody.drag = friction;
    }

    public void Down()
    {
        rigidbody.AddForce(-(transform.forward) * backPower);
        Debug.Log("Down Used");
        rigidbody.drag = friction;
    }

    //Fonction de rotation
    //Le véhicule ne peut tourner que quand sa valocité est différente de 0 pour éviter qu'il ne puisse tourner sur lui même
    public void Left()
    {
        if (GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0)
        {
            if (!inverse)
            {
                transform.Rotate(Vector3.up * -turnpower);
            }
            else
            {
                transform.Rotate(Vector3.up * turnpower);
            }
        }
    }

    public void Right()
    {
        if (GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0)
        {
            if (!inverse)
            {
                transform.Rotate(Vector3.up * turnpower);
            }
            else
            {
                transform.Rotate(Vector3.up * -turnpower);
            }
        }
    }

    //Permet de lancer une coroutine depuis un autre script tout en permettant au CarController de garder la charge de la couroutine
    //Dans le cas contraire c'est le script qui appelle la coroutune qui la "déroule"
    //Dans le cas des power up qui sont détruits au contact avec le véhicule il faut que la coroutine soit lancée depuis un script qui ne se détruise pas
    public void lancerCoroutine(float duration, float newPower)
    {
        if (!powerUp)
        {
            powerUp = true;
            StartCoroutine(applyPowerUp(duration, newPower));
        }
    }

    //Coroutine d'application de powerup
    //Prend en paramètre la durée et la nouvelle puissance appliquée au véhicule
    //Par soucis de gain de temps une valeur de newPower de -1 revient à appliquer un power up qui inverse les contrôles gauche/droite
    public IEnumerator applyPowerUp(float duration, float newPower)
    {
        if (newPower == -1)
        {
            inverse = true;
            yield return new WaitForSeconds(duration);
            inverse = false;
            powerUp = false;
        }
        else
        {
            this.maxPower = newPower;
            this.power = maxPower;
            yield return new WaitForSeconds(duration);
            this.maxPower = baseMaxPower;
            this.power = baseMaxPower;
            powerUp = false;
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("Object instantiates, is Bandit :" + this.isBandit);
        info.Sender.TagObject = this.gameObject;
    }

    //Fonctions d'ativation/desactivation des inputs pour la fin ou le début du jeu
    public void EnableInput()
    {
        this.inputBlocked = false;
    }

    public void DisableInput()
    {
        this.inputBlocked = true;
    }

    //Gestion de la collision entre un badit et un policier qui entraine une fin de partie et une victoire pour le policier
    //La room Property "CopWin" est passée à true dans le cas ou la collision de produit.
    public void OnCollisionEnter(Collision other)
    {
        if (isBandit)
        {
            if (other.gameObject.GetComponent<CarController>() && gameEnded == false)
            {
                if (!other.gameObject.GetComponent<CarController>().isBandit)
                {
                    //other.gameObject.GetComponent<CarController>().decrementHelath();
                    GameObject.Find("Game Manager").GetComponent<GameManager>().SetRoomProperty("CopWin", true);
                }
                gameEnded = true;
            }
        }
    }
}