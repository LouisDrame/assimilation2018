using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    Vector3 BaseCameraPos;
    Quaternion BaseCameraRot;
    // Start is called before the first frame update
    void Start()
    {
        BaseCameraPos = gameObject.transform.position;
        BaseCameraRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = BaseCameraRot;
    }
}
