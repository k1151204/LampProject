using UnityEngine;
using System.Collections;

public class RestartButtonScript : MonoBehaviour {

    public UIController Controller;
    public GameObject PausePanel;
     LampInfo lampInfo;
    
    void OnClick()
    {
        if (!PausePanel.activeSelf)
        {
            if (lampInfo == null)
                lampInfo = GameObject.Find("Lamp").GetComponent<LampInfo>();
            //SoundManager.manager.playSound(SoundIndex.ClickSE);
            if (lampInfo.isPosibleControl())
            {
                Controller.UIBullet(true);
                Controller.SendMessage("enableMenu");
            }
        }
    }
}
