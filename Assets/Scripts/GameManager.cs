using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{

    [Tooltip("The prefab to use for representing the player")]
    public Transform StartPosCop, StartPosThief; //Positions des 2 GameObject ou sont instanciés les deux motos
    private GameObject InstantiateCar;           //Référence au GameObject instancié pour ce joueur
    public GameObject winUI, loseUI, startUI;    //Référence aux différents éléments d'UI, winUI s'affiche si le joueur gagne, loseUI s'il perd et startUI sert au moment du compre à rebours de début
    public AudioSource source; //Audio source utilisée pour gérer les défférents sons en jeu
    public AudioClip loseSound, winSound, gameSound; //Différents sons utilisés en jeu

    #region Photon Callbacks

    //Quand le joueur quitte la room il est automatiquement renvoyé au menu
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Public Methods

    private void Start()
    {
        Debug.Log("Start");
        //Gestion des instanciations, par défaut le master est instancié en tant que policier
        //Le second joueur est instancié en tant que bandit
        //Lorqu'un second joueur entre dans la room une roomProperty est passée à True pour indiquer que le jeu peut commencer
        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateCar = PhotonNetwork.Instantiate("Police", StartPosCop.position, StartPosCop.rotation, 0);
        }
        else
        {
            InstantiateCar = PhotonNetwork.Instantiate("Bandit", StartPosThief.position, Quaternion.identity, 0);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "GameStart", true } });
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    //Fonction de retour au menu, appelle LeaveRoom(), après cet appel OnLeftRoom est appelé et charge le menu principal
    public void QuitToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    //Méthode Photon qui permet de détecter des changements sur les RoomProperties et effectuer des actions en conséquence
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object gameStart = propertiesThatChanged["GameStart"];
        object gameStartForCop = propertiesThatChanged["GameStartForCop"];
        object banditWin = propertiesThatChanged["BanditWin"];
        object copWin = propertiesThatChanged["CopWin"];

        //Quand le second joueur entre en jeu la property GameStart passe à true et ce bloc de code est exécuté
        //La coroutine GameStartCoroutine affiche un compte à rebours et indique quand les joueurs peuvent effectuer des inputs
        if (gameStart is bool && (bool)gameStart)
        {
            StartCoroutine(GameStartCoroutine());
        }

        //Quand le bandit passe une certaine ligne le policier peut commencer à le suivre
        //A ce moment là un message s'affiche pendant une seconde grâce à la coroutine GoForCop
        if (gameStartForCop is bool && (bool)gameStartForCop)
        {
            if (!InstantiateCar.GetComponent<CarController>().isBandit)
            {
                InstantiateCar.GetComponent<CarController>().EnableInput();
                StartCoroutine(GoForCop());
            }
        }

        //Gestion de la victoire d'un bandit
        //Affichage des UI de victoire pour le bandit et de defaite pour le policier
        //Lecture des sons de victoire ou de défaite pour chacun des joueurs
        if (banditWin is bool && (bool)banditWin)
        {
            if (InstantiateCar.GetComponent<CarController>().isBandit)
            {
                winUI.SetActive(true);
                source.Stop();
                source.clip = winSound;
                source.Play();

            }
            else
            {
                loseUI.SetActive(true);
                source.Stop();
                source.clip = loseSound;
                source.Play();
            }
            InstantiateCar.GetComponent<CarController>().DisableInput();
        }

        //Gestion de la victoire d'un policier
        //Affichage de l'UI de défaite pour le bandit et de victoire pour le policier
        //Lecture des sons de victoire ou de défaite pour chacun des joueurs
        if (copWin is bool && (bool)copWin)
        {
            if (InstantiateCar.GetComponent<CarController>().isBandit)
            {
                loseUI.SetActive(true);
                source.Stop();
                source.clip = loseSound;
                source.Play();
            }
            else
            {
                winUI.SetActive(true);
                source.Stop();
                source.clip = winSound;
                source.Play();
            }
            InstantiateCar.GetComponent<CarController>().DisableInput();
        }

    }

    //Coroutine de début du jeu, lancée lorsque le second joueur rejoint la partie
    //Affiche un compte à rebours et permet au bandit de jouer.
    //Affiche un message d'attente pour el policier qui devra attendre que le bandit dépasse une ligne de déclenchement
    //Lecture de la musique de jeu quand les deux joueurs sont présents
    IEnumerator GameStartCoroutine()
    {
        source.clip = gameSound;
        source.Play();
        startUI.GetComponent<Text>().text = "Prêt ? ";
        for (int i = 5; i > 0; i--)
        {
            startUI.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        if (InstantiateCar.GetComponent<CarController>().isBandit)
        {
            InstantiateCar.GetComponent<CarController>().EnableInput();
            startUI.GetComponent<Text>().text = "Go!";
            yield return new WaitForSeconds(1);
            startUI.SetActive(false);
        }
        else
        {
            startUI.GetComponent<Text>().text = "...";
        }
    }

    //Coroutine qui affichera le message de départ au policier une fois que le bandit aura franchi la ligne de déclenchement
    IEnumerator GoForCop()
    {
        startUI.GetComponent<Text>().text = "Go!";
        yield return new WaitForSeconds(1);
        startUI.SetActive(false);
    }

    //Fonction utilisée pour changer une roomproperty depuis un autre script pour éviter d'avoir à créer un Using Photon dans tous les scripts
    public void SetRoomProperty(string propertyName, bool value)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { propertyName, value } });
    }
    #endregion
}