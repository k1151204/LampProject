using UnityEngine;
using System.Collections;

public class AxeScript : MonoBehaviour {

    public bool ActivateOnAwake = true;
    GameObject gameManager;
    public Animator avatar;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        if (ActivateOnAwake)
        {
            ActivateStart();
        }
        else
        {
            ActivateStop();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (avatar.GetBool("active"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void ActivateStart()
    {
        avatar.SetBool("active", true);
    }
    public void ActivateStop()
    {
        avatar.SetBool("active", false);
    }
}
