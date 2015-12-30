//using UnityEngine;
//using System.Collections;

//public class Stage1Direction : MonoBehaviour {
//    GameObject lamp;
//    LampInfo lampInfo;
//    Renderer lampRenderer;
//    GameObject lampEff;
//    GameObject effectTemp;
//    bool updateSwitch = false;
//    float fallingSpeed = 0.3f;
//    GameObject tutorialMgr;
//    bool dontControl = true;
//    public GameObject boneEffect;

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            //램프 탄생 이펙트 생성 및 램프에 붙이기
//            lamp = other.gameObject;
//            lampRenderer = lamp.renderer;
//            lamp.transform.rotation = Quaternion.identity;
//            lampRenderer.enabled = false;
//            lampEff = lamp.transform.FindChild("Effect").gameObject;
//            lampEff.SetActive(false);

//            //램프 조작 불가 및 물리 법칙 영향 안받게 변경
//            lampInfo = lamp.GetComponent<LampInfo>();
//            lamp.rigidbody.isKinematic = true;
//            lampInfo.setControl(false);

//            //LAMP 출력
//            Invoke("PrintPG", 1.0f);
//            Invoke("PrintLamp", 9.0f);
            
//            Invoke("CreateFallEffect", 4.5f);
//        }
//    }

//    void Update()
//    {
//        if (dontControl)
//        {
//            if(lampInfo != null)
//                lampInfo.setControl(false);
//        }
//        if (updateSwitch)
//        {
//            //1초에 10번씩 반복
//            Invoke("invokeUpdate", 0.1f);
//            if (lamp.transform.position.y <= -0.18f)
//            {
//                lamp.rigidbody.isKinematic = true;
//                updateSwitch = false;
//                lampRenderer.enabled = true;
//                effectTemp.GetComponent<Animator>().Play("LampBonePadeOut");
//                Invoke("StartTutorial", 2.5f);
//            }
//        }
//    }

//    void invokeUpdate()
//    {
//        ////개 짱나서 하드코딩.. ㅈㅅ
//        if (fallingSpeed - 0.02f >= 0.03f)
//        {
//            fallingSpeed -= 0.03f;
//        }
//        if (lamp.transform.position.y <= 2.5f)
//            fallingSpeed = 0.02f;
//        if (lamp.transform.position.y <= 2.0f)
//            fallingSpeed = 0.01f;
//        if (lamp.transform.position.y <= 1.0f)
//            fallingSpeed = 0.05f;
//        lamp.transform.Translate(new Vector3(0, -fallingSpeed, 0));
//    }

//    void CreateFallEffect()
//    {
//        effectTemp = (GameObject)Instantiate(boneEffect, lamp.transform.position, Quaternion.identity);
//        effectTemp.transform.parent = lamp.transform;
//        updateSwitch = true;
//        lampInfo.setControl(false);
//        lampEff.SetActive(true);
//    }

//    void PrintLamp()
//    {
//        UIController.PrintMessage("L A M P", 60);
//    }
//    void PrintPG()
//    {
//        UIController.PrintMessage("PlayGround\nStudio", 45);
//    }

//    void StartTutorial()
//    {
//        lamp.rigidbody.isKinematic = false;
//        dontControl = false;
//        lampInfo.setControl(true);
//        tutorialMgr = GameObject.Find("TutorialMgr");
//        //tutorialMgr.SendMessage("TouchLightTutorialOn");
//    }
//}
