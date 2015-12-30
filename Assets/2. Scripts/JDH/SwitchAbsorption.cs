using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
public class SwitchAbsorption : MonoBehaviour {
    public float PointLightCollider = 1.3f;
    public GameObject BaseObject; //임펙트의 시작위치가되는 오브젝트
    public GameObject TargetObject; // 빨려들어가는 임펙트가 가야하는 오브젝트
     GameObject EffectObject; //임펙트효과
     GameObject LastEffectObject = null;

     public AudioClip lightBoomSound;// 2015-09-10 새로운 사운드 매니저 추가


    public GameObject SetOnObject= null; // 없으면 NULL 넣어도됨 
    public string FuntionName = null; // 받고싶은 함수이름
    //
    public float MoveSpeed = 3; // 진행속도 // 높을수록 빠름
    public int Mode = 0; // 0 이면 세로 (커브) 1이면 가로 (직선)
    public GameObject Lotan;
    public GameObject Ruins;
    public GameObject Boss3;
	// Use this for initialization
    LampInfo Info;
    ParticleRenderer TempEffectInfo;

    GameObject TempEffect;

    public  bool BullCheck = false;
    bool OnCheck;

    public int SpringRotateCount = 0;
    bool IsSpringRotateCount = false;
    public GameObject SpringDoor = null;

    void Start () {
        EffectObject = LampCharChage.GetData.GetEff((int)LampChar.BULLET);
        LastEffectObject = LampCharChage.GetData.GetEff((int)LampChar.BULLETBOOM);
        if (this.tag == "Mirror")
            FuntionName = "MirrorBullet";
        else if (this.tag == "Door")
            FuntionName = "DoorBullet";
        else if (this.tag == "SpringDoor")
            FuntionName = "SpringDoorSwitch";

        OnCheck = false;
        SettingLamp();
        Info = GameObject.Find("Lamp").GetComponent<LampInfo>();   
	}

    void SettingLamp()
    {
        if (BaseObject == null)
            BaseObject = GameObject.Find("Lamp");
        if(TargetObject == null)
            TargetObject = GameObject.Find("Lamp");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "LightCheck")
        {
            if (gameObject.GetComponent<BoxCollider>() != null)
                gameObject.GetComponent<BoxCollider>().enabled = false;
            CreateEffect();
        }
    }
    public void Create()
    {
        if (BullCheck == true)
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        OnCheck = true;
        BullCheck = true;
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();

        if (Mode == 0)
            MoveEffect(FindPointCurve());
        else if (Mode == 1)
            MoveEffect(TargetObject.transform.position);

    }
    void LotanCollCheck()
    {
        if (BullCheck == true)
            return;
        if (!Info.isPosibleControl() || GetComponent<LotanMotion>().GetState() == 2)
            return;
        BullCheck = true;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();
            if (BaseObject.transform.position.y + 0.45 > TargetObject.transform.position.y && BaseObject.transform.position.y - 0.45 < TargetObject.transform.position.y)
            MoveEffect(TargetObject.transform.position);
        else
            MoveEffect(FindPointCurve());

    }
    void Boss3CollCheck()
    {
        if (BullCheck == true)
            return;
        if (!Info.isPosibleControl() || GameObject.Find("Boss3").GetComponent<Boss3Motion>().GetState() != 0)
            return;
        BullCheck = true;

        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();
        TempEffect.transform.LookAt(TargetObject.transform);

        TempEffect.transform.Rotate(180, 0, 0);
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();

        MoveEffect(TargetObject.transform.position);

    }
    void DoorBullet()
    {
        if (BullCheck == true)
            return;
        if (!Info.isPosibleControl())
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        DoorEffect(TargetObject.transform.position);
    }
     void DoorEffect(Vector3 Path)
    {
        Hashtable ht = new Hashtable();
        ht.Add("position", Path);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);

        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "DoorEffectEnd");

        iTween.MoveTo(TempEffect, ht);
    }
     void DoorEffectEnd()
     {
         TargetObject.GetComponent<DoorObject>().Open(TempEffect);
     }
    void MirrorBullet()
    {
        if (BullCheck == true)
            return;
        if (!Info.isPosibleControl())
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        if (TargetObject.GetComponent<MirrorObject>().Check == true)
        {
            TempEffect.GetComponent<LightMoveBoom>().SetCamaerView(true);
            GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag(TempEffect);
        }
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        TempEffect.GetComponent<LightMoveBoom>().SetVector(TargetObject.transform.position);
        MirrorEffect(TargetObject.transform.position);


    }
    void RUCollCheck()
    {
        if (BullCheck == true)
            return;
        if (!Info.isPosibleControl())
            return;
        if (GameObject.Find("RuinsDark").GetComponent<RuinsDarkScript>().GetInfo() != GameObject.Find("RuinsDark").GetComponent<RuinsDarkScript>().GetPettern())
            return;
        if (TargetObject.GetComponent<RuinsDarkScript>().GetDie())
            return;
        BullCheck = true;

        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();

            MoveEffect(TargetObject.transform.position);


    }
    
    void CreateEffect()
    {
        if (BullCheck == true)
            return;
        if (OnCheck )
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        OnCheck = true;
        BullCheck = true;
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();

        if (Mode == 0)
            MoveEffect(FindPointCurve());
        else if (Mode == 1)
            MoveEffect(TargetObject.transform.position);

    }
    void CollCheck()
    {
        if (BullCheck == true)
            return;
        if (OnCheck || !Info.isPosibleControl())
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
            //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
            OnCheck = true;
            BullCheck = true;
            TempEffect.GetComponent<LightMoveBoom>().NoEffect();

            if(Mode == 0)
            MoveEffect(FindPointCurve());
            else if(Mode == 1)
                MoveEffect(TargetObject.transform.position);

    }
    Vector3[] FindPointCurve()
    {
        float x = (TargetObject.transform.position.x + BaseObject.transform.position.x);
        float x1 = (TargetObject.transform.position.x - BaseObject.transform.position.x)/2;
        float y = (TargetObject.transform.position.y + BaseObject.transform.position.y) / 2;
        float z = (TargetObject.transform.position.z + BaseObject.transform.position.z) / 2;
        Vector3[] ReturnVector = new Vector3[10];
        Vector3 TwoPoint = new Vector3(x, y, z);
        Vector3 ThreePoint = new Vector3(x1, y, z);
        Vector3[] GetCurvePoint = new Vector3[4];
        GetCurvePoint[0] = BaseObject.transform.position;
        GetCurvePoint[1] = TwoPoint;
        GetCurvePoint[2] = ThreePoint;
        GetCurvePoint[3] = TargetObject.transform.position;
        for (int i = 0; i < 10; i++)
        {
            Vector3 OneLine = GetCurvePoint[0] + (GetCurvePoint[1] - GetCurvePoint[0]) * ((i + 1) * 0.1f);
            Vector3 TwoLine = GetCurvePoint[1] + (GetCurvePoint[2] - GetCurvePoint[1]) * ((i + 1) * 0.1f);
            Vector3 ThreeLine = GetCurvePoint[2] + (GetCurvePoint[3] - GetCurvePoint[2]) * ((i + 1) * 0.1f);
            Vector3 GetPointOne = OneLine + (TwoLine - OneLine) * ((i + 1) * 0.1f);
            Vector3 GetPointTwo = TwoLine + (ThreeLine - TwoLine) * ((i + 1) * 0.1f);
            ReturnVector[i] = GetPointOne + ((GetPointTwo - GetPointOne) * (i + 1) * 0.1f);
        }
        return ReturnVector;
    }
    void MirrorEffect(Vector3 Path)
    {
        Hashtable ht = new Hashtable();
        ht.Add("position", Path);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);

        ht.Add("oncomplete", "MirrorEffectEnd");

        iTween.MoveTo(TempEffect, ht);
    }
    void MirrorEffectEnd()
    {
        TempEffect.GetComponent<LightMoveBoom>().SetZ(TargetObject.transform.position.z);
        TempEffect.GetComponent<LightMoveBoom>().DirCheck((int)TargetObject.GetComponent<MirrorObject>().MIrrorDir);
    }
    void MoveEffect(Vector3 Path)
    {
        Hashtable ht = new Hashtable();
        ht.Add("position", Path);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");

        iTween.MoveTo(TempEffect, ht);
    }
    void MoveEffect(Vector3[] Path)
    {
     
        Hashtable ht = new Hashtable();
        ht.Add("path", Path);
        ht.Add("easetype", iTween.EaseType.easeInOutSine);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);

        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(TempEffect, ht);

    }
    void NewLookAt()
    {

        if (TempEffect != null && TargetObject != null)
        {
            TempEffect.transform.LookAt(TargetObject.transform);
            TempEffect.transform.Rotate(180, 0, 0);
        }

        //if (TempEffectInfo.maxParticleSize <= 0)
        //    return;
        //TempEffectInfo.maxParticleSize -= 0.0015f;
    }
    void EndMove()
    {
        if (SetOnObject != null)
        {
            Light Lamp = GameObject.Find("Lamp").transform.FindChild("light").gameObject.GetComponent<Light>();
            SetOnObject.SetActive(!SetOnObject.activeSelf);
            if (Lamp != null)
            {
                SetOnObject.tag = "TestLight";
                //SetOnObject.GetComponent<Light>().color = Lamp.color;
                //SetOnObject.GetComponent<Light>().intensity = 8;
                //SetOnObject.GetComponent<Light>().range = 5;
                SetOnObject.GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;
                SetOnObject.AddComponent<SphereCollider>();
                SetOnObject.AddComponent<Rigidbody>();
                SetOnObject.GetComponent<SphereCollider>().radius = PointLightCollider;
                SetOnObject.GetComponent<SphereCollider>().isTrigger = true;
                SetOnObject.GetComponent<Rigidbody>().isKinematic = false;
                SetOnObject.GetComponent<Rigidbody>().useGravity = false;

                NewSoundMgr.instance.PlaySingle(lightBoomSound);// 2015-09-10 새로운 사운드 매니저 추가
            }
        }
        if (LastEffectObject != null)
            Instantiate(LastEffectObject, TargetObject.transform.position, Quaternion.identity);
        if (TargetObject != null)
        {
            if (TargetObject.tag == "boss")
            {
                if (TargetObject.gameObject == Lotan)
                    TargetObject.GetComponent<LotanMotion>().HitedLight();
                else if (TargetObject.gameObject == Ruins)
                {
                    TargetObject.GetComponent<RuinsDarkScript>().HitedLight();
                }

                else if (TargetObject.gameObject == Boss3)
                {
                    GameObject.Find("Boss3").GetComponent<Boss3Motion>().HitedLight();

                }
            }
            else if (TargetObject.transform.CompareTag("Monster"))
            {
                TargetObject.GetComponent<MoveMonster>().HitedLight();
            }
            else if (TargetObject.transform.CompareTag("Remove"))
            {
                TargetObject.GetComponent<DieCheck>().remove();
            }
            else
            {
                if (GameObject.Find("StageInfo").GetComponent<LightCheck>() != null)
                {
                    GameObject.Find("StageInfo").GetComponent<LightCheck>().OnCheck(); // 라이트닝체크체크
                }
            }
        }
        if (TempEffect != null)
        {
            TempEffect.GetComponent<LightMoveBoom>().LightEndMove();
        }
        BullCheck = false;
    }

    /* Stage1StartAction 스크립트 함수호출 부분(2015-05-22) - 호연 */
    void LightObjectTutorial()
    {
        if (BullCheck == true)
            return;
        if (OnCheck)
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        OnCheck = true;
        BullCheck = true;
        if (Mode == 0)
            MoveEffect(FindPointCurve());
        else if (Mode == 1)
            MoveEffect(TargetObject.transform.position);
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();
        TempEffect.GetComponent<LightMoveBoom>().SetDeletTime(10);

        GameObject Stage1StartActionFuntion = GameObject.Find("StartDirection");
        Stage1StartActionFuntion.SendMessage("LightObjectTutorialOff", SendMessageOptions.DontRequireReceiver);

    }
	void NextLightObjectTutorial()
	{
		if (BullCheck == true)
			return;
		if (OnCheck)
			return;
		TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
		TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
		//TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();
		
		TempEffect.transform.LookAt(TargetObject.transform);
		TempEffect.transform.Rotate(180, 0, 0);
		OnCheck = true;
		BullCheck = true;
		if (Mode == 0)
			MoveEffect(FindPointCurve());
		else if (Mode == 1)
			MoveEffect(TargetObject.transform.position);
		TempEffect.GetComponent<LightMoveBoom>().NoEffect();
		TempEffect.GetComponent<LightMoveBoom>().SetDeletTime(10);
		GameObject.Find ("TextOpen4").GetComponent<LightObjectTutorial> ().OffLight ();
	}



    /* 보석 색상 맞추기 퍼즐 관련 */
    void GemMatchingPuzzle()
    {
        if (BullCheck == true)
            return;
        if (OnCheck || !Info.isPosibleControl())
            return;
        TempEffect = (GameObject)Instantiate(EffectObject, BaseObject.transform.position, Quaternion.identity);
        TempEffect.transform.parent = GameObject.Find("CreateObject").transform;
        //TempEffectInfo  = TempEffect.GetComponent<ParticleRenderer>();

        TempEffect.transform.LookAt(TargetObject.transform);
        TempEffect.transform.Rotate(180, 0, 0);
        TempEffect.GetComponent<LightMoveBoom>().NoEffect();

        if (Mode == 1)
        {
            GemMatchingPuzzle_MoveEffect(TargetObject.transform.position);
        }
    }

    void GemMatchingPuzzle_MoveEffect(Vector3 Path)
    {
        Hashtable ht = new Hashtable();
        ht.Add("position", Path);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "GemMatchingPuzzle_EndMove");

        iTween.MoveTo(TempEffect, ht);
    }

    void GemMatchingPuzzle_EndMove()
    {
        if (SetOnObject != null)
        {
            SetOnObject.SetActive(true);
            NewSoundMgr.instance.PlaySingle(lightBoomSound);// 2015-09-10 새로운 사운드 매니저 추가
        }
        if (TempEffect != null)
        {
            TempEffect.GetComponent<LightMoveBoom>().LightEndMove();
        }
    }

}
