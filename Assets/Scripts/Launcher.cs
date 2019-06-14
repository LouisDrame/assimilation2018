using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks {

  [Tooltip ("The Ui Panel to let the user enter name, connect and play")]
  [SerializeField]
  private GameObject controlPanel;

  [Tooltip ("The UI Label to inform the user that the connection is in progress")]
  [SerializeField]
  private GameObject progressLabel;

  [Tooltip ("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
  [SerializeField]
  private byte maxPlayersPerRoom = 4;

  [Tooltip ("Input du nom de la salle a rejoindre")]
  [SerializeField]
  private InputField RoomNameJoinInput;

  [Tooltip ("Input du nom de creation de la salle")]
  [SerializeField]
  private InputField RoomNameCreateInput;

  string gameVersion = "1";

  void Awake () {
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  void Start () {
    PhotonNetwork.GameVersion = gameVersion;
    PhotonNetwork.ConnectUsingSettings ();
  }

  public override void OnConnectedToMaster () {
    Debug.Log ("OnConnectedToMaster");
  }

  public override void OnDisconnected (DisconnectCause cause) {
    Debug.LogWarningFormat ("DISCONECTED", cause);
  }

  public override void OnCreatedRoom () {
    Debug.Log ("Serveur " + RoomNameCreateInput.text + " a été crée.");
  }

  public override void OnJoinedRoom () {
    PhotonNetwork.LoadLevel ("ProtoVoiture");
    Debug.Log ("Rejoins serveur " + RoomNameJoinInput.text + ".");
  }

  public override void OnCreateRoomFailed (short returnCode, string message) {
    switch (returnCode) {
      case 32766:
        {
          Debug.Log ("Nom de serveur déjà pris.");
          break;
        }

      default:
        {
          Debug.Log (message);
          break;
        }
    }
  }

  public override void OnJoinRoomFailed (short returnCode, string message) {
    Debug.Log (returnCode + " - " + message);
    switch (returnCode) {
      case 32758:
        {
          Debug.Log ("Le serveur n'existe pas.");
          break;
        }

      case 32764:
        {
          Debug.Log ("Le serveur est fermé.");
          break;
        }

      default:
        {
          Debug.Log (message);
          break;
        }
    }
  }

  public void CreateRoom () {
    if (PhotonNetwork.IsConnected) {
      PhotonNetwork.CreateRoom (RoomNameCreateInput.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
  }

  public void JoinRoom () {
    if (PhotonNetwork.IsConnected) {
      Debug.Log(RoomNameJoinInput.text);
      PhotonNetwork.JoinRoom (RoomNameJoinInput.text);
    }
  }

}