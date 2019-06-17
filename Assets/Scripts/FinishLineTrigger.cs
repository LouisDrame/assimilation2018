using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FinishLineTrigger : MonoBehaviour
{
    //Script utilisé pour déclencher la victoire du bandit en cas de traversée de la ligne d'arrivée

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<CarController>()){
            if(other.gameObject.GetComponent<CarController>().isBandit){
                GameObject.Find("Game Manager").GetComponent<GameManager>().SetRoomProperty("BanditWin",true);
            }
        }
    }
}
