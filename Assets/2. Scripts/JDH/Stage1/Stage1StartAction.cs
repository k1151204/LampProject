using UnityEngine;
using System.Collections;

public class Stage1StartAction : MonoBehaviour
{
    GameObject lamp; // Player
    GameObject lampEffect; // Player 기본 이펙트
    GameObject boneEffect; // Player 1Stage 연출 이펙트 최상위 객체

    Animator boneEffect1; // Player 처음 떨어질때 이펙트
    GameObject boneEffect2; // Player 땅과 충돌 부터 이펙트
    Animator boomFlash; // Player 땅과 충돌 시 플래시 효과

    Animator absorptionEffect; // 점등 오브젝트 안으로 들어가는 이펙트

    bool updateSwitch = false;
    bool dontControl = true;

    LampInfo lampInfo;
    Renderer lampRenderer;

    GameObject brighten; // 브라이튼 스킬
    public Transform target1; // 첫번째 카메라 타겟

    GameObject touchLightTutorial;
    bool IsTouchLightTutorial = false;
    public GameObject lightObject; // LightObject 점등 오브젝트
    public GameObject lightObjectTutorial; // 점등 오브젝트 터치 튜토리얼
    public GameObject lightObjectTutorialPoint;

    public GameObject lampDownPos; // 점등 위치
    public GameObject pointLight; // 점등 조명
     GameObject RightTo;
    GameObject joyStickTutorial; // 조이스틱 튜토리얼 

    bool JoyStickCheck;
    LampMoveManager MoveManager;

    public AudioClip openningLampDropSound; // 2015-09-10 새로운 사운드 매니저 추가

    bool RightJoy = false;
    bool LeftJoy= false;

    GameObject joyStick;
    [HideInInspector]
    public GameObject jumpButton;
    [HideInInspector]
    public GameObject brightenButton;

    void Awake()
    {
        GameObject.Find("Lamp").GetComponent<CreateBoneLamp>().Create();
        MoveManager = GameObject.Find("Lamp").GetComponent<LampMoveManager>();
    }
    public bool GetJoyStickCheck()
    {
        return JoyStickCheck;
    }
    void Start()
    {
        jumpButton = GameObject.Find("JumpButtonImage");
        joyStick = GameObject.Find("JoyStick");
        brightenButton = GameObject.Find("BrightenOnButton");
        jumpButton.SetActive(false);
        joyStick.SetActive(false);
        brightenButton.SetActive(false);
    }

    void StartText()
    {
        GameObject.Find("TextOpen1").GetComponent<ConnectStory>().StartStoryTelling();
        
    }
    void Update()
    {
        if (dontControl)
        {
            if (lampInfo != null)
            {
                lampInfo.setControl(false);
            }
        }

        if (IsTouchLightTutorial == true) // 터치 라이트 튜토리얼 활성화 확인
        {
            if (Input.GetMouseButton(0)) // 화면 터치
            {
                Invoke("StartText", 0.8f);
                IsTouchLightTutorial = false;
                touchLightTutorial.SetActive(false); // 화면터치 튜토리얼 끄기
                Invoke("LightObjectTutorialOn", 2f); // 라이트 오브젝트 튜토리얼 켜기
            }
        }
        if (JoyStickCheck)
        {
            if (MoveManager.GetJoystickWeight() < 0)
            {
                LeftJoy = true;
            }
            else if (MoveManager.GetJoystickWeight() > 0)
            {
                RightJoy = true;
            }
            if (LeftJoy == true || RightJoy == true)
            {
                JoyStickCheck = false;
            }
        }
       
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            lamp = other.gameObject;
            lampRenderer = lamp.GetComponent<Renderer>();
            lamp.transform.rotation = Quaternion.identity;
            lampRenderer.enabled = false;
            lampEffect = lamp.transform.FindChild("Effect").gameObject;
            lampEffect.SetActive(false);
            boneEffect = lamp.transform.FindChild("BoneLamp").gameObject;

            boneEffect1 = boneEffect.transform.FindChild("1").GetComponent<Animator>();
            boneEffect2 = boneEffect.transform.FindChild("2").gameObject;
            // 램프 조작 불가 및 물리 법칙 정지
            boneEffect1.gameObject.SetActive(true);
            lampInfo = lamp.GetComponent<LampInfo>();
            lamp.GetComponent<Rigidbody>().isKinematic = true;
            lampInfo.setControl(false);

            Invoke("CreateFallEffect", 2.5f);
            
        }
    }

    void CreateFallEffect()
    {
        boneEffect.SetActive(true);
        updateSwitch = true;
        lampInfo.setControl(false);
        lampEffect.SetActive(false);
        if (brighten != null)
            brighten.SetActive(false);
        NewSoundMgr.instance.PlaySingle(openningLampDropSound);// 2015-09-10 새로운 사운드 매니저 추가
    }

    public void StartLampEvent()
    {
        Debug.Log("eWQEQW");
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetTarget(target1);
    
        JoyStickCheck = false;
        brighten = GameObject.Find("Brighten"); // 브라이튼 스킬
        Invoke("invokeUpdate", 2.5f);
        Invoke("LampEffectFadeOut", 9.0f);
        Invoke("LampBoomEffect", 10.0f);
        Transform ui = GameObject.Find("Interface").transform;
        Transform tutorial = ui.transform.FindChild("Tutorial");
        touchLightTutorial = tutorial.FindChild("TouchLightTutorial").gameObject;
        joyStickTutorial = tutorial.FindChild("JoyStickTutorial").gameObject;
        RightTo = GameObject.Find("RightTo").transform.GetChild(0).gameObject;
        MoveManager.TJump(false);
        RightTo.SetActive(false);
    }
    void invokeUpdate()
    {

        iTween.MoveBy(lamp.gameObject, iTween.Hash("y", -26.0f, "time", 7.0f, "delay", 0.1f, "easeType",
                                                    iTween.EaseType.linear));
    }
    
    void LampEffectFadeOut()
    {
        boneEffect1.enabled = true;
    }
   
    void LampBoomEffect()
    {
        dontControl = false;
        lampInfo.setControl(true);
        boneEffect2.SetActive(true);
        Invoke("TouchLightTutorialOn", 3.7f);
        // 브라이튼 스킬 비활성화
        //if (brighten != null)
        //brighten.transform.FindChild("Effect").GetComponent<SphereCollider>().enabled = false;
        // 플래시 효과
        boomFlash = this.gameObject.transform.FindChild("Flash").GetComponent<Animator>();
        boomFlash.enabled = true;
    }

    void StartMove()
    {
        boneEffect2.SetActive(false);
    }

    /* ___________________튜토리얼___________________ */

    // 터치 라이트 튜토리얼 함수
    void TouchLightTutorialOn()
    {
        IsTouchLightTutorial = true;
        touchLightTutorial.SetActive(true); // 터치 라이트 튜토리얼
    }
    void LightObjectTutorialPointStart()
    {
        lightObjectTutorialPoint.SetActive(true);
        lightObjectTutorial.SetActive(true);
    }

    // 라이트 오브젝트 튜토리얼 함수
    void LightObjectTutorialOn()
    {
        lightObject.GetComponent<Collider>().enabled = true;
        Invoke("LightObjectTutorialPointStart", 1.9f);
    }

    void LightObjectTutorialOff()
    {
        boneEffect2.SetActive(false);
        lightObjectTutorialPoint.SetActive(false);
        Invoke("LampMove", 1.5f);
        Invoke("LampDown2", 1.8f);
    }


    void LampMove()
    {
        lamp.transform.position = lampDownPos.transform.position;
        lamp.GetComponent<LampHpCreate>().SetMoveCheck(true);
    }

    void LampDown2()
    {
        lamp.GetComponent<LampHpCreate>().SetMoveCheck(true);

        lightObjectTutorial.SetActive(false);      
        Invoke("LampRendererOn", 2.0f);
    }

    void LampRendererOn()
    {
        lamp.GetComponent<LampHpCreate>().SetMoveCheck(true);
        lamp.GetComponent<Rigidbody>().isKinematic = false;
        lampEffect.SetActive(true);
        RightTo.SetActive(true);
        Invoke("LampRendOn", 0.01f);
        Invoke("JoyStickTutorial", 3.5f);
        if (brighten != null)
            brighten.SetActive(true);
    }

    void LampRendOn()
    {
        //lamp.GetComponent<Animator>().enabled = true;
        lampRenderer.enabled = true;
    }
    void JoyStickTutorial()
    {
        JoyStickCheck = true;
        joyStickTutorial.SetActive(true);
        joyStick.SetActive(true);
        for (int i = 0; i < 2; i++)
            joyStickTutorial.transform.GetChild(i).gameObject.SetActive(true);
    }
}
