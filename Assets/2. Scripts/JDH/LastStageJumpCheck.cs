using UnityEngine;
using System.Collections;

public class LastStageJumpCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Lamp").GetComponent<LampMoveManager>().CehckJump = true;
        }
    }
}
