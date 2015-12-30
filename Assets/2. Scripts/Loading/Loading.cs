using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
    public float speed;


	void Update () {
       GetComponent<Rigidbody>().AddForce(speed, 0, 0);
        
        if (transform.position.x > 0.55)
        {
            Application.LoadLevel("Main");
        }
	}
}
