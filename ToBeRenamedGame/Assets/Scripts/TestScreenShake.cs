using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScreenShake : MonoBehaviour
{
    private Transform transform;
    private float shakeDuration;
    private float shakeMangitude = 0.001f;

    private float dampingSpeed = 1.0f;

    Vector3 initialPos;
    
    private void Awake() {
        if(transform == null){
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable() {
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeDuration > 0){
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMangitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else {
            shakeDuration = 0f;
            transform.localPosition = initialPos;
        }
    }

    public void TriggerShake(){
        shakeDuration = 0.01f;
    }
}
