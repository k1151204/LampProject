using UnityEngine;
using System.Collections;

public class DeadZoneScript : MonoBehaviour {
    GameObject gameManager;
    public GameObject StopObject;
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
        else if (collider.gameObject.CompareTag("CollNoMove"))
        {
            ScreenLightObject Temp = collider.gameObject.GetComponent<ScreenLightObject>();
            if(Temp != null)
            {
                if (collider.gameObject.GetComponent<ScreenLightObject>().GetCheck() == 1)
                {
                    if (StopObject != null)
                    {
                        StopObject.GetComponent<NewStageMoveEvent>().Stop();
                    }

                }
            }
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
        else if (collider.gameObject.CompareTag("CollNoMove"))
        {
            ScreenLightObject Temp = collider.gameObject.GetComponent<ScreenLightObject>();
            if (Temp != null)
            {
                if (collider.gameObject.GetComponent<ScreenLightObject>().GetCheck() == 1)
                {
                    if (StopObject != null)
                    {
                        StopObject.GetComponent<NewStageMoveEvent>().Stop();
                    }

                }
            }
        }
    }

}
