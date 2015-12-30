using UnityEngine;
using System.Collections;

public class EscapeToMain : MonoBehaviour 
{
    public UIPanel mainPanel;

    public AudioClip escapeButtonSound; // 2015-09-10 새로운 사운드 매니저 추가

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NewSoundMgr.instance.PlaySingle(escapeButtonSound); // 2015-09-10 새로운 사운드 매니저 추가
            GameObject.Find("TitleManager").GetComponent<TitleManager>().nowState = TitleState.main;
            this.gameObject.SetActive(false);
            mainPanel.gameObject.SetActive(true);
            for (int i = 0; i < GameObject.Find("3D_UI").transform.childCount; i++)
            {
                if (GameObject.Find("3D_UI").transform.GetChild(i).gameObject.name != "ShopUI")
                {
                    GameObject.Find("3D_UI").transform.GetChild(i).gameObject.SetActive(true);               
                }
            }
        }
    }
}
