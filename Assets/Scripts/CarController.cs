using Photon.Pun;
using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public float power; /*!< Puissance courante du véhicule, est initialisée à la valeur prise par minPower en début de jeu */
    public float maxPower; /*!< Puissance maximum courante du véhicule, peut être modifiée par les bonus, la puissance courante du véhicule ne peut pas dépasser cette valeur */
    public float baseMaxPower; /*!< Puissance max, utilisée pour garder une trace de la puissance maximum du véhicule en dehors des bonus */
    public float minPower = 50; /*!< Puissance minimale utilisée quand le véhicule démarre, la valeur de puissance courante diminue vers cette valeur dès que le joueur n'accélère pas (Touche Z) */
    public float basMinPower = 50;
    public float accelerationValueFloat = 30f; /*!< Valeur d'accélération */
    public float turnpower = 2;
    public float friction = 3;
    public bool isBandit;
    public float health = 3;
    public bool inputBlocked;
    private Rigidbody rigidbody;
    private float savePower;
    private bool inverse = false;
    private bool powerUp = false;
    private float backPower = 200;

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
            if (!inputBlocked)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Debug.Log("S pushed");
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
            // Debug.Log("Current power :" + power);
        }
    }

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

    public void lancerCoroutine(float duration, float newPower)
    {
        if (!powerUp)
        {
            powerUp = true;
            StartCoroutine(applyPowerUp(duration, newPower));
        }
    }

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

    public void EnableInput()
    {
        this.inputBlocked = false;
    }

    public void DisableInput()
    {
        this.inputBlocked = true;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (isBandit)
        {
            if (other.gameObject.GetComponent<CarController>())
            {
                if (!other.gameObject.GetComponent<CarController>().isBandit)
                {
                    //other.gameObject.GetComponent<CarController>().decrementHelath();
                    GameObject.Find("Game Manager").GetComponent<GameManager>().SetRoomProperty("CopWin", true);
                }
            }
        }
    }

    public void decrementHelath()
    {
        health--;
    }

}