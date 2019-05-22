using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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

  string gameVersion = "1";
  bool isConnecting;

  void Awake () {
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  void Start () {
    PhotonNetwork.GameVersion = gameVersion;
    PhotonNetwork.ConnectUsingSettings();
  }

  public void Connect () {
    // isConnecting = true;
    // progressLabel.SetActive (true);
    // controlPanel.SetActive (false);
    if (PhotonNetwork.IsConnected) {
      PhotonNetwork.JoinRandomRoom ();
    } else {
      PhotonNetwork.GameVersion = gameVersion;
      PhotonNetwork.ConnectUsingSettings ();
    }
  }

  public override void OnConnectedToMaster () {
    Debug.Log ("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    if (isConnecting) {
      PhotonNetwork.JoinRandomRoom ();
    }
  }

  public override void OnDisconnected (DisconnectCause cause) {
    // progressLabel.SetActive (false);
    // controlPanel.SetActive (true);
    Debug.LogWarningFormat ("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
  }

  public override void OnJoinRandomFailed (short returnCode, string message) {
    Debug.Log ("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

    // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
    PhotonNetwork.CreateRoom (null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
  }

  public override void OnJoinedRoom () {
    Debug.Log ("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
    Debug.Log ("We load the 'Room for 1' ");

    // #Critical
    // Load the Room Level.
    PhotonNetwork.LoadLevel ("ProtoVoiture");
  }
}