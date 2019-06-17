using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FinishLineTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<CarController>()){
            if(other.gameObject.GetComponent<CarController>().isBandit){
                GameObject.Find("Game Manager").GetComponent<GameManager>().SetRoomProperty("BanditWin",true);
            }
        }
    }
}
