using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks {

  [Tooltip ("Reference le panel du premier menu")]
  [SerializeField]
  private GameObject controlPanel;

  [Tooltip ("Nombre de joueur maximums dans la salle")]
  [SerializeField]
  private byte maxPlayersPerRoom = 2;

  [Tooltip ("Input du nom de la salle a rejoindre")]
  [SerializeField]
  private InputField RoomNameJoinInput;

  [Tooltip ("Input du nom de creation de la salle")]
  [SerializeField]
  private InputField RoomNameCreateInput;

  string gameVersion = "1";

  void Awake () {
    PhotonNetwork.AutomaticallySyncScene = true; //Permet la synchronisation de la scene
  }

  void Start () {
    PhotonNetwork.GameVersion = gameVersion; //Défini la version du jeu pour empecher les joueurs de rejoindre avec une mauvaise version
    PhotonNetwork.ConnectUsingSettings ();
  }

  public override void OnConnectedToMaster () {
    Debug.Log ("OnConnectedToMaster"); //Permet d'afficher dans la console lorsque le client se connecte au Master. Principalement a pour débug
  }

  public override void OnDisconnected (DisconnectCause cause) {
    Debug.LogWarningFormat ("DISCONECTED", cause); //Affiche le message de disconect. Principalement pour debug
  }

  public override void OnCreatedRoom () {
    Debug.Log ("Serveur " + RoomNameCreateInput.text + " a été crée."); //Affiche le nom de la salle crée si la salle s'est créer correctement. Pour debug.
  }

  public override void OnJoinedRoom () {
    PhotonNetwork.LoadLevel ("ProtoVoiture"); //Charge la scèene "ProtoVoiture"
    Debug.Log ("Rejoins serveur " + RoomNameJoinInput.text + ".");
  }

  public override void OnCreateRoomFailed (short returnCode, string message) { //Gère le cas ou la room ne s'est pas créer et renvoie dans la console l'erreur retourné
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

  public override void OnJoinRoomFailed (short returnCode, string message) { //Si il y a une erreur lorsque le joueur rejoint revoie l'erreur
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
      PhotonNetwork.CreateRoom (RoomNameCreateInput.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); //Créer la room avec les paramètre de nom, et de nombre de joueurs max
    }
  }

  public void JoinRoom () {
    if (PhotonNetwork.IsConnected) {
      Debug.Log(RoomNameJoinInput.text);
      PhotonNetwork.JoinRoom (RoomNameJoinInput.text); //Rejoins la room avec le nom spécifié
    }
  }

}