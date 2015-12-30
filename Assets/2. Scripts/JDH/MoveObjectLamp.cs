using UnityEngine;
using System.Collections;

public class MoveObjectLamp : MonoBehaviour {
    public GameObject MoveObjectEff;
    GameObject lamp;
    GameObject Brighten;
    BrightenScript BrightenS;

    bool MoveCheck;
    public float MoveTime = 7f;
	// Use this for initialization
	void Start () {
        lamp = GameObject.Find("Lamp");
        Brighten = GameObject.Find("Brighten");
        BrightenS = Brighten.GetComponent<BrightenScript>();
        MoveCheck = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (MoveCheck)
        {
            iTween.LookUpdate(gameObject, iTween.Hash("looktarget", lamp.transform.position, "time", MoveTime));
            iTween.MoveUpdate(gameObject, iTween.Hash("position", lamp.transform.position, "time", MoveTime));
            if (Vector3.Distance(gameObject.transform.position,lamp.transform.position) <= 0.2f)
                EndMove();
        }
	}
    void OnTriggerEnter(Collider other)
    {
   
            if (other.tag == Brighten.tag)
            {
                MoveCheck = true;
            }
    }
    void EndMove()
    {
        if(MoveObjectEff != null)
        Instantiate(MoveObjectEff,lamp.transform.position,Quaternion.identity);
        DestroyObject(gameObject);
        MoveCheck = false;
    }
}
