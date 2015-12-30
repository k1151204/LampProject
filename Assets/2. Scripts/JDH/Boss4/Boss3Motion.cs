using UnityEngine;
using System.Collections;
public class Boss3Motion : MonoBehaviour { //노말 상태에서만 공격가능하게 구현..
    public enum MOTION { NOMEL = 0, ATTACK1 = 1, ATTACK2 = 2,ATTACK3 = 3,ATTACK4 = 4,DEMAGED = 5,DIE  = 6,NEXT = 7};

    public GameObject CreateBossEff;
    public GameObject Attack1Eff;
    public GameObject Attack1ReadyEff;
    public GameObject Attack1BoomEff;
    public GameObject LastBoomEff;
    // 패턴 3 단일공격을 위한 임펙트들

    // 패턴2 전범위 공격을 위한 임펙들

    public GameObject ShieldEff;
    public GameObject AllAttackEff;
    public GameObject[] AllatackVectorTemp;
    Vector3 Temp; //
    Vector3 BulletTemp;
    GameObject Bullet;

    // 패턴 3 몬스터 생성?

    public GameObject Door;
    public GameObject CreateDoorEffect;
    // 다음 페이지

    public GameObject NextEff;
    // 패턴4 윈드 
    public GameObject[] WindVertor;
    public GameObject WindReadyEff;
    public GameObject WindEff;
    public float Power = 450f;
	// Use this for initialization
    GameObject[] AllTempEff;
    Animator anim;
    GameObject Lamp;
    Quaternion curRot;  
    float RotTime;
    float CreateTime; // 패턴 타임
    float TempTime; //시간 체크를위한 변수
    bool BossIng; // 보스중인지?
    bool BossNextIng; // 다음페이지를 위한 변수
    bool BossEndIng; // 끝날때
    int AttackCount; // 노말상태에서 최대공격횟수 4로 잡음
    int BossHp;
    int MotionState;

    int AllAttackCount;
    int DoorCreateCount; // 도어 패턴 생성 카운터
    int DoorCount;   // 전맵공격할때 마다 한개씩증가
    int Count;

    /*2015-09-10 새로운 사운드 매니저 추가*/
    public AudioClip guardianstart_Sound;
    public AudioClip guardianspin_1_Sound;
    public AudioClip guardianspin_2_Sound;
    public AudioClip guardianshoot_Sound;
    public AudioClip guardianbossdie_Sound;


    void Start () {
        BossIng = false;
        BossNextIng = false;
        MotionState = 0;
        anim = gameObject.GetComponent<Animator>();
        Lamp = GameObject.Find("Lamp");
        AllTempEff = new GameObject[6];
        TempTime = 0;
        RotTime = 0;
        CreateTime = 1.5f;
        curRot = transform.rotation;
        MotionState = (int)MOTION.NOMEL;
        BossHp = 100;
        AttackCount = 0;
        AllAttackCount = 2; // 범위공격이 몇번의 단일공격하고 해야하는가?
        Count = 0; // 현재 단일공격횟수
        DoorCount = 0;
        DoorCreateCount = 1;
        BossEndIng = false;
        Invoke("BossSrartSoundOn", 6.0f);
	}
    void BossSrartSoundOn()
    {
        NewSoundMgr.instance.PlayBossSound(guardianstart_Sound);// 2015-09-10 새로운 사운드 매니저 추가
    }

    // 씬 시작후 하는함수
    public int GetState()
    {
        return MotionState;
    }
    public void SetCreateEff()
    {
        CreateBossEff.SetActive(true);
    }
    public void StratBoss() // 보스시작
    {
        BossIng = true;
    }
    public void SetBossHp(int _hp)
    {
        BossHp = _hp;
    }
    public int GetBossHp()
    {
        return BossHp;
    }
    public bool GetBossNextIng()
    {
        return BossIng;
    }
    public bool GetBossEndIng()
    {
        return BossEndIng;
    }
	// Update is called once per frame
    void Update()
    {
        if (BossEndIng) // 죽을때 조건
        {
            if (Lamp.transform.position.y < -7f)
            {
                GameObject.Find("GameManager").SendMessage("stageClear", SendMessageOptions.DontRequireReceiver);
                BossEndIng = false;
            }
        }
        if (!BossIng)
        {
            if (BossEndIng == true || BossNextIng == true)
            {
                if (GameObject.Find("BossManagerEff").transform.childCount != 0)
                {
                    Transform[] test = GameObject.Find("BossManagerEff").GetComponentsInChildren<Transform>();
                    for (int i = 1; i < test.Length; i++)
                        DestroyObject(test[i].gameObject);
                }
            }
            return;
        }
        if (Lamp.transform.position.y < -3f) // 평상시 떨어질때
        {
            //UIController.PrintMessage("", 45);
            CameraController.Endshake();
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);

        }
        if (MotionState == (int)MOTION.NOMEL)
        {
                NomelState();

            TempTime += Time.deltaTime;
            if (TempTime > CreateTime)
            {
                CreateMotion();
            }
        }

	}
  
    void CreateMotion() //모션만들기
    {
        CreateTime = Random.Range(1.8f, 2.8f); //2초와 3초사이로 랜덤으로 정하고
        TempTime = 0;
        RotTime = 0;
        curRot = transform.rotation;
        if (!BossNextIng && BossHp <= 50)
        {
            MotionState = (int)MOTION.NEXT;
        }
        else if (DoorCount >= DoorCreateCount)
            MotionState = (int)MOTION.ATTACK1;
        else if (Count >= AllAttackCount)
            MotionState = (int)MOTION.ATTACK2; // 모션 체크후 동작 
        else
        {
            int temp = 0;
            if (BossNextIng)
                temp = Random.Range(0, 3);
            if (temp == 0)
                MotionState = (int)MOTION.ATTACK3;
            else
                MotionState = (int)MOTION.ATTACK4;
        }

        switch (MotionState)
        {
            case (int)MOTION.ATTACK4:
                Attack4State(); //윈드
                break;
            case (int)MOTION.ATTACK1: // 도어 모션
                Attack1State();
                break;
            case (int)MOTION.ATTACK2:
                Attack2State(); //전체공격
                break;
            case (int)MOTION.ATTACK3:
                Attack3State(); // 단일공격
                break;
            case (int)MOTION.NEXT:
                NextSceen();
                break;
         
        }
        AttackCount = 0;
    }
    void NextSceen() // 피 일정이하 내려갈떄 효과
    {
        BossNextIng = true;
        BossIng = false;
        //UIController.PrintMessage("NEXT", 45);
        GameObject.Find("Lamp").GetComponent<LampInfo>().setControl(false);
        Invoke("EventOn", 1f);
        Invoke("BOXRemove", 1f);
        StartCoroutine(EndNext());
    }
    void EventOn()
    {
        CameraController.SetShake(3f, 0.06f); // 3초동안 모든애니메이션진행
        NextEff.SetActive(true);
    }
    IEnumerator EndNext()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("Lamp").GetComponent<LampInfo>().setControl(true);
        BossIng = true;
        ReSet();
    }

    void BOXRemove()
    {
        ExplodeObject[] test = GameObject.Find("Next").GetComponentsInChildren<ExplodeObject>();
        for (int i = 0; i < test.Length; i++)
            test[i].StartEffect();
    }
    void NomelState() // 노말상태일때
    {
        //UIController.PrintMessage("NOMAL ", 45);

        RotTime += Time.deltaTime * 5f;
        Quaternion lookRotation = Quaternion.LookRotation(Lamp.transform.position - transform.position);
        Quaternion rot = Quaternion.Slerp(curRot, lookRotation, RotTime);
        transform.rotation = rot;
    }
    void Attack4State() // 4번쨰 양쪽 박스무너진후 + 발동스킬
    {
        Count++;
        // 0.3 < 램프좌표 왼쪽 else 오른쪽
        WindReadyEff.SetActive(true);
        if (0.3f > Lamp.transform.position.x)
        {
            //UIController.PrintMessage("BOSS LEFT WIND!! ", 45);
            StartCoroutine(WindStart(WindVertor[1].transform));
        }
        else
        {
            //UIController.PrintMessage("BOSS RIGHT WIND!! ", 45);
            StartCoroutine(WindStart(WindVertor[0].transform));
        }
    }
    IEnumerator WindStart(Transform windtemp)
    {
        yield return new WaitForSeconds(3.5f);
        GameObject Temp = (GameObject)Instantiate(WindEff);
        Temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        Temp.transform.position = Lamp.transform.localPosition;
        WindReadyEff.SetActive(false);
        Vector3 dirVec = (windtemp.position - Lamp.GetComponent<Rigidbody>().position).normalized;
        Vector3 resultVec = dirVec * Power * Time.deltaTime;
        resultVec.y = 0.005f * Power;
        Lamp.GetComponent<Rigidbody>().AddForce(resultVec, ForceMode.VelocityChange);
        ReSet();
    }

    void Attack1State() // 첫번째 공격패턴일때 (도어)
    {
        //UIController.PrintMessage("Create DOOR ", 45);
        DoorCreateCount = Random.Range(1, 3); // 1번 ~ 2번; 전맵공격하고 발동하게
        DoorCount = 0;
        int index = Random.Range(0, 6);
        anim.SetInteger("State", 1); // 애니메이션 변경 후
        GameObject Temp = (GameObject)Instantiate(CreateDoorEffect);
        Temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        Temp.transform.position = AllatackVectorTemp[index].transform.position;
        GameObject temp = (GameObject)Instantiate(Door);
        temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        temp.transform.position = AllatackVectorTemp[index].transform.position;
        
        Invoke("ReSet", 2f); // 문열고 2초뒤에 ReSet( 평소상태로)
    }
    // 두번째 패턴 전맵 공격 + 쉴드 
  
    void Attack2State() // 두번째 공격패턴일때 (전맵공격)
    {
        Count = 0;
        DoorCount++;
        AllAttackCount = Random.Range(2, 4); // 1~2 단일공격후  전맵공격 
        anim.SetInteger("State", 2);
        //UIController.PrintMessage("BOSS3 ALL ATTACK", 45);


        int index = Random.Range(1, 5);
        GameObject temp = (GameObject)Instantiate(ShieldEff);
        temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        temp.transform.position = AllatackVectorTemp[index].transform.position;
        temp.SetActive(true);
        CameraController.SetShake(3f, 0.05f); // 

        StartCoroutine(CreateAllBullet(index));
        StartCoroutine(AllAttackEnd(temp));
        Invoke("ReSet", 4f);       
    }
    IEnumerator AllAttackEnd(GameObject T)
    {
        yield return new WaitForSeconds(6f);
        if (T != null)
            DestroyObject(T);
        for (int i = 0; i < AllatackVectorTemp.Length; i++)
        {
            if(AllTempEff[i] != null)
            DestroyObject(AllTempEff[i]);
        }
    }
    IEnumerator CreateAllBullet(int _index)
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < AllatackVectorTemp.Length; i++)
        {
            if (i == _index)
            {
                AllTempEff[i] = null;
                continue;
            }
            AllTempEff[i] = (GameObject)Instantiate(AllAttackEff);
            AllTempEff[i].transform.parent = GameObject.Find("BossManagerEff").transform;
            AllTempEff[i].transform.position = AllatackVectorTemp[i].transform.position;
            AllTempEff[i].SetActive(true);
        }
    }
    //
    void Attack3State() // 세번째 공격패턴일때
    {
        Count++;
        Temp = new Vector3(Lamp.transform.position.x, 0.09573476f, Lamp.transform.position.z);
        anim.SetInteger("State", 3);
        //UIController.PrintMessage("BOSS3 ATTACK", 45);
        Attack1ReadyEff.SetActive(true);
        Invoke("CreateBullet", 1f); // 기 모으는시간 1초
        Invoke("ReSet", 2f);
    }
   // 패턴1 총알발사
    void CreateBullet()
    {
        Hashtable ht = new Hashtable();
        Bullet = (GameObject)Instantiate(Attack1Eff);
        Bullet.transform.parent = GameObject.Find("BossManagerEff").transform;
        Bullet.transform.localPosition = new Vector3(0.290081f, -1.853253f, 1.886795f);
        Bullet.transform.localRotation = Quaternion.identity;
        Bullet.SetActive(true);

        ht.Add("position", Temp);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 1f); //1.5초안에 발사체날라가서
        ht.Add("oncompletetarget", gameObject);
        ht.Add("oncomplete", "BulletEnd");
        iTween.MoveTo(Bullet, ht);

        NewSoundMgr.instance.PlayBossSound(guardianshoot_Sound);// 2015-09-10 새로운 사운드 매니저 추가
    }
    void BulletEnd()
    {
        BulletTemp = Bullet.transform.position;
        GameObject Temp = (GameObject)Instantiate(Attack1BoomEff); //여기까지 3.5초
        Temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        Temp.transform.position = BulletTemp;
        DestroyObject(Bullet);
        Attack1Eff.SetActive(false);
        Attack1ReadyEff.SetActive(false);
        CreateBoom();
    }

    void CreateBoom() // 발사 터진후에 처리
    {
        GameObject Temp = (GameObject)Instantiate(LastBoomEff);
        Temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        Temp.transform.position = BulletTemp;
        Temp.transform.localRotation = Quaternion.identity;
        Invoke("CreateDirBoom", 1f);
        StartCoroutine(FiushBoom(Temp, null, null));
    }
    void CreateDirBoom() // 발사 좌우 터지는 임펙트
    {
        GameObject Temp1 = null;
        GameObject Temp2 = null;
        if (BulletTemp.x - 1f > -2.5f)
        {
            Temp1 = (GameObject)Instantiate(LastBoomEff);
            Temp1.transform.parent = GameObject.Find("BossManagerEff").transform;
            Temp1.transform.position = new Vector3(BulletTemp.x - 1f, BulletTemp.y, BulletTemp.z);
            Temp1.transform.localRotation = Quaternion.identity;
        }
        if (BulletTemp.x + 1f < 2.5f)
        {
            Temp2 = (GameObject)Instantiate(LastBoomEff);
            Temp2.transform.parent = GameObject.Find("BossManagerEff").transform;
            Temp2.transform.position = new Vector3(BulletTemp.x + 1f, BulletTemp.y, BulletTemp.z);
            Temp2.transform.localRotation = Quaternion.identity;
        }
        StartCoroutine(FiushBoom(null, Temp1, Temp2));


    }
    IEnumerator FiushBoom(GameObject T, GameObject T1, GameObject T2) // 붐 후 마무리 공격 
    {             // 마무리 후 
       
        yield return new WaitForSeconds(2f);
        if (T != null)
        {
            DestroyObject(T);
        }
        if (T1 != null)
        {
            DestroyObject(T1);
        }
        if (T2 != null)
        {
            DestroyObject(T2);
        }
    }
    // 총알발사 끝


    //
    void ReSet()
    {
        MotionState = (int)MOTION.NOMEL;
        anim.SetInteger("State", 0);
    }
    //공격햇을때
    void MoveNo()
    {
        if(BossIng)
        anim.SetInteger("State", 0); // 상태만 변환.
    }
    public void HitedLight()
    {

        DemagedState();
    }
    //
    void DemagedState() // 데미지 받을떄 상태
    {
        if (AttackCount < 5)
        {
            anim.SetInteger("State", 4);
            AttackCount++;
            BossHp -= 5;
            GetComponent<BossUI>().ShowHpBar(5);
            if (BossHp > 0)
                Invoke("MoveNo", 1f);
            else
                DieState();

        }
        else
        {
            ReSet();
        }
    }
    void DieState() // 죽을때 상태
    {
        NewSoundMgr.instance.PlayBossSound(guardianbossdie_Sound);
        BossEndIng = true;
        BossIng = false;
        StopAllCoroutines();
        
        Lamp.GetComponent<LampInfo>().setControl(false);
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = this.gameObject.transform;
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(0);
        GameObject.Find("Main Camera").GetComponent<CameraController>().zoomSizeList[2] = -3;
        CameraController.SetShake(6f, 0.05f); //  3초동안 흔들리고
        Invoke("End", 3f);
        Invoke("BossEnd", 5f);
        GetComponent<BossUI>().off();
    }
    public void BossEnd()
    {
        anim.SetInteger("State", 5);
        Invoke("GoText", 1f);
        Invoke("ReMoveBox", 12f);
      
    }
    void GoText()
    {
        GameObject.Find("TextOpen").GetComponent<ConnectStory>().StartStoryTelling();

    }
    void ReMoveBox()
    {
        ExplodeObject[] test = GameObject.Find("End").GetComponentsInChildren<ExplodeObject>();
        for (int i = 0; i < test.Length; i++)
            test[i].StartEffect();
    }
    public void End()
    {
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = Lamp.transform;
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(0.7f);
        GameObject.Find("Main Camera").GetComponent<CameraController>().zoomSizeList[2] = -2.5f;
        GetComponent<BossUI>().EndHpBar();
    }
  
}
