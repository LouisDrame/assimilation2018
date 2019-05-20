using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("collision");
        if(other.gameObject.GetComponent<CarController>() != null){
            other.gameObject.GetComponent<CarController>().power = other.gameObject.GetComponent<CarController>().power * 10;
            Application.Quit();
            Destroy(gameObject);
        }
    }
}
