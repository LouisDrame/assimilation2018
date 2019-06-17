using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){ // Gestion de l'entrée du bandit sur l'arrivée
        if(other.gameObject.GetComponent<CarController>()){
            if(other.gameObject.GetComponent<CarController>().isBandit==false){
                Debug.Log("Ok");
            }
        }
    }
}
