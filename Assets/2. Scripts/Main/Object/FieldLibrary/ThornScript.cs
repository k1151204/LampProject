using UnityEngine;
using System.Collections;

public class ThornScript : MonoBehaviour {
    GameObject gameManager;
    float rotateY;
    bool isActivate;
    public bool ActivateOnAwake = true;
    public bool use3DSound = true;

    // Use this for initialization
    void Start()
    {
        rotateY = 0;
        gameManager = GameObject.Find("GameManager");
        if (gameManager.GetComponent<GameManager>().isEnableSE() == false || use3DSound == false)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }

        if (ActivateOnAwake)
        {
            ActivateStart();
        }
        else
        {
            ActivateStop();
        }
    }

    void Update()
    {
        if (isActivate)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, rotateY, 0));
            rotateY = (rotateY + 20.0f) % 360;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isActivate)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void ActivateStart()
    {
        isActivate = true;
        if (gameManager.GetComponent<GameManager>().isEnableSE())
        {
            if (use3DSound)
                gameObject.GetComponent<AudioSource>().mute = false;
        }
    }
    void ActivateStop()
    {
        isActivate = false;
        if (gameManager.GetComponent<GameManager>().isEnableSE())
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }
    }
}
