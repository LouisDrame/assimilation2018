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
    public Transform StartPosCop, StartPosThief;
    private GameObject InstantiateCar;
    public GameObject winUI, loseUI, startUI;
    public AudioSource source;
    public AudioClip loseSound, winSound,gameSound;

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Public Methods

    private void Start()
    {
        Debug.Log("Start");
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

    public void QuitToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object gameStart = propertiesThatChanged["GameStart"];
        object gameStartForCop = propertiesThatChanged["GameStartForCop"];
        object banditWin = propertiesThatChanged["BanditWin"];
        object copWin = propertiesThatChanged["CopWin"];

        if (gameStart is bool && (bool)gameStart)
        {
            // Debug.Log("Debut du jeu");
            // if (InstantiateCar.GetComponent<CarController>().isBandit)
            // {
            //     InstantiateCar.GetComponent<CarController>().EnableInput();
            // }
            StartCoroutine(GameStartCoroutine());

        }

        if (gameStartForCop is bool && (bool)gameStartForCop)
        {
            if (!InstantiateCar.GetComponent<CarController>().isBandit)
            {
                InstantiateCar.GetComponent<CarController>().EnableInput();
                StartCoroutine(GoForCop());
            }
        }

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

    IEnumerator GoForCop()
    {
        startUI.GetComponent<Text>().text = "Go!";
        yield return new WaitForSeconds(1);
        startUI.SetActive(false);
    }

    public void SetRoomProperty(string propertyName, bool value)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { propertyName, value } });
    }
    #endregion
}