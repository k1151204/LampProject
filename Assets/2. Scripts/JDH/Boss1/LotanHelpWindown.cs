using UnityEngine;
using System.Collections;

public class LotanHelpWindown : MonoBehaviour { // 도움창 및 체력바같은 UI 관리
    public GameObject Goal;
    public GameObject[] EndEffect;
     int BossHp ;
    GameObject TouchHelp;
    GameObject HpBarHelp;
    GameObject StartHelp;
    GameObject DashHelp;
  
    LampMoveManager JoytickState; // 조이스틱 상태를위해
    int NowHelpWindow = 0; //현재 상태 1이면 터치! 2이면 보스bar 3이면 시작전! 4이면 엔딩
    float chagetime = 0;
    UIController BossUI;
    bool DieCheck;

    public AudioSource bossPanSoundOff;

	// Use this for initialization
    void Awake()
    {
        BossUI = GameObject.Find("UIController").GetComponent<UIController>();
        DieCheck = false;
    }
	void Start () 
    {

    }
    public bool GetDieCheck()
    {
        return DieCheck;
    }
    public void ReSetting()
    {
        JoytickState = GameObject.Find("Lamp").GetComponent<LampMoveManager>();
        TouchHelp = GameObject.Find("LotanHelp").transform.FindChild("TouchLabel").gameObject;
        HpBarHelp = GameObject.Find("LotanHelp").transform.FindChild("BossHpBar").gameObject;
        StartHelp = GameObject.Find("LotanHelp").transform.FindChild("StartHelp").gameObject;
        DashHelp = GameObject.Find("LotanHelp").transform.FindChild("DashHelp").gameObject;
        HpBarHelp.SetActive(false);
        JoytickState.SetJoystick(true);
        TouchHelp.SetActive(false);
        StartHelp.SetActive(false);
        DashHelp.SetActive(false);
        BossUI.BossHpReSetting();

        NowHelpWindow = 0;
        chagetime = 0;
    }
    public void EndLotan() // 엔딩씬
    {
		GameObject.Find ("TextOpen2").GetComponent<ConnectStory>().StartStoryTelling();
        NowHelpWindow = 4;
        GetComponent<LotanMotion>().offMoveEffect();
        GetComponent<LotanMotion>().offDashEffect();
        //iTween.Stop();
        GameObject.Find("Main Camera").SendMessage("startCameraWalk", GameObject.Find("Lotan").GetComponent<SwitchScript>().targetObject.GetComponent<CameraWalkScript>().cameraWalk, SendMessageOptions.DontRequireReceiver);
        JoytickState.SetJoystick(false);
        TouchHelp.SetActive(false);
        StartHelp.SetActive(false);

        BossUI.BossHpEnd();
        StartCoroutine(EndBossRo());
        CameraController.SetShake(6f, 0.05f); // 
        DieCheck = true;
        Invoke("EndReady", 6f);
    }

    void EndReady()
    {
        EndEffect[0].SetActive(true);
        gameObject.SetActive(false);
        Invoke("EndLotanEffect", 1f);
    }
    void EndLotanEffect()
    {
        for (int i = 0; i < EndEffect.Length; i++)
        {
            EndEffect[i].SetActive(false);
        }

            //gameObject.SetActive(false);
        Goal.SetActive(true);
        JoytickState.SetJoystick(true);
    }
    IEnumerator EndBossRo()
    {

        for (float i = 10; i >= 0 ; i--)
        {
            GetComponent<LotanMotion>().setRotY(i);
            yield return new WaitForSeconds(0.3f); 
        }
    }
    public void SetBossHp(int _set)
    {
        BossHp = _set;
    }
    public int GetBossHp()
    {
        return BossHp;
    }
    public bool SetHpBarCheck(int _power)
    {
        BossHp -= _power ;
        BossUI.BossHpDownSet(_power);

        if (BossHp <= 0)
        {
            if (DieCheck == false)
            {
                bossPanSoundOff.enabled = false;
                EndLotan();
                return false;
            }
        }
        return true;
    }
    public void OnHpBar() //체력바 효과
    {
        BossUI.BossHpStart();
    }
    public void OnDashIngWindow()
    {
        DashHelp.SetActive(true);
        StartCoroutine(StartEffectText(DashHelp));
    }
    public void OffDashIngWindow()
    {
        StopCoroutine(StartEffectText(DashHelp));
        StopCoroutine(EndEffectText(DashHelp));
        DashHelp.SetActive(false);
    }
    public void OnHelp()
    {
        JoytickState.SetJoystick(false);
    }
    public void OffHelp()
    {
        JoytickState.SetJoystick(true);
    }
    public int GetHelpWidwos()
    {
        return NowHelpWindow;
    }
    public bool GetHelp()
    {
        return JoytickState.GetJoystick();
    }
    public void TouchManager()
    {
        if (NowHelpWindow == 1)
            HelpBossHpBarWindow();
    }
    // Update is called once per frame
    void Update()
    {
        if (NowHelpWindow != 0 || NowHelpWindow != 1)
        {

            if (NowHelpWindow == 2)
            {
                chagetime += Time.deltaTime;
                if (chagetime > 0.4f)
                {
                    HelpStartWindow();
                    chagetime = 0;
                }
            }
            else if (NowHelpWindow == 3)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(EndFadaIn(StartHelp));
                    NowHelpWindow = 0;
                    JoytickState.SetJoystick(true);
                    GameObject.Find("Lotan").GetComponent<LotanMotion>().RotanMove();

                }
            }
        }
    }

    public void HelpStartWindow()
    {

        StartHelp.SetActive(true);
        StartCoroutine(EndFadaIn(HpBarHelp));
        StartCoroutine(StartFadaIn(StartHelp));
        NowHelpWindow = 3;
    }
    public void HelpBossHpBarWindow()
    {
        //HpBarHelp.SetActive(true);
        StartCoroutine(EndFadaIn(TouchHelp));
        //StartCoroutine(StartFadaIn(HpBarHelp));
        NowHelpWindow = 2;
    }
    public void HelpTouchWindow()
    {
        TouchHelp.SetActive(true);
        StartCoroutine(StartFadaIn(TouchHelp));
        NowHelpWindow = 1;
    }
    IEnumerator StartEffectText(UISprite sprite)
    {

        for (float i = 0.5f; i <= 1; i += 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(EndEffectText(sprite));
    }
    IEnumerator EndEffectText(UISprite sprite)
    {
        for (float i = 1f; i >= 0.5f; i -= 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(StartEffectText(sprite));
    }
   //깜빡깜빡
    IEnumerator StartEffectText(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();
        for (float i = 0.5f; i <= 1; i += 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;
            yield return 0;
        }
        StartCoroutine(EndEffectText(DashHelp));
    }
    IEnumerator EndEffectText(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();
        for (float i = 1f; i >= 0.5f; i -= 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;
            yield return 0;
        }
        StartCoroutine(StartEffectText(DashHelp));
    }
    //
 // 페이드인아웃///
    IEnumerator StartFadaIn(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();
        for (float i = 0; i <= 1; i += 0.03f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;
            yield return 0;
        }

    }
    IEnumerator EndFadaIn(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();

        for (float i = 1f; i >= 0; i -= 0.03f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;

            yield return 0;
        }
    }

}
