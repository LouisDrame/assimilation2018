using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviourPun {
  public float power = 3;
  public float maxspeed = 5;
  public float turnpower = 2;
  public float friction = 3;
  public float accelMax = 3;
  public float currentacel = 1f;
  public float accelGap = 0.01f;
  private Rigidbody rigidbody;
  private Vector2 carSpeed;
  [Tooltip ("The local player instance. Use this to know if the local player is represented in the Scene")]
  public static GameObject LocalPlayerInstance;

  private void Awake () {
    // #Important
    // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
    if (photonView.IsMine) {
      CarController.LocalPlayerInstance = this.gameObject;
    }
    // #Critical
    // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
    DontDestroyOnLoad (this.gameObject);
  }

  private void Start () {
    rigidbody = GetComponent<Rigidbody> ();
    CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork> ();

    if (_cameraWork != null) {
      if (photonView.IsMine) {
        _cameraWork.OnStartFollowing ();
      }
    } else {
      Debug.LogError ("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
    }
  }

  private void FixedUpdate () {
    if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
      return;
    }
    GameObject.Find ("WPTrail1").GetComponent<TrailRenderer> ().emitting = false;
    GameObject.Find ("WPTrail2").GetComponent<TrailRenderer> ().emitting = false;
    if (carSpeed.magnitude > maxspeed) {
      carSpeed = carSpeed.normalized;
      carSpeed *= maxspeed;
    }

    if (Input.GetKey (KeyCode.Z)) {
      Up ();
    }
    if (Input.GetKey (KeyCode.S)) {
      Down ();
    }
    if (Input.GetKey (KeyCode.Q)) {
      Left ();
    }
    if (Input.GetKey (KeyCode.D)) {
      Right ();
    }
  }

  public void Up () {
    rigidbody.AddForce (transform.forward * power);
    rigidbody.drag = friction;
  }

  public void Down () {
    rigidbody.AddForce (-(transform.forward) * (power / 2));
    rigidbody.drag = friction;
  }

  public void Left () {
    if (GetComponent<Rigidbody> ().velocity.x != 0 || GetComponent<Rigidbody> ().velocity.z != 0) {

      transform.Rotate (Vector3.up * -turnpower);
      GameObject.Find ("WPTrail1").GetComponent<TrailRenderer> ().emitting = true;
      GameObject.Find ("WPTrail2").GetComponent<TrailRenderer> ().emitting = true;
    }
  }

  public void Right () {
    Debug.Log ("Rotate called");
    if (GetComponent<Rigidbody> ().velocity.x != 0 || GetComponent<Rigidbody> ().velocity.z != 0) {
      transform.Rotate (Vector3.up * turnpower);
      GameObject.Find ("WPTrail1").GetComponent<TrailRenderer> ().emitting = true;
      GameObject.Find ("WPTrail2").GetComponent<TrailRenderer> ().emitting = true;
    }
  }
}