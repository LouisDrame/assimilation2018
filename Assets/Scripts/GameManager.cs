using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {

  [Tooltip ("The prefab to use for representing the player")]
  public Transform StartPos;
  private GameObject InstantiateCar;

  #region Photon Callbacks

  /// <summary>
  /// Called when the local player left the room. We need to load the launcher scene.
  /// </summary>
  public override void OnLeftRoom () {
    SceneManager.LoadScene (0);
  }

  #endregion

  #region Public Methods

  private void Start () {
    if (PhotonNetwork.IsMasterClient) {
      InstantiateCar = PhotonNetwork.Instantiate ("Police", StartPos.position, Quaternion.identity, 0);
    } else {
      InstantiateCar = PhotonNetwork.Instantiate ("Bandit", StartPos.position, Quaternion.identity, 0);
    }
    //GameObject.Find("/topDownCar/tron/Plane001").GetComponent<MeshRenderer>().material.SetColor("_Color",Color.red);
    //rend = GetComponent<Renderer>();
  }

  public void LeaveRoom () {
    PhotonNetwork.LeaveRoom ();
  }

  public override void OnPlayerEnteredRoom (Player other) {
    Debug.LogFormat ("OnPlayerEnteredRoom() {0}", other.NickName);

    if (PhotonNetwork.IsMasterClient) {
      Debug.LogFormat ("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
    }

  }

  public override void OnPlayerLeftRoom (Player other) {
    Debug.LogFormat ("OnPlayerLeftRoom() {0}", other.NickName);

    if (PhotonNetwork.IsMasterClient) {

      Debug.LogFormat ("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
    }
  }

  #endregion

}