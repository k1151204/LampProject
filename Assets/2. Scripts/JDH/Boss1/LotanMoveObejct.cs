using UnityEngine;
using System.Collections;

public class LotanMoveObejct : MonoBehaviour {
    
    GameObject Lamp;
    LampInfo lampinfo;
	// Use this for initialization
	void Start () {
        Lamp = GameObject.Find("Lamp");
        lampinfo = Lamp.GetComponent<LampInfo>();
	}
	
	// Update is called once per frame
    void Update()
    {
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            floating(other);

        }
    }
    public void OnTriggerExit(Collider other)
    {
    }
    void floating(Collider target)
    {
        Rigidbody targetRig = target.GetComponent<Rigidbody>();
        if (targetRig != null)
        {
                targetRig.AddForce(Vector3.up * 15, ForceMode.Acceleration);
                if (targetRig.velocity.y < -0.1f)
                {
                    targetRig.velocity = new Vector3(targetRig.velocity.x, targetRig.velocity.y * 0.8f, targetRig.velocity.z);
                } 
        }
    }
}
