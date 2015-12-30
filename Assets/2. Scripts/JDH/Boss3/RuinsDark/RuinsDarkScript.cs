using UnityEngine;
using System.Collections;

public class RuinsDarkScript : MonoBehaviour
{
    //애니메이션 상태
    enum pattern { Awaken, Stand, GroundWave, Miteor, SpinChase, DrainDark, Stern };
    AnimatorStateInfo currentBaseState;
    int[] animState = new int[7];
    int lastAnimation;
    public GameObject Goal; // 골인지점
    //컴포넌트 및 오브젝트
    GameObject lamp;
    LampInfo lampInfo;
    Animator anim;
    GameObject hitedLight;
    Transform parentTran;
    Transform lampTran;

    //패턴 이펙트
    Transform patternEff;
    public GameObject drainDarkEff;
    public GameObject groundWaveEff;
    public GameObject smokeExplosionEff;
    public GameObject smokeTrailEff;
    public GameObject metiorWaveEff;
    public GameObject NomalEff; // 애니메이션시작전
    public GameObject DieEff; // 죽을때

    //기타 변수
    int hitCnt = 0;
    float startPosX;
    int lastPattern = 0;
    int noticeCriticalPoint = 0;

    //- GroundWave 세기
    public float gw_power = 25.0f;
    public float gw_strongPower = 45.0f;
    public float gw_jumpPer = 0.1f;

    //- Stand 상태 시 원래 위치로 돌아오는 속도
    public float st_comeBackSpeed = 0.1f;

    //- SpinChase 속도
    public float sc_chaseSpeed = 0.1f;
    float sc_dir = 1;

    //- Metior
    public GameObject metiorObj;
    // hp bar
    bool Die;

    UIController BossUI;
    void Start()
    {
        BossUI = GameObject.Find("UIController").GetComponent<UIController>();
        //GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(1);
        anim = gameObject.GetComponent<Animator>();
        lamp = GameObject.Find("Lamp");
        lampInfo = lamp.GetComponent<LampInfo>();
        GameObject.Find("Main Camera").GetComponent<CameraController>().setCameraZoom(CameraZoom.zoomOut);
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(0.3f);
      
        hitedLight = transform.FindChild("HitedLight").gameObject;
        patternEff = transform.FindChild("PatternEff");
        parentTran = transform.parent;
        lampTran = lamp.transform;
        startPosX = parentTran.position.x;
        //애니메이션 상태 캐싱
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        animState[(int)pattern.Awaken] = Animator.StringToHash("Base Layer.Awaken");
        animState[(int)pattern.Stand] = Animator.StringToHash("Base Layer.idle");
        animState[(int)pattern.GroundWave] = Animator.StringToHash("Base Layer.groundWave");
        animState[(int)pattern.Miteor] = Animator.StringToHash("Base Layer.miteor");
        animState[(int)pattern.SpinChase] = Animator.StringToHash("Base Layer.spinChase");
        animState[(int)pattern.DrainDark] = Animator.StringToHash("Base Layer.drainDark");
        animState[(int)pattern.Stern] = Animator.StringToHash("Base Layer.stern");
        BossUI.BossHpReSetting();
        anim.speed = 0.0f;
        lastAnimation = animState[(int)pattern.Awaken];
        Die = false;
    }
    public int GetInfo()
    {
        return currentBaseState.nameHash;
    }
    public int GetPettern()
    {
        return animState[(int)pattern.DrainDark];
    }
    public bool GetDie()
    {
        return Die;
    }
    void Update()
    {
        if (Die)
            return;
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentBaseState.nameHash != lastAnimation)
        {
            //애니메이션 상태 빠져나갈 때 처리
            //if (lastAnimation == animState[(int)pattern.Stern])
            //{
            //    InitHitCnt();
            //}

            //애니메이션 상태 진입 시 처리
            if (currentBaseState.nameHash == animState[(int)pattern.Stand])
            {
                Invoke("SetRandomPattern", Random.Range(1f, 2.5f)); //1~2.5초 뒤 다음 공격 패턴으로 넘어감 (시간 변경함).
                hitCnt = 0;
            }
            else if (currentBaseState.nameHash == animState[(int)pattern.DrainDark])
            {
                anim.SetInteger("attackPattern", 0);
                GameObject eff = (GameObject)Instantiate(drainDarkEff, transform.position, Quaternion.identity);
                eff.transform.parent = patternEff;
                eff.transform.localPosition = Vector3.zero;

                if (++noticeCriticalPoint <= 5)
                {
                    //UIController.PrintMessage("Purify Now!");
                }
            }
            else if (currentBaseState.nameHash == animState[(int)pattern.SpinChase])
            {
                float distance = lampTran.position.x - parentTran.position.x;
                sc_dir = (distance > 0) ? 1 : -1;
            }
            lastAnimation = currentBaseState.nameHash;
        }


        //애니메이션 상태 업데이트
        if (currentBaseState.nameHash == animState[(int)pattern.Stand])
        {
            if (parentTran.position.x != startPosX)
            {
                float distance = startPosX - parentTran.position.x;
                float dir = (distance > 0) ? 1 : -1;

                if (Mathf.Abs(distance) > st_comeBackSpeed * Time.deltaTime)
                {
                    parentTran.position += Vector3.right * dir * st_comeBackSpeed * Time.deltaTime;
                }
                else
                {
                    parentTran.position = new Vector3(startPosX, parentTran.position.y, parentTran.position.z);
                }
            }
        }
        else if (currentBaseState.nameHash == animState[(int)pattern.SpinChase])
        {
            if (anim.GetBool("chase"))
            {
                if (2.0f < parentTran.position.x && parentTran.position.x < 15.5f)
                    parentTran.position += Vector3.right * sc_dir * sc_chaseSpeed * Time.deltaTime;
            }
        }
    }


    //등장 연출
    void EnterPlayer()
    {
        anim.speed = 1.0f;

        NomalEff.SetActive(false);
        BossUI.BossHpStart();
    }
    IEnumerator StartEffectText(UISprite sprite) //페이드인아웃
    {

        for (float i = 0.4f; i <= 1; i += 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(EndEffectText(sprite));
    }
    IEnumerator EndEffectText(UISprite sprite)
    {
        for (float i = 1f; i >= 0.4f; i -= 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(StartEffectText(sprite));
    }

    
    void AwakenShake() { CameraController.SetShake(0.1f, 0.1f); }
    void rowShake() { CameraController.SetShake(0.05f, 0.03f); }
    void PrintBossName() { UIController.PrintMessage("Ruin's Dark", 45); }


    //스탠드 상태
    void SetRandomPattern()
    {
        int pattern = Random.Range(1, 3 + 1);
        while (pattern == lastPattern)
        {
            pattern = Random.Range(1, 3 + 1);
        }
        anim.SetInteger("attackPattern", pattern);
        lastPattern = pattern;
    }

    public void HitedLight()
    {
        if (Die)
            return;
        if (currentBaseState.nameHash == animState[(int)pattern.DrainDark])
        {
            anim.SetFloat("nowHp", anim.GetFloat("nowHp") - 5f); //데미지
            if (lampInfo.getLight() < 100)
            {
                lampInfo.addLight(100f);
                BossUI.UpdateHpBar(-100f);
            }
            hitedLight.SetActive(true);
            hitedLight.GetComponent<ParticleSystem>().Play(true);
            hitedLight.GetComponentInChildren<ParticleSystem>().Play(true);
            CameraController.SetShake(0.05f, 0.05f);
            BossUI.BossHpDownSet(5);

            if (anim.GetFloat("nowHp") <= 0)
            {
                Die = true;
                //UIController.PrintMessage("Ruin s Dark Die", 45); // 2015-06-07 주석처리(호연)
                CameraController.SetShake(4f, 0.1f);
                Invoke("DieEffect", 4f);
                Invoke("DieBoss", 5f);
                GameObject.Find("Main Camera").GetComponent<CameraController>().target = transform;
                StopAllCoroutines();
                BossUI.BossHpEnd();

            }

            if (++hitCnt >= 5)
            {
                anim.SetTrigger("sternTrg");
            }
        }
    }
    void DieEffect()
    {
        DieEff.SetActive(true);

    }
    void DieBoss()
    {
        GameObject.Find("TextOpen").GetComponent<ConnectStory>().StartStoryTelling();
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = GameObject.Find("Lamp").transform;
        this.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        Invoke("GoEnd", 6f);
      
    }
    void GoEnd()
    {
        CameraController.SetShake(3.2f, 0.1f);

        GameObject.Find("TextOpen1").GetComponent<ConnectStory>().StartStoryTelling();

        Invoke("CreateGoal", 3.2f); 
    }
    void CreateGoal()
    {
        Goal.SetActive(true);
    }
    //GroundWave 시 땅을 찍을 때 램프를 당겨오는 기능
    void GroundWave_weak()
    {
        Vector3 dirVec = (transform.position - lamp.GetComponent<Rigidbody>().position).normalized;
        Vector3 resultVec = dirVec * gw_power * Time.deltaTime;
        resultVec.y = gw_power * gw_jumpPer;
        lamp.GetComponent<Rigidbody>().AddForce(resultVec, ForceMode.VelocityChange);
        CreateWaveEff();
    }
    void GroundWave_strong()
    {
        Vector3 dirVec = (transform.position - lamp.GetComponent<Rigidbody>().position).normalized;
        Vector3 resultVec = dirVec * gw_strongPower * Time.deltaTime;
        resultVec.y = gw_strongPower * gw_jumpPer;
        lamp.GetComponent<Rigidbody>().AddForce(resultVec, ForceMode.VelocityChange);
        CreateWaveEff();
    }
    //GroundWave 시 파동 이펙트 발생

    void CreateWaveEff()
    {
        GameObject eff = (GameObject)Instantiate(groundWaveEff, transform.position, Quaternion.identity);
        eff.transform.parent = patternEff;
        eff.transform.localPosition = new Vector3(0, -0.3f, 0);
    }

    //SpinChase
    void CreateSmokeExplosion()
    {
        GameObject eff = (GameObject)Instantiate(smokeExplosionEff, transform.position, Quaternion.identity);
        eff.transform.parent = patternEff;
        eff.transform.localPosition = Vector3.zero;
    }
    void CreateSmokeTrail()
    {
        GameObject eff = (GameObject)Instantiate(smokeTrailEff, transform.position, Quaternion.identity);
        eff.transform.parent = patternEff;
        eff.transform.localPosition = new Vector3(0, -0.9f, 0);
    }
    void EnableChase() { anim.SetBool("chase", true); }
    void DisableChase() { anim.SetBool(("chase"), false); }

    // Miteor
    void CreateMiteorWave()
    {
        GameObject eff = (GameObject)Instantiate(metiorWaveEff, transform.position, Quaternion.identity);
        eff.transform.parent = patternEff;
        eff.transform.localPosition = Vector3.zero;
    }
    void CreateMetior()
    {
        float offset = Random.Range(-2.5f, 2.5f);
        float posX = lamp.transform.position.x + offset;
        posX = Mathf.Clamp(posX, 1.8f, 15.8f);
        GameObject metior = (GameObject)Instantiate(metiorObj, transform.position, Quaternion.identity);
        metior.transform.parent = parentTran.parent;
        metior.transform.localPosition = new Vector3(posX, -3.86f, 2.0f);
    }

}
