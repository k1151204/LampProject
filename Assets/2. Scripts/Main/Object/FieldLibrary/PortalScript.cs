using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour 
{
    public Transform[] connectPortal;
    public bool onlyPlayer = true;
    bool isRandom;
    ArrayList exceptObject;

    GameObject player;
    GameObject FadeInOut;

    public bool isDisposable = false;


	void Start () 
    {
        if (connectPortal.Length > 1)
        {
            isRandom = true;
        }
        else
        {
            isRandom = false;
        }
        exceptObject = new ArrayList();
        player = GameObject.Find("Lamp");
        FadeInOut = GameObject.Find("StoryTellerCanvas").transform.FindChild("_InGamePadeInOut").gameObject;
	}

    void FadeInOutOff()
    {
        FadeInOut.SetActive(false);
        player.GetComponent<LampMoveManager>().JoystickIng = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (connectPortal.Length >= 1)
        {
            if (onlyPlayer)
            {
                if (other.CompareTag("Player"))
                {
                    FadeInOut.SetActive(true);
                    Invoke("PortalDelay", 0.5f);
                }
            }
            else
            {
                warpTarget(player.gameObject);
            }
        }

    }

    void PortalDelay()
    {
        player.GetComponent<LampMoveManager>().JoystickIng = false;
        Invoke("FadeInOutOff", 2.5f);
        if (player.GetComponent<LampInfo>().isPosibleControl())
            warpTarget(player.gameObject);
    }


    void OnTriggerExit(Collider other)
    {
        //예외 목록에서 해당 오브젝트를 제거
        if (onlyPlayer)
        {
            if (other.CompareTag("Player"))
            {
                removeExceptObject(other.gameObject);
            }
        }
        else
        {
            removeExceptObject(other.gameObject);
        }

    }

    void warpTarget(GameObject target)
    {
        if (exceptObject.Contains(target) == false)
        {
            int index = 0;
            if (isRandom)
            {
                index = Random.Range(0, connectPortal.Length);
            }
            if (target.GetComponent<Rigidbody>() != null)
            {
                target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            target.transform.position = connectPortal[index].position;
            if (isDisposable == true)
            {
                Destroy(connectPortal[index].gameObject, 1.5f);
            }
            //CameraController.SetStickyMode();
            connectPortal[index].GetComponent<PortalScript>().addExceptObject(target);
        }
    }

    void addExceptObject(GameObject target) 
    { 
        exceptObject.Add(target);
    }

    void removeExceptObject(GameObject target)
    {
        if (exceptObject.Contains(target))
        {
            exceptObject.Remove(target);
        }
    }
}
