using UnityEngine;
using System.Collections;

public class Effet_Switch : MonoBehaviour 
{
    public GameObject effet;
    public float effetOn_Time = 0.1f;

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
            Invoke("EffetOn", effetOn_Time);
        }
    }

    void EffetOn()
    {
        effet.SetActive(true);
    }
}
