using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class EnableCopOnTrigger : MonoBehaviour
{
    // Script utilisé pour activer les inputs pour le policier en début de partie
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            if (other.gameObject.GetComponent<CarController>().isBandit)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "GameStartForCop", true } });
            }
        }
    }
}
