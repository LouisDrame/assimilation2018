using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame {
  public class GameManager : MonoBehaviourPunCallbacks {

    [Tooltip ("The prefab to use for representing the player")]
    public GameObject playerPrefab;

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
      if (playerPrefab == null) {
        Debug.LogError ("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
      } else {
        if (CarController.LocalPlayerInstance == null) {
          Debug.LogFormat ("INSTANCIATION");
          // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
          PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
        } else {
          Debug.LogFormat("DEJA INSTANCIE");
        }
      }
    }

    public void LeaveRoom () {
      PhotonNetwork.LeaveRoom ();
    }

    void LoadArena () {
      if (!PhotonNetwork.IsMasterClient) {
        Debug.LogError ("PhotonNetwork : Trying to Load a level but we are not the master Client");
      }
      PhotonNetwork.LoadLevel ("ProtoVoiture");
    }

    #endregion

  }
}