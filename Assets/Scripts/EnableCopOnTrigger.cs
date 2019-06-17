using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class EnableCopOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
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
