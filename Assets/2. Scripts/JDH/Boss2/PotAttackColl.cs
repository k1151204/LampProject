using UnityEngine;
using System.Collections;

public class PotAttackColl : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag == "Player")
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
    }


}
