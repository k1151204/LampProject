using UnityEngine;
using System.Collections;

public class Stage9ObjectDieCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);

        }
    }
}
