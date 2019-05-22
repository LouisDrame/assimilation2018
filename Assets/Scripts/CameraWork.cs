using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{

    private Transform cameraTransform;

    private bool isFollowing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isFollowing)
        {
            cameraTransform.position = new Vector3(gameObject.transform.position.x, 14f, gameObject.transform.position.z);
        }
    }

    public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
        }
}
