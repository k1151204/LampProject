using UnityEngine;
using System.Collections;

public class DesireSwitch : MonoBehaviour 
{
    public GameObject desire;
    public GameObject DestroyObject;
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            Invoke("SrartMoveDownMessage",12f);
            Destroy(DestroyObject, 11f);
        }
    }

    public void SrartMoveDownMessage()
    {
        desire.GetComponent<PotMotion>().SendMessage("ReactionStart", SendMessageOptions.DontRequireReceiver);
    }
}
