using UnityEngine;
using System.Collections;

public class Attack3Coll : MonoBehaviour 
{

	
 
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
            
        }
         
      
    }
   
}
