using UnityEngine;
using System.Collections;

public class DieCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);

        }


    }
    public void remove()
    {
        DestroyObject(this.gameObject);
    }
}
