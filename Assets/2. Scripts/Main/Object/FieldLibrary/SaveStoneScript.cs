using UnityEngine;
using System.Collections;

public class SaveStoneScript : MonoBehaviour {
    public GameObject pointLight;
    public GameObject effect;
    GameObject gameManager;
    bool isActivate = false;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (isActivate == false)
            {
                LampInfo lampInfo = collider.gameObject.GetComponent<LampInfo>();
                if (lampInfo.isPosibleControl())
                {

                    ActivateSaveStone();

                    lampInfo.restoreLight();
                    gameManager = GameObject.Find("GameManager");
                    gameManager.SendMessage("UpdateSaveStone", transform.FindChild("Effect"));
                    if (GameObject.Find("StageInfo").GetComponent<LightCheck>() != null)
                    {
                        LightDataManager.SetNowLightCount(GameObject.Find("StageInfo").GetComponent<LightCheck>().SetMaxIndex());
                        GameObject.Find("StageInfo").GetComponent<LightCheck>().ReSetting();
                    }
                }
            }
        }
    }

    public void ActivateSaveStone()
    {
        if (isActivate == false)
        {
            effect.SetActive(true);
            isActivate = true;
        }
    }

    public bool isActivated() { return isActivate; }
}
