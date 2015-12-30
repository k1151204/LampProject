using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

    GameObject gameManager;
    float waitTime;
    float timer;
    bool isSendClear;
    public bool use3DSound = true;
    public void Setting()
    {
        isSendClear = false;
        waitTime = 0.15f;
        timer = 0;
        gameManager = GameObject.Find("GameManager");
        if (gameManager.GetComponent<GameManager>().isEnableSE() == false || use3DSound == false)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }
    }
	// Use this for initialization
    void Start()
    {
        Setting();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(1, 0, 0);
	}

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            timer = 0;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                if (isSendClear == false)
                {
                    gameManager.SendMessage("stageClear", SendMessageOptions.DontRequireReceiver);
                    isSendClear = true;
                }
            }
        }
    }

    void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                if (isSendClear == false)
                {
                    gameManager.SendMessage("stageClear", SendMessageOptions.DontRequireReceiver);
                    isSendClear = true;
                }
            }
        }
    }

    void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            timer = 0;
        }
    }
}
