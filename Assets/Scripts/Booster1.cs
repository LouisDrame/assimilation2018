using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster1 : MonoBehaviour
{
	public float countdown; //Durée du powerUp
	public float vitesse; //Nouvelle valeur de puissance appliquée au véhicule, si à -1 appliquera un effet d'inversion des contrôles gauche/droite
	
	void OnCollisionEnter(Collision collision)
    {
		collision.gameObject.GetComponent<CarController>().lancerCoroutine(countdown,vitesse);
		Destroy(gameObject);
    }

}