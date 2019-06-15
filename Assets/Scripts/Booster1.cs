using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster1 : MonoBehaviour
{
	public float countdown = 3f;
	public float vitesse = 200f;
	
	void OnCollisionEnter(Collision collision)
    {
		collision.gameObject.GetComponent<CarController>().lancerCoroutine(countdown,vitesse);
		//StartCoroutine(collision.gameObject.GetComponent<CarController>().applyPowerUp(countdown,vitesse));
		Destroy(gameObject);
    }

}