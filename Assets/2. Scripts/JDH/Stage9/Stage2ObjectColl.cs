using UnityEngine;
using System.Collections;

public class Stage2ObjectColl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("StageInfo").GetComponent<Stage2>().SetColl();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("StageInfo").GetComponent<Stage2>().ReSettingColl();
        }
    }
}
