using UnityEngine;
using System.Collections;

public class WaterJumpPad : MonoBehaviour 
{
    public float JumpPower = 4.0f;
    private GameObject lamp;

	void Start () 
    {
        lamp = GameObject.Find("Lamp");
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0, JumpPower, 1.0f), ForceMode.VelocityChange);
            Invoke("Z_FreezePosition", 1.0f);
        }
    }

    void Z_FreezePosition()
    {
        lamp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
}
