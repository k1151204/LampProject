using UnityEngine;
using System.Collections;

public class CartPathDivide : MonoBehaviour 
{
    public Animator ani_Lever;
    public GameObject pathSetting;
	void Start () 
    {
        
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LightBoom"))
        {
            ani_Lever.enabled = true;
            pathSetting.GetComponent<PathMove>().NextPathName[4] = "Stage8Cart6";
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
