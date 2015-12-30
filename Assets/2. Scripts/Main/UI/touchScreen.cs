using UnityEngine;
using System.Collections;

public class touchScreen : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject mainCamera;
    float lastTouchTime = 0.0f;
    float timer = 0.0f;
    Transform lampTran;
    GameObject pfLightArrow;
    UIController UI;
    //클릭 시 해당 위치로 레이캐스트를 보내고 Brighten과 충돌했는지를 확인한다.
    //충돌했으면 브라이튼, 안했으면 터치라이트 사용
    void Start()
    {
        UI = GameObject.Find("UIController").GetComponent<UIController>();
        lampTran = GameObject.Find("Lamp").transform;
        pfLightArrow = (LampCharChage.GetData.GetEff((int)LampChar.BULLET));
      
    }
   
    void Update()
    {
        timer += Time.deltaTime;
    }
    void OnClick()
    {

        if (StoryTeller.Instanace.IsOpenedStoryTeller())
            return;
        if (timer - lastTouchTime > 0.5f)
        {
            timer = 0.0f;
            RaycastHit hit = new RaycastHit();

            Ray ray;
#if     UNITY_EDITOR
            ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
#else
                ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.touches[Input.touchCount-1].position);
#endif
            
            int layerMask = (1 << 14) | (1 << 13) | (1 << 12) | (1 << 11) | (1 << 10) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
           
                if (hit.collider.CompareTag("Selectable"))
                {
                    CreateLightArrow(hit.collider.transform.position);
                }
                else 
                {
                    SwitchAbsorption switchAbsorption = hit.collider.GetComponent<SwitchAbsorption>();
                    if (switchAbsorption)
                    {
                        hit.collider.SendMessage(hit.collider.GetComponent<SwitchAbsorption>().FuntionName, SendMessageOptions.DontRequireReceiver);
                    }
                }
                if(hit.collider.CompareTag("LightLaunchSwitch"))
                {
                    LightLaunchSwitch lightLaunchSwitch = hit.collider.GetComponent<LightLaunchSwitch>();
                    if(lightLaunchSwitch)
                    {
                        hit.collider.SendMessage(hit.collider.GetComponent<LightLaunchSwitch>().FuntionName, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            else 
            {
                if (UI.GetBullet() == true)
                    return;
                gameManager.SendMessage("UseTouchLight", SendMessageOptions.DontRequireReceiver);
            }
        }

    }

    void CreateLightArrow(Vector3 dest)
    {
        GameObject lightArrow = (GameObject)Instantiate(pfLightArrow, lampTran.position, Quaternion.identity);
        LightMoveBoom lightMoveBoom = lightArrow.GetComponent<LightMoveBoom>();
        lightMoveBoom.SetVector(dest);
        lightMoveBoom.Create();
    }

}