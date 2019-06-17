using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster1 : MonoBehaviour
{
	public float countdown = 3f;
	public float vitesse = 200f;
	
	void OnCollisionEnter(Collision collision)
    {
		//GetComponent<particule>().enabled;
		collision.gameObject.GetComponent<CarController>().lancerCoroutine(countdown,vitesse);
		Destroy(gameObject);
    }

}