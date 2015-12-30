using UnityEngine;
using System.Collections;

public class OnRigidbody : MonoBehaviour {
    Rigidbody rig;
    public float invokeTime = 1.0f;
	// Use this for initialization
	void Start () {
        rig = gameObject.GetComponent<Rigidbody>();
        rig.isKinematic = true;
	}

    void OnCollisionStay(Collision other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("fall", invokeTime);
        }
    }

    void fall()
    {
        rig.isKinematic = false;
    }
}
