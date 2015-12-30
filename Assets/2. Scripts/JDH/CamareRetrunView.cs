using UnityEngine;
using System.Collections;

public class CamareRetrunView : MonoBehaviour {
    bool check;
    CameraController Camare;

	// Use this for initialization
	void Start () {
        Camare = GameObject.Find("Main Camera").GetComponent<CameraController>();
        check = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag != "Player")
            return;
        if (check == true)
            return;
        check = true;
        Camare.ReSetSleepCamaer();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        if (check == true)
            return;
        check = true;
        Camare.ReSetSleepCamaer();


    }
}
