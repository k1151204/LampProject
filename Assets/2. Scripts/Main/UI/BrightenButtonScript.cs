using UnityEngine;
using System.Collections;

public class BrightenButtonScript : MonoBehaviour {
    public GameManager gameManager;

    void OnClick()
    { 
        gameManager.SendMessage("UseBrighten");
    }
}
