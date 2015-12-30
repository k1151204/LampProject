using UnityEngine;
using System.Collections;

public class Stage1StartCollider : MonoBehaviour 
{
    Transform target;

	void Start () 
    {
        
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = GameObject.Find("Lamp").transform;
            GameObject.Find("Main Camera").GetComponent<CameraController>().SetTarget(target);
        }
    }
}
