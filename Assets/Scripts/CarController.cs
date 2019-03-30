using UnityEngine;

public class CarController : MonoBehaviour
{
    public float power = 3;
    public float maxspeed = 5;
    public float turnpower = 2;
    public float friction = 3;
    private Rigidbody2D rigidbody2D;
    private Vector2 carSpeed;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {


        if (carSpeed.magnitude > maxspeed)
        {
            carSpeed = carSpeed.normalized;
            carSpeed *= maxspeed;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            rigidbody2D.AddForce( transform.up * power);
            rigidbody2D.drag = friction;
            Debug.Log("Z pressed");
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody2D.AddForce(-(transform.up) * (power / 2));
            rigidbody2D.drag = friction;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward * turnpower);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * -turnpower);
        }
    }
}
