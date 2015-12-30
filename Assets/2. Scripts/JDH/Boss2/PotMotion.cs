using UnityEngine;
using System.Collections;

public class PotMotion : MonoBehaviour {

    Animator anim ;
    // 오른쪽 공격 
    public GameObject[] RightObject;
    public GameObject[] LeftObject;

    public GameObject DieObject;
    //
    // 첫 연출 조명
    public GameObject[] startlight;

    // 공격 이펙트
    public GameObject[] attackEffet;
    //공격전 위험표시 이펙트
    public GameObject[] attackreadyEffect;
    //
    public GameObject attackreadyskill2;  //두번째 스킬쓸때 준비임펙트
    public GameObject PotUp; //머리

    public GameObject[] JumpObject;
    public GameObject Goal;
    //
    PotHelpWindow helpwindow;
    bool StartState;
    float AttackTime = 0;
    int Attackstate; //현재공격상태

    float NextAttackTime = 6f;

    int BossHp;
    bool BlacKHollCheck;
    float BlackSpeedTime;
    float BlackSpeed;//초기속도

    bool CollCheck; //충돌해야하나?

    bool EndCheck;
    bool RightLeft; // 오른쪽 왼쪽공격?

    int RightCount;
    int []RandArr; // 랜덤생성을위한변수


    int MotionCount;
    int Count;
    public bool GetEnd()
    {
        return EndCheck;
    }

    public void End()
    {

        if (BossHp > 0)
            return;
        GameObject.Find("Lamp").GetComponent<Rigidbody>().useGravity = true;
        helpwindow.OffAttackHelp();
        attackreadyEffect[0].SetActive(false);
        attackreadyEffect[1].SetActive(false);
        attackreadyskill2.SetActive(false);
        PotUp.SetActive(false);
        for (int i = 0; i < RightObject.Length; i++)
        {
            RightObject[i].SetActive(false);
            LeftObject[i].SetActive(false);
            RightObject[i].transform.FindChild("Go").gameObject.SetActive(false);
            LeftObject[i].transform.FindChild("Go").gameObject.SetActive(false);
        }
        iTween.Stop(this.gameObject);
        CollCheck = false;
        BlacKHollCheck = false;
        StartState = false;
        EndCheck = false;
        RightLeft = false;
        RightCount = 0;
        BossKillOff();
        DieObject.SetActive(true);
        GetComponent<Collider>().isTrigger = true;
        for (int i = 0; i < JumpObject.Length; i++)
            JumpObject[i].SetActive(false);
        Invoke("DIeStart", 3f);
        Invoke("DieEnd", 6f);
    }
    void Start()
    {
        MotionCount = 1;
        Count = 0;
        helpwindow = GetComponent<PotHelpWindow>();
        anim = gameObject.GetComponent<Animator>();
        GameObject.Find ("Main Camera").GetComponent<CameraController> ().zoomSizeList[2] = -3.5f;
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(0.7f);
        //GameObject.Find("Main Camera").GetComponent<CameraController>().target = GameObject.Find("view").transform;
        Attackstate = 1;
        AttackTime = 0;
        StartState = false;
        RightLeft = false;
        RightCount = 0;
        NextAttackTime = 9f;
        BossHp = 100;
        BlacKHollCheck = false;
        BlackSpeedTime = 0.0f;
        BlackSpeed = 0.02f;
        CollCheck = false;
        EndCheck = true;
        RandArr = new int[RightObject.Length];
        for (int i = 0; i < RightObject.Length; i++)
            RandArr[i] = i;
     
      
    }
    public bool GetCollCheck()
    {
        return CollCheck;
    }
    public int GetBossHp()
    {
        return BossHp;
    }
    public void SetBossHp(int _hp)
    {
        BossHp -= _hp;
    }
    // 시작시 관리
    void ReactionStart()
    {
      
        anim.enabled = true;
        Invoke("SpinStart", 1f);
        Invoke("SpinStop", 6f);
        helpwindow.OnHpBar();

        
    }
    void DieEnd()
    {
        Goal.SetActive(true);
        DestroyObject(gameObject);
    }
    void DIeStart()
    {
        anim.SetBool("Die", true);
        DieObject.SetActive(false);
        helpwindow.Off();
    }
    // Spin 상태
    void SpinStart() //공격전에는 항상 스핀 후 공격함
    {
        if (!EndCheck)
            return;

        anim.SetBool("Spin", true);
        if (Attackstate == 1)
        {

            AffectMotion();
            if (RightLeft)
            {
                rightaffackready();
                CameraController.SetShake(16f, 0.03f);
            }
            else
                CameraController.SetShake(3.5f, 0.03f);

        }
        else if (Attackstate == 2)
        {
            CameraController.SetShake(9f, 0.03f);

            AffectMotion2();
        }
        CollCheck = true;
    }
    void SpinStop()
    {

        if (!EndCheck)
            return;

        anim.SetBool("Spin", false);

        if (Attackstate == 1)
        {
            AffectMotionEnd();
            if (RightLeft)
            {
                RightCount = 0;
            }
        }
        else if (Attackstate == 2)
        {
            AffectMotionEnd2();
        }
        StartState = true;
        Attackstate = 0;
        AttackTime = 0.0f;
        NextAttackTime = Random.Range(4, 7); // 4 초와 6초사이에 한번씩 공격
        CollCheck = false;
    }

    public void DamagedStart()
    {
        anim.SetBool("Damaged", true);
        Invoke("DamagedStop", 1f);
    }
    public void DamagedStop()
    {
        anim.SetBool("Damaged", false);
    }
    //
    void AffectMotion() // 공격시 해줘야할꺼
    {
        attackreadyEffect[0].SetActive(false);
        attackreadyEffect[1].SetActive(false);

        BossSkill();
        PotUp.SetActive(true);
        PotUp.transform.FindChild("AttackRedayEffect").gameObject.SetActive(true);

    }
    void AffectMotionEnd() // 공격끝나고 
    {
        //helpwindow.OffAttackHelp();
        BossKillOff();
        PotUp.SetActive(false);
        PotUp.transform.FindChild("AttackRedayEffect").gameObject.SetActive(false);
        
        int tempindex = 0;
        for (int i = 0; i < RightCount; i++)
        {
            RandArr[i] = -1;
        }
        while (true)
        {
            int randtemp = Random.Range(0, RightObject.Length);
                int check = 0;
            for (int i = 0; i < tempindex; i++)
            {
                if (RandArr[i] == randtemp)
                {
                   check++;
                }
            }
            if (check == 0)
            {
                RandArr[tempindex] = randtemp;
                tempindex++;
            }
            if (tempindex == 4)
                break;
        }
    }

    void AffectMotion2() //두번째 패턴 공격 (블랙홀)
    {
        attackreadyskill2.SetActive(false);
        GameObject.Find("Lamp").GetComponent<Rigidbody>().useGravity = false;
        BlacKHollCheck = true;
        PotUp.SetActive(true);
        PotUp.transform.FindChild("BlackHollEffect").gameObject.SetActive(true);
        GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(false);

    }
    void AffectMotionEnd2() //블랙홀 공격끝나고 
    {
        GameObject.Find("Lamp").GetComponent<Rigidbody>().useGravity = true;
        //helpwindow.OffAttackHelp();
        PotUp.SetActive(false);
        PotUp.transform.FindChild("BlackHollEffect").gameObject.SetActive(false);

        Skill2MoveEnd();
        BlacKHollCheck = false;
        BlackSpeed = 0.02f;
        BlackSpeedTime = 0.0f;
        GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(true);

    }
    void OnTriggerEnter(Collider other)
    {
        if (!CollCheck)
            return;
        if (other.transform.tag == "Player")
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
    }

    void Update()
    {

        if (!StartState)
              return;
        if (EndCheck)
        {
            if (BlacKHollCheck)
            {
                GameObject.Find("Lamp").transform.position = Vector3.Lerp(GameObject.Find("Lamp").transform.position, transform.position, BlackSpeed);
                BlackSpeedTime += Time.deltaTime;
                if (BlackSpeedTime > 1.5f)
                {
                    BlackSpeed += 0.01f;
                    BlackSpeedTime = 0.0f;
                }
            }
            if (Attackstate == 0)
            {
                AttackTime += Time.deltaTime;
                if (AttackTime > NextAttackTime)
                {
                    Attackstate = 1; //현재상태를 공격상황으로
                    if (BossHp <= 65) // 피 가 65아래면 다공격!
                        RightLeft = true;

                    if (Count == MotionCount)
                    {
                        Attackstate = 2;
                    }
                    MotionCheck();
                }
            }
        }
    }
    void rightaffackend()
    {
        RightObject[RandArr[RightCount]].transform.FindChild("Go").gameObject.SetActive(false);
        RightObject[RandArr[RightCount]].SetActive(false);
        LeftObject[RandArr[RightCount]].transform.FindChild("Go").gameObject.SetActive(false);
        LeftObject[RandArr[RightCount]].SetActive(false);
        RightObject[RandArr[RightCount]].GetComponentsInChildren<ParticleEmitter>().Initialize();
        LeftObject[RandArr[RightCount]].GetComponentsInChildren<ParticleEmitter>().Initialize();

        RightCount++;

        rightaffackready();
    }
    void rightaffack()
    {
        RightObject[RandArr[RightCount]].transform.FindChild("Go").gameObject.SetActive(true);
        LeftObject[RandArr[RightCount]].transform.FindChild("Go").gameObject.SetActive(true);

        Invoke("rightaffackend", 2f);
    }
    void rightaffackready()
    {
        if (RightCount == RightObject.Length)
            return;
        RightObject[RandArr[RightCount]].SetActive(true);
        LeftObject[RandArr[RightCount]].SetActive(true);

        Invoke("rightaffack",2f);
    }
    void MotionCheck()
    {
        //helpwindow.OnAttackHelp();
        Invoke("SpinStart", 5f); // 5초동안 준비

        if (Attackstate == 1)
        {
            Count++;
            attackreadyEffect[0].SetActive(true);
            attackreadyEffect[1].SetActive(true);
            if (!RightLeft)
                Invoke("SpinStop", 10f);
            else
                Invoke("SpinStop", 22f); 

        }
        else if (Attackstate == 2)
        {
            Count = 0;
            MotionCount = Random.Range(1, 3);
            attackreadyskill2.SetActive(true);
            Skill2Move();
            Invoke("SpinStop", 15f); // 10초간 공격

        }

    }
    void BossSkill()
    {
        attackEffet[0].SetActive(true);
        attackEffet[1].SetActive(true);
    }
    void BossKillOff()
    {
        attackEffet[0].SetActive(false);
        attackEffet[1].SetActive(false);
    }
    //
    void Skill2Move() // 스킬발동시 보스 이동
    {
        Hashtable ht = new Hashtable();
        ht.Add("y",transform.position.y + 1.2 );
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 5);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(gameObject, ht);
    }
    void Skill2MoveEnd() // 스킬발동시 보스 이동
    {
        Hashtable ht = new Hashtable();
        ht.Add("y", transform.position.y - 1.2);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 2);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(gameObject, ht);
    }
    IEnumerator StartFadaIn(Light sprite)
    {
        for(float i = 4f; i >= 0 ; i-=0.01f)
        {
            sprite.range = i;
            yield return 0;
        }
    }
    IEnumerator EndFadaIn(Light sprite)
    {
        for (float i = 0f; i <= 4f; i += 0.01f)
        {
            sprite.range = i;
            yield return 0;
        }
    }
}
