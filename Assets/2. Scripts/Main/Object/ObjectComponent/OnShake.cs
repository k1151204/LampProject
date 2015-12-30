using UnityEngine;
using System.Collections;

public class OnShake : MonoBehaviour {
    public float invokeTime = 0.5f;
    public float shakeTime = 0.5f;
    public float shakeStrenth = 0.1f;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Invoke("Shake", invokeTime);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            Invoke("Shake", invokeTime);
    }

    void Shake()
    {
        CameraController.SetShake(shakeTime, shakeStrenth);
    }
}
