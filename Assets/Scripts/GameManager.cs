using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Tooltip("The prefab to use for representing the player")]
    public Transform StartPosCop, StartPosThief;
    private GameObject InstantiateCar;

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
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{"GameStart",true}});
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

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged){
        object gameStart = propertiesThatChanged["GameStart"];
        object gameStartForCop = propertiesThatChanged["GameStartForCop"];
        if(gameStart is bool && (bool) gameStart){
            Debug.Log("Debut du jeu");
            if(InstantiateCar.GetComponent<CarController>().isBandit){
                InstantiateCar.GetComponent<CarController>().EnableInput();
            }
        }

        if(gameStartForCop is bool && (bool)gameStartForCop){
            if(!InstantiateCar.GetComponent<CarController>().isBandit){
                InstantiateCar.GetComponent<CarController>().EnableInput();
            }
        }

    }
    #endregion
}