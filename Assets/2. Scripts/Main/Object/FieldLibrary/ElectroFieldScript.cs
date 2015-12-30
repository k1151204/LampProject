using UnityEngine;
using System.Collections;

public class ElectroFieldScript : MonoBehaviour {
    GameObject gameManager;
    bool isActivate;
    MeshRenderer objRenderer;
    public bool ActivateOnAwake = true;

	void Start () {
        objRenderer = gameObject.GetComponent<MeshRenderer>();
        gameManager = GameObject.Find("GameManager");
        //if (gameManager.GetComponent<GameManager>().isEnableSE() == false)
        //{
        //    gameObject.GetComponent<AudioSource>().mute = true;
        //}

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
        if (isActivate)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void ActivateStart()
    {
        isActivate = true;
        objRenderer.enabled = true;
        //if (gameManager.GetComponent<GameManager>().isEnableSE())
        //{
        //    gameObject.GetComponent<AudioSource>().mute = false;
        //}
    }
    void ActivateStop()
    {
        isActivate = false;
        objRenderer.enabled = false;
        //if (gameManager.GetComponent<GameManager>().isEnableSE())
        //{
        //    gameObject.GetComponent<AudioSource>().mute = true;
        //}
    }
}
