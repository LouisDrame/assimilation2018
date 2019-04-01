using UnityEngine;

public class CarController : MonoBehaviour
{
    public float power = 3;
    public float maxspeed = 5;
    public float turnpower = 2;
    public float friction = 3;
    public float accelMax = 3;
    public float currentacel = 1f;
    public float accelGap = 0.01f;
    private Rigidbody2D rigidbody2D;
    private Vector2 carSpeed;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        GameObject.Find("WPTrail1").GetComponent<TrailRenderer>().emitting = false;
        GameObject.Find("WPTrail2").GetComponent<TrailRenderer>().emitting = false;
        if (carSpeed.magnitude > maxspeed)
        {
            carSpeed = carSpeed.normalized;
            carSpeed *= maxspeed;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            rigidbody2D.AddForce(transform.up * power * currentacel);
            rigidbody2D.drag = friction;
            // if (currentacel <= accelMax)
            // {
            //     currentacel += accelGap;
            // }
        }
        // else
        // {
        //     if(currentacel > 0){
        //         currentacel -= accelGap;
        //     }
        // }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody2D.AddForce(-(transform.up) * (power / 2));
            rigidbody2D.drag = friction;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward * turnpower);
            GameObject.Find("WPTrail1").GetComponent<TrailRenderer>().emitting = true;
            GameObject.Find("WPTrail2").GetComponent<TrailRenderer>().emitting = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * -turnpower);
            GameObject.Find("WPTrail1").GetComponent<TrailRenderer>().emitting = true;
            GameObject.Find("WPTrail2").GetComponent<TrailRenderer>().emitting = true;

        }
    }
}
