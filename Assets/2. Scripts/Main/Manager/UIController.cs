using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    GameObject gameInterface;
    public GameObject menu;
    public GameObject diePopUp;
    GameObject stageClear;
    GameObject touchLight;
    UILabel stageName;
    Animator screenAnim;
    static UILabel message;

    bool displayLightGauge = true;

    GameObject HpBar;
    GameObject HpBackBar;
    GameObject CountText;
    GameObject MainHp;
    GameObject MainHpEff;

    UISprite MainHpSprite;
    UISprite MainHpEffSprite;
    UISprite HpEffSprite;
    UISprite HpBackEffSprite;

    GameObject TouchEff;
    UIAnimation TouchEffAnimation;
    int FadeCheck;
    float number;


    GameObject BossHpBar;
    GameObject BossHpBackBar;
    GameObject BossHpBackBlackBar;
    GameObject newBossHpBackBlackBar;

    UISprite BossHpBarSpirte;
    UISprite BossHpBackBarSpirte;
    UISprite BossHpBackBlackBarSpirte;
    UISprite newBossHpBackBlackBarSpirte;

    //GameObject LampHpBar; // 램프 체력바
    bool Check;
    bool BulletCheck;
    bool MenuSeleteCheck;
    // Use this for initialization
    void Awake()
    {
        
        FadeCheck = 0;
        gameInterface = GameObject.Find("Interface");
        stageClear = gameInterface.transform.FindChild("StageClear").gameObject;
        touchLight = Resources.Load("MainGame/Prefab/Effect/Light") as GameObject;
        stageName = gameInterface.transform.FindChild("RightTopAnchor").FindChild("StageName").GetComponent<UILabel>();
        message = gameInterface.transform.FindChild("Message").GetComponent<UILabel>();
        screenAnim = gameInterface.transform.FindChild("Screen").GetComponent<Animator>();
        HpBackBar = GameObject.Find("LampHelp").transform.FindChild("HpBackBar").gameObject;
        HpBar = GameObject.Find("LampHelp").transform.FindChild("HpBar").gameObject;
        HpEffSprite = HpBar.GetComponent<UISprite>();
        HpBackEffSprite = HpBackBar.GetComponent<UISprite>();

        CountText = GameObject.Find("LampHelp").transform.FindChild("LampCount").gameObject;
        MainHp = GameObject.Find("LampHelp").transform.FindChild("MainHp").gameObject;
        MainHpEff = GameObject.Find("LampHelp").transform.FindChild("MainHpEff").gameObject;
        TouchEff = GameObject.Find("LampHelp").transform.FindChild("TouchEff").gameObject;
        BossHpBar = GameObject.Find("BossHp").transform.FindChild("HpBar").gameObject;
        BossHpBackBar = GameObject.Find("BossHp").transform.FindChild("HpBarBack").gameObject;
        BossHpBackBlackBar = GameObject.Find("BossHp").transform.FindChild("HpBarBlackBack").gameObject;
        newBossHpBackBlackBar = GameObject.Find("BossHp").transform.FindChild("newHpBar").gameObject;
        BossHpBarSpirte = BossHpBar.GetComponent<UISprite>();
        BossHpBackBarSpirte = BossHpBackBar.GetComponent<UISprite>();
        BossHpBackBlackBarSpirte = BossHpBackBlackBar.GetComponent<UISprite>();
        newBossHpBackBlackBarSpirte = newBossHpBackBlackBar.GetComponent<UISprite>();
        TouchEffAnimation = TouchEff.GetComponent<UIAnimation>();
        MainHpSprite = MainHp.GetComponent<UISprite>();
        MainHpEffSprite = MainHpEff.GetComponent<UISprite>();
        Check = false;
        BulletCheck = false;
        MenuSeleteCheck = false;
        number = 1f / 100f;
        BossHpBarSpirte.width = 430;
        BossHpBar.SetActive(false);
        BossHpBackBar.SetActive(false);
        BossHpBackBlackBar.SetActive(false);
        newBossHpBackBlackBar.SetActive(false);
    }
    public void BossHpReSetting()
    {
        BossHpBarSpirte.width = 430;
        BossHpBarSpirte.color = new Color(255, 255, 255, 0);
        BossHpBackBarSpirte.color = new Color(255, 255, 255, 0);
        BossHpBackBlackBarSpirte.color = new Color(255, 255, 255, 0);
        newBossHpBackBlackBarSpirte.color = new Color(255, 255, 255, 0);
        BossHpBar.SetActive(false);
        BossHpBackBar.SetActive(false);
        BossHpBackBlackBar.SetActive(false);
        newBossHpBackBlackBar.SetActive(false);

        Check = false;
    }

    public void BossHpStart()
    {
        Check = true;
        BossHpBar.SetActive(true);
        BossHpBackBar.SetActive(true);
        BossHpBackBlackBar.SetActive(true);
        newBossHpBackBlackBar.SetActive(true);
        BossHpBarSpirte.color = new Color(255, 255, 255, 255);
        BossHpBackBlackBarSpirte.color = new Color(255, 255, 255, 255);

        StartCoroutine(BossStartEffectText(BossHpBackBarSpirte));
        StartCoroutine(BossStartEffectText(newBossHpBackBlackBarSpirte));
    }
    public void BossHpEnd()
    {
        Check = false;
        BossHpBar.SetActive(false);
        BossHpBackBar.SetActive(false);
        BossHpBackBlackBar.SetActive(false);
        newBossHpBackBlackBar.SetActive(false);
    }
    public void BossHpDownSet(int _hp)
    {
        StartCoroutine(BossHpDown(_hp * 3));
    }
     IEnumerator BossHpDown(int _hp) // 피 다운 
    {
        if (_hp > 0)
        {
            for (int i = 0; i < _hp; i++)
            {
                if (BossHpBarSpirte.width > 150)
                {
                    BossHpBarSpirte.width -= 1;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }
        else
        {
            for (int i = _hp; i <= 0; i++)
            {
                if (BossHpBarSpirte.width < 430)
                {
                    BossHpBarSpirte.width += 1;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }
    }
    IEnumerator BossStartEffectText(UISprite sprite)
    {
        if (Check == false)
           yield  return 0;
        for (float i = 0.5f; i <= 1; i += 0.01f)
        {
           
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(BossEndEffectText(sprite));
    }
    IEnumerator BossEndEffectText(UISprite sprite)
    {
        if (Check== false)
            yield return 0;
        for (float i = 1f; i >= 0.5f; i -= 0.01f)
        {
         
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(BossStartEffectText(sprite));
    }
    public void TouchUIEff()
    {
        int a = Random.Range(1, 4);
        string Name = "HpBall_Effect" + a.ToString() + "_";

        TouchEffAnimation.SetName(Name);
        TouchEffAnimation.Go();
        Invoke("Stop", TouchEffAnimation.GetMaxTime());

    }
     void Stop()
    {
        TouchEffAnimation.Stop();
    }
     public void UIBullet(bool _set)
     {
         BulletCheck = _set;
     }
     public bool GetBullet()
     {
         return BulletCheck;
     }
    public void ReSet()
    {
        MainHpSprite.color = new Vector4(MainHpSprite.color.r, MainHpSprite.color.g, MainHpSprite.color.b,
                      0);
        MainHpEffSprite.color = new Vector4(MainHpEffSprite.color.r, MainHpEffSprite.color.g, MainHpEffSprite.color.b,
            0);
        HpEffSprite.color = new Vector4(HpEffSprite.color.r, HpEffSprite.color.g, HpEffSprite.color.b,
          1f);
        HpBackEffSprite.color = new Vector4(HpBackEffSprite.color.r, HpBackEffSprite.color.g, HpBackEffSprite.color.b,
          1f);
        //if(HpBar != null)
        //HpBar.GetComponent<UISprite>().width = 130;
        //HpBar 리셋
    }
    //메뉴창 제어
    public void disableMenu()
    {
        //menu.transform.FindChild("RestartButton").GetComponent<UIButton>().state = UIButtonColor.State.Normal;
        //menu.transform.FindChild("TitleButton").GetComponent<UIButton>().state = UIButtonColor.State.Normal;
        //menu.transform.FindChild("BackButton").GetComponent<UIButton>().state = UIButtonColor.State.Normal;
        if (MenuSeleteCheck == true)
            return;

        if(menu.activeSelf)
        {
            menu.GetComponent<Animator>().SetTrigger("Off");
        }
        else if (diePopUp.activeSelf)
        {
            diePopUp.GetComponent<Animator>().SetTrigger("Off");
        }
   
        Invoke("MenuOff", 1f);
        Time.timeScale = 1.0f;
    }

    void MenuOff()
    {
        BulletCheck = false;
        if (menu.activeSelf)
        {
            menu.gameObject.SetActive(false);
        }
        else if(diePopUp.activeSelf)
        {
            diePopUp.SetActive(false);
        }
    }
    public void enableMenu()
    {
        menu.gameObject.SetActive(true);
        MenuSeleteCheck = true;
        Invoke("TimeStop", 0.8f);
    }

    void TimeStop()
    {
        Time.timeScale = 0.0f;
        MenuSeleteCheck = false;
    }
    public void onoffMenu()
    {
        if (menu.gameObject.activeSelf)
        {
            disableMenu();
        }
        else
        {
            enableMenu();
        }
    }

    //클리어창 제어
    public void enableClearWindow() { stageClear.SetActive(true); }
    public void disableClearWindow() { stageClear.SetActive(false); }

    //라이트 게이지 제어

    //터치라이트 오브젝트 제어
    public void LightHpDown(float _power)
    {
        StartCoroutine(LowHpBar((int)(_power)));
    }
    public void CreateLightObject()
    {
        // 여기다가 hp 감소시키면될듯. /// 수치감소 
        //SoundManager.manager.playSound(SoundIndex.lightSE);
        //Vector2 mousePos = UICamera.currentTouch.pos;

        //Vector3 mousePosToWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1.5f));
        //Object lightClone = Instantiate(touchLight, mousePosToWorld, Quaternion.identity);
        //Destroy(lightClone, 1f);
    }

    public void CreateBrighten(float _power)
    {
        //// 수치 감소
        //StartCoroutine(LowHpBar((int)(_power )));
        //Debug.Log("eqweqwew");

        // 값 감소를 위한?
    }
    public void UpdateHpBar(float _power)
    {
        StartCoroutine(LowHpBar((int)(_power )));
    }
    IEnumerator EndEffectText(UILabel sprite)
    {
        for (float i = 1f; i >= 0.3f; i -= 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(ReEffectText(sprite));

    }
    IEnumerator EndEffectText(UISprite sprite)
    {
        for (float i = 1f; i >= 0.3f; i -= 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(ReEffectText(sprite));
    }
    IEnumerator ReEffectText(UILabel sprite)
    {

        for (float i = 0.3f; i <= 1f; i += 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
    }
    IEnumerator ReEffectText(UISprite sprite)
    {
        for (float i = 0.3f; i <= 1f; i += 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
    }
    IEnumerator LowHpBar(int _hp)
    {

        if (_hp > 0)
        {
            for (int i = 0; i < _hp; i++)
            {
                if (MainHpSprite.color.a < 1f)
                {
                    MainHpSprite.color = new Vector4(MainHpSprite.color.r, MainHpSprite.color.g, MainHpSprite.color.b,
                        MainHpSprite.color.a + number);
                    MainHpEffSprite.color = new Vector4(MainHpEffSprite.color.r, MainHpEffSprite.color.g, MainHpEffSprite.color.b,
                        MainHpEffSprite.color.a + number);
                }
                if (HpEffSprite.color.a > 0.3f)
                {
                    HpEffSprite.color = new Vector4(HpEffSprite.color.r, HpEffSprite.color.g, HpEffSprite.color.b,
                     HpEffSprite.color.a - number);
                    HpBackEffSprite.color = new Vector4(HpBackEffSprite.color.r, HpBackEffSprite.color.g, HpBackEffSprite.color.b,
                HpBackEffSprite.color.a - number);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {

            for (int i = 0; i > _hp; i--)
            {
                if (MainHpSprite.color.a > 0)
                {
                    MainHpSprite.color = new Vector4(MainHpSprite.color.r, MainHpSprite.color.g, MainHpSprite.color.b,
                         MainHpSprite.color.a - number);
                    MainHpEffSprite.color = new Vector4(MainHpEffSprite.color.r, MainHpEffSprite.color.g, MainHpEffSprite.color.b,
                          MainHpEffSprite.color.a - number);
                }
                if (HpEffSprite.color.a < 1f)
                {
                    HpEffSprite.color = new Vector4(HpEffSprite.color.r, HpEffSprite.color.g, HpEffSprite.color.b,
                  HpEffSprite.color.a + number);
                    HpBackEffSprite.color = new Vector4(HpBackEffSprite.color.r, HpBackEffSprite.color.g, HpBackEffSprite.color.b,
                HpBackEffSprite.color.a + number);

                }
                yield return new WaitForSeconds(0.01f);

            }
        }
    }
    //스테이지 이름 표시
    public void SetStageNameLabel(string name)
    {
        stageName.text = name;
    }
    public void DisplayStageName()
    {
        stageName.gameObject.SetActive(true);
        stageName.gameObject.GetComponent<Animator>().Play("PadeIn");
        Invoke("PadeOutStageName", 3.0f);
    }
    void PadeOutStageName()
    {
        stageName.gameObject.GetComponent<Animator>().Play("PadeOut");
        Invoke("DisableStageName", 2.0f);
    }
    void DisableStageName()
    {
        stageName.gameObject.SetActive(false);
    }

    //메세지 출력
    static public void PrintMessage(string text, int size = 45)
    {
        if (message != null)
        {
            message.gameObject.SetActive(true);
            message.text = text;
            message.fontSize = size;
            message.MakePixelPerfect();
            message.GetComponent<Animator>().Play("UILabelPadeIn");
        }
    }

    //스테이지 이동 연출
    public void PadeOutScreen()
    {
        if (screenAnim != null)
        {
            screenAnim.Rebind();
            if (FadeCheck == 0)
                screenAnim.Play("StagePadeOut");
            else
                screenAnim.Play("NextStagePadeOut");

            FadeCheck++;
        }
    }
    public void PadeInScreen()
    {
        if (screenAnim != null)
        {
            screenAnim.Rebind();
            if (FadeCheck != 0)
                screenAnim.Play("StagePadeIn");
        }
    }

}
