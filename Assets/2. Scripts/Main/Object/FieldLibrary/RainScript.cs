using UnityEngine;
using System.Collections;

public class RainScript : MonoBehaviour {
    GameObject gameManager;

	void Start () {
        gameManager = GameObject.Find("GameManager");
        if (gameManager.GetComponent<GameManager>().isEnableSE() == false)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }
	}
}
