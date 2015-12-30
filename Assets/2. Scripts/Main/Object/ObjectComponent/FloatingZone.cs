using UnityEngine;
using System.Collections;

public class FloatingZone : MonoBehaviour {
    public enum floatMode { Acceleration, VelocityChange };

    public floatMode FloatingMode = floatMode.Acceleration;
    public float speed = 10.0f;
    public float fallWaveRange = 0.1f;
    public bool onlyPlayer = true;

    void OnTriggerStay(Collider other)
    {
        if (onlyPlayer)
        {
            if (other.CompareTag("Player"))
            {
                floating(other);
            }
        }
        else
        {
            floating(other);
        }
    }

    void floating(Collider target)
    {
        Rigidbody targetRig = target.GetComponent<Rigidbody>();
        if (targetRig != null)
        {
            if (FloatingMode == floatMode.Acceleration)
            {
                targetRig.AddForce(Vector3.up * speed, ForceMode.Acceleration);
            }
            else if (FloatingMode == floatMode.VelocityChange)
            {
                targetRig.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
            }
            if (targetRig.velocity.y < -fallWaveRange)
            {
                targetRig.velocity = new Vector3(targetRig.velocity.x, targetRig.velocity.y*0.8f , targetRig.velocity.z);
            } 
        }
    }
}
