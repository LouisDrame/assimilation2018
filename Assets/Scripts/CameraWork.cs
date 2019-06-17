using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{

    private Transform cameraTransform;
    private bool isFollowing; //Si vrai, la camera suis le joueur concerné. Ce paramètre ne sera modifié que si on rentre dans OnStartFollowing, appelé depuis le CarController du prefab correspondant au role du joueur

    void Start()
    {

    }

    void LateUpdate()
    {
        if (isFollowing)
        {
            cameraTransform.position = new Vector3(gameObject.transform.position.x, 40f, gameObject.transform.position.z); //On assigne la position de la moto à la caméra
        }
    }

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
    }
}
