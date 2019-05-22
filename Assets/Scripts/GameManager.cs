using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {

  [Tooltip ("The prefab to use for representing the player")]
  public GameObject playerPrefab;
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
    InstantiateCar = PhotonNetwork.Instantiate (playerPrefab.name, StartPos.position, Quaternion.identity, 0);
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