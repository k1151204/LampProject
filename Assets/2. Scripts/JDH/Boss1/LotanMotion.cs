using UnityEngine;
using System.Collections;
public enum MOTION { MOVE,DASH };

public class LotanMotion : MonoBehaviour {

    //
    public float DashIngTime = 4f; //대쉬시 기달리는시간 
    public GameObject MoveEffect;

    public GameObject[] DashEffect;
    public GameObject[] DashReadyEffect;
    public GameObject[] JumpObject;
    public GameObject GoalObject;
	// Use this for initialization
    GameObject Lamp;
    LampInfo lampInfo;
    LotanHelpWindown helpwindow; // 도움창?
    int MoveDir; // 방향 0 이면 왼쪽으로 1이면 오른쪽으로
    int state; //현재 액션 상태
    float RotY;

    static bool Help = false; // 한번만 ?
    
    //Effect 회전값
    Quaternion LeftRot;
    Quaternion RightRot;
    bool PlayIng;
    bool test;
    bool Dash;
    float DashTime;
    float DashPlayTime;
    bool check ;

    /*톱니보스 사운드*/
    public AudioClip skillGo;
    public AudioClip skillSound;
    public AudioClip dieSound;

    public AudioSource boss;

    public void setRotY(float set)
    {
        RotY = set;
    }
    public void HitedLight()
    {
        if (!helpwindow.GetHelp ())
			helpwindow.HelpBossHpBarWindow ();
		else
			helpwindow.SetHpBarCheck (10);
    }
    void Start () {
        check = false;
        DashPlayTime = 6f;
        PlayIng = false;
        test = false;
        DashTime = 0.0f;
        Dash = false;
        Lamp = GameObject.Find("Lamp");
        lampInfo = Lamp.GetComponent<LampInfo>();
        helpwindow = GetComponent<LotanHelpWindown>();
        MoveDir = 0;
        state = (int)MOTION.MOVE;
         LeftRot = Quaternion.identity;
         RightRot = Quaternion.identity;
        LeftRot.eulerAngles = new Vector3(0, 90, 0);
        RightRot.eulerAngles = new Vector3(0, -90, 0);
        helpwindow.ReSetting();
        helpwindow.SetBossHp(100);
    }
    public int GetState()
    {
        return state;
    }
  
    // Update is called once per frame
    public void offMoveEffect() // 무브관련된 eff 정지
    {
        if(MoveEffect.activeSelf == true)
        MoveEffect.SetActive(false);
    }
    public void offDashEffect()
    {
        for (int i = 0; i < DashReadyEffect.Length; i++)
        {
            if (DashReadyEffect[i].activeSelf == true)
            DashReadyEffect[i].SetActive(false);
        }
        for (int i = 0; i < DashEffect.Length; i++)
        {
            if (DashEffect[i].activeSelf == true)
                DashEffect[i].SetActive(false);
        }
    }
    void Update()
    {
        transform.Rotate(0, RotY, 0);
        if (!PlayIng)
            return;
        if (!Dash)
        {
            DashTime += Time.deltaTime;
            if (DashTime > DashPlayTime)
            {
                Dash = true;
                DashTime = 0.0f;
            }

        }
        if (helpwindow.GetDieCheck())
        {
            if (check == false)
            {
                iTween.Stop(gameObject.transform.parent.gameObject);
                check = true;
            }
        }
    }

    public void RotanMove() // 이동
    {
        
        test = true;
        Hashtable hash = new Hashtable();
        MoveEffect.SetActive(true);
        if (MoveDir == 0)
        {
            hash.Add("x", gameObject.transform.parent.position.x - 0.5f);
            MoveEffect.transform.rotation = LeftRot;
            RotY = 10;
        }
        else if (MoveDir == 1)
        {
            hash.Add("x", gameObject.transform.parent.position.x + 0.5f);
            MoveEffect.transform.rotation = RightRot;
            RotY = -10;
        }
        hash.Add("name", "moveitween");
        hash.Add("speed", 0.8f);
        hash.Add("easetype", iTween.EaseType.linear);
        hash.Add("onupdatetarget", gameObject);
        hash.Add("onupdate", "DashIngCheck");
        hash.Add("oncompletetarget", gameObject);
        hash.Add("oncomplete", "RotanMoveEnd");

        state = 1;
        iTween.MoveTo(gameObject.transform.parent.gameObject, hash);
    }
    void RotanMoveEnd() //이동종료
    {

        test = false;
        state = -1;
        DirCheck();

        if (Dash)
        {
            RotanDashReady();
        }
        else
            RotanMove();
    }
    public void RotanDashReady() //대쉬준비
    {
        NewSoundMgr.instance.PlayBossSound(skillSound);
        //helpwindow.OnDashIngWindow();
        state = 2;
        if (MoveDir == 0)
            RotY = 20;
        else
            RotY = -20;
        for (int i = 0; i < DashReadyEffect.Length; i++)
        {
          //  DashReadyEffect[i].transform.position = transform.transform.position;
            DashReadyEffect[i].SetActive(true);

        }
        for (int i = 0; i < JumpObject.Length; i++)
        {
            JumpObject[i].SetActive(true);
        }
        Invoke("RotanDash", DashIngTime);
    }
  
    public void RotanDash() // 대쉬
    {
        NewSoundMgr.instance.PlayBossSound(skillGo);
        offDashEffect();
        for (int i = 0; i < DashReadyEffect.Length; i++)
        {
            //  DashReadyEffect[i].transform.position = transform.transform.position;
            DashReadyEffect[i].SetActive(false);
        }
        for(int i = 0 ; i <DashEffect.Length;i++)
        DashEffect[i].SetActive(true);
        Hashtable hash = new Hashtable();
        if (helpwindow.GetHelpWidwos() != 4)
        {
            if (MoveDir == 0)
            {
                hash.Add("x", -8);
            }
            else if (MoveDir == 1)
            {
                hash.Add("x", 0.3);
            }
            hash.Add("speed", 4.5f);
            hash.Add("easetype", iTween.EaseType.linear);
            hash.Add("oncompletetarget", gameObject);
            hash.Add("oncomplete", "RotanDashEnd");
            hash.Add("name", "Dashname");
            iTween.MoveTo(gameObject.transform.parent.gameObject, hash);
        }
    }
    public void RotanDashEnd() //종료
    {
        for (int i = 0; i < DashEffect.Length; i++)
            DashEffect[i].SetActive(false);
        for (int i = 0; i < JumpObject.Length; i++)
        {
            JumpObject[i].SetActive(false);
        }
        Dash = false;
        DashPlayTime = Random.Range(4f, 7f); //4 에서 6초사이로 대쉬
        if (MoveDir == 0)
            RotY = 10;
        else
            RotY = -10;
        state = -1;
       // helpwindow.OffDashIngWindow();
        DirCheck();
        RotanMove();

    }
    public void DashIngCheck()
    {
       
    }
    void DirCheck()
    {
        if (gameObject.transform.position.x <= Lamp.transform.position.x)
            MoveDir = 1;
        else
            MoveDir = 0;

    }
   
    void RotanMoveDown()
    {
        boss.enabled = true; // 사운드 킴
        if(!Help)
            helpwindow.OnHelp();
        RotY = 10;
        Hashtable hash = new Hashtable();
        hash.Add("y", -0.25f);
        hash.Add("easetype", iTween.EaseType.easeOutElastic);
        hash.Add("time", 4.0f);
        hash.Add("oncompletetarget", gameObject);
        hash.Add("oncomplete", "RotanMoveDownEnd");
        iTween.MoveBy(gameObject.transform.parent.gameObject,hash);
    }
    void RotanMoveDownEnd()
    {
		if (!Help) {
			GameObject.Find ("TextOpen1").GetComponent<ConnectStory>().StartStoryTelling();
			Invoke("Open",6.2f);
		}
		else
            RotanMove();
        this.GetComponent<Collider>().enabled = true;
        Help = true;
        PlayIng = true;
        helpwindow.OnHpBar();
    }
	void Open()
	{
		helpwindow.HelpTouchWindow ();
	}
}
