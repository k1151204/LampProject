using UnityEngine;
using System.Collections;

public class DestryObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
				if (other.tag == "boss" || other.tag == "Monster" || other.tag == "Remove") {
			Destroy(gameObject,0.1f);
				}
	}

}
