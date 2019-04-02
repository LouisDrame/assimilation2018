using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGame {
  public class PlayerManager : MonoBehaviourPunCallbacks {
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>

    [Tooltip ("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    void Start () {
      CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork> ();

      if (_cameraWork != null) {
        if (photonView.IsMine) {
          _cameraWork.OnStartFollowing ();
        }
      } else {
        Debug.LogError ("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
      }
    }

    private void Awake () {
      // #Important
      // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
      if (photonView.IsMine) {
        PlayerManager.LocalPlayerInstance = this.gameObject;
      }
      // #Critical
      // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
      DontDestroyOnLoad (this.gameObject);
    }
  }
}