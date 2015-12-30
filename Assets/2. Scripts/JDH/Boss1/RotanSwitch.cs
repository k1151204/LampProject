using UnityEngine;
using System.Collections;

public class RotanSwitch : MonoBehaviour 
{
    public GameObject Check = null;
    public GameObject rotan;
    GameObject rotanSwitch;


    void Start()
    {
        rotanSwitch = GameObject.Find("Lotan_Switch");
	}
	
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.tag == "Lotanswitch")
            {
                Invoke("RotanMoveDownMessage", 2.0f);
                Destroy(this.gameObject, 2.9f);
            }
            if (Check != null)
                Check.SetActive(false);
         
        }
    }

    void RotanMoveDownMessage()
    {
        rotan.SendMessage("RotanMoveDown", SendMessageOptions.DontRequireReceiver);
    }
 
}
