using UnityEngine;
using System.Collections;

public class LightMoveBoom : MonoBehaviour
{
    public GameObject BoomEff;
    LightButt UI;
    GameObject touchLight;
    Vector3 MoveVector3;

    GameObject TempGame;
    GameObject LAMP;

    public AudioClip lightArrowExplosionSound;// 2015-09-10 새로운 사운드 매니저 추가

    public bool BoomCheck = false;
    float MoveSpeed = 2f;
    bool CollCehck = true;
    bool NoMoveCheck = false;
    SphereCollider Collider;
    // Use this for initialization
    Vector3 TempLampVector;
    Vector3 TempDirVector;
    bool ReColl = false;
    bool ReturnCam = false;
    bool BulletDoorCheck = false;
    float DeletTime = 3f;
    float ExTime = 0f;
    bool UseIng;


    bool CamaerView = false;

    float TempZ;
    public void SetCamaerView(bool _cehck)
    {
        CamaerView = _cehck;
    }
    public void SetZ(float Z)
    {
        TempZ = Z;
    }
    public void SetDeletTime(float _time)
    {
        DeletTime = _time;
    }
    public void SetVector(Vector3 _vector3)
    {
        MoveVector3 = _vector3;
        TempDirVector = MoveVector3 - transform.position;
        
    }

    public Vector3 GetTempLampVector()
    {
        return TempLampVector;
    }
    public Vector3 GetTempDirVector()
    {
        return TempDirVector;
    }
    public void DirCheck(int _dir)
    {
        Vector3 TEMP;
        if (_dir == (int)MirrorDir.RIGHTLEFT)
        {
            TEMP = Vector3.Reflect(TempDirVector, Vector3.right);

        }
        else
        {
            TEMP = Vector3.Reflect(TempDirVector, Vector3.up);
        }
        SetVector(TEMP * 10);
        MirrorCreate();
        ReColl = true;

    }
    void Start()
    {
        touchLight = Resources.Load("MainGame/Prefab/Effect/Light") as GameObject;

        Collider = GetComponent<SphereCollider>();
        LAMP = GameObject.Find("Lamp");

        UseIng = false;
        BulletDoorCheck = false;
    }
    public void NoEffect()
    {
        CollCehck = false;

    }
    public void SetMoveSpeed(float set)
    {
        MoveSpeed = set;
    }
    public void StopMove()
    {
        iTween.Stop(this.gameObject);
    }
    public void BulletCreate(Vector3 _MoveVector3)
    {

        if (GameObject.Find("UIController") != null)
            UI = GameObject.Find("UIController").GetComponent<LightButt>();
        ExTime = 0;
        NoMoveCheck = false;
        if (LAMP != null)
            TempLampVector = LAMP.transform.position;
        if (UI != null)
        {
            if (UI.GetMoveBoomSpeed() != 0)
                MoveSpeed = UI.GetMoveBoomSpeed();
        }
        Hashtable ht = new Hashtable();
        ht.Add("position", new Vector3(MoveVector3.x + _MoveVector3.x, MoveVector3.y + _MoveVector3.y, MoveVector3.z));
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(this.gameObject, ht);
    }
    public void MirrorCreate()
    {
        if (GameObject.Find("UIController") != null)
            UI = GameObject.Find("UIController").GetComponent<LightButt>();
        ExTime = 0;
        NoMoveCheck = false;
        if (LAMP != null)
            TempLampVector = LAMP.transform.position;
        if (UI != null)
        {
            if (UI.GetMoveBoomSpeed() != 0)
                MoveSpeed = UI.GetMoveBoomSpeed();
        }
        Hashtable ht = new Hashtable();
        ht.Add("position", new Vector3(MoveVector3.x * 10, MoveVector3.y * 10, TempZ));
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(this.gameObject, ht);
    }
    public void Create()
    {
        if (GameObject.Find("UIController") != null)
            UI = GameObject.Find("UIController").GetComponent<LightButt>();
        ExTime = 0;
        NoMoveCheck = false;
        if (LAMP != null)
            TempLampVector = LAMP.transform.position;
        if (UI != null)
        {
            if (UI.GetMoveBoomSpeed() != 0)
                MoveSpeed = UI.GetMoveBoomSpeed();
        }
        Hashtable ht = new Hashtable();
        ht.Add("position", MoveVector3);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed", MoveSpeed);
        ht.Add("onupdatetarget", gameObject);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("onupdate", "NewLookAt");
        ht.Add("oncomplete", "EndMove");
        iTween.MoveTo(this.gameObject, ht);
    }
    void NewLook()
    {
        transform.LookAt(MoveVector3);
    }
    void Update()
    {
        ExTime += Time.deltaTime;
        if (ExTime > DeletTime)
        {
            ExTime = 0;
            EndMove();
        }
    }
    public void LightEndMove()
    {
        if (UseIng)
            return;
        if (NoMoveCheck)
            return;
        GameObject lightClone = (GameObject)Instantiate(touchLight, transform.localPosition, Quaternion.identity);
        if (BoomEff != null)
            TempGame = (GameObject)Instantiate(BoomEff, transform.localPosition, Quaternion.identity);
        Destroy(lightClone, 2f);
        DestroyObject(this.gameObject);
        UseIng = true;
    }
    void EndMove()
    {
        if (UseIng)
            return;
        if (NoMoveCheck)
            return;
        if (CamaerView == true)
        {
                GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag();
        }
        ExplosionLightArrow();


    }
    public void ExplosionLightArrow()
    {

        GameObject lightClone = (GameObject)Instantiate(touchLight, transform.localPosition, Quaternion.identity);
        lightClone.tag = "LightObject";
        if (BoomEff != null)
            TempGame = (GameObject)Instantiate(BoomEff, transform.localPosition, Quaternion.identity);
        Destroy(lightClone, 2f);
        Destroy(TempGame, 2.1f);
        DestroyObject(this.gameObject);
        UseIng = true;

        NewSoundMgr.instance.PlaySingle(lightArrowExplosionSound);// 2015-09-10 새로운 사운드 매니저 추가
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            return;
        if (CollCehck == false)
            return;
        if (other.tag == "TestLight" || other.tag == "NoColl" || other.tag == "BrightenObject" || other.tag == "Player" || other.tag == "brighten" || other.transform.tag == "LampMoveEff" || other.tag == "LightObject" || other.transform.tag == "light" || other.tag == "boss" || other.tag == "Blackhole" || other.tag == "Door" || other.tag == "Selectable")
        {
            if (other.tag == "Blackhole")
            {
                if (CamaerView == true)
                {
                    GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag();
                }
                return;
            }
            else if (other.tag == "Door")
            {
                if (other.GetComponent<DoorObject>().Type == BULLETTYPE.PLAYER)
                {
                    if (CamaerView == true)
                    {
                        GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag();
                    }
                }
                else
                {
                    if (BulletDoorCheck == false)
                    {
                        Vector3 newTemp;
                        StopMove();
                        if (LAMP.transform.position.x < this.transform.position.x)
                            newTemp.x = 7;
                        else
                            newTemp.x = -7;
                        newTemp.y = 0;
                        newTemp.z = 0;
                        this.transform.position = other.transform.GetComponent<DoorObject>().Door.GetComponent<DoorObject>().OnEff[0].transform.position;
                        BulletDoorCheck = true;
                        SetVector(this.transform.position);
                        BulletCreate(newTemp);
                    }
                }
            }
            return;
        }
        else if (other.tag == "Mirror")
        {
            if (ReColl == false)
                return;
   

            DirCheck((int)other.GetComponent<MirrorObject>().MIrrorDir);
            return;
            //iTween.Stop();
        }
        else
        {
            //iTween.Stop();
            EndMove();
        }
    }
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.layer == 8)
            return;
        if (CollCehck == false)
            return;
        if (other.transform.tag == "TestLight" || other.transform.tag == "NoColl" || other.transform.tag == "BrightenObject" || other.transform.tag == "Player" || other.transform.tag == "brighten" || other.transform.tag == "LampMoveEff" || other.transform.tag == "LightObject" || other.transform.tag == "light" || other.transform.tag == "boss" || other.transform.tag == "Blackhole" || other.transform.tag == "Door" || other.transform.tag == "Selectable")
        {
            if (other.transform.tag == "Blackhole")
            {
                if (CamaerView == true)
                {
                    GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag();
                }
            }
            else if ((other.transform.tag == "Door" || other.transform.GetComponent<DoorObject>().GetOn() == false))
            {
                if (other.transform.GetComponent<DoorObject>().Type == BULLETTYPE.PLAYER)
                {
                    if (CamaerView == true)
                    {
                        GameObject.Find("Main Camera").GetComponent<CameraController>().SettingTag();
                    }
                }
                else
                {
                    if (BulletDoorCheck == false)
                    {
                        Vector3 newTemp;
                        StopMove();
                        if (LAMP.transform.position.x < this.transform.position.x)
                            newTemp.x = 7;
                        else
                            newTemp.x = -7;
                        newTemp.y = 0;
                        newTemp.z = 0;
                        this.transform.position = other.transform.GetComponent<DoorObject>().Door.GetComponent<DoorObject>().OnEff[0].transform.position;
                        BulletDoorCheck = true;
                        SetVector(this.transform.position);
                        BulletCreate(newTemp);
                    }
                }
            }
            return;
        }
        else if (other.transform.tag == "Mirror")
        {
            if (ReColl == false)
                return;
     
            DirCheck((int)other.gameObject.GetComponent<MirrorObject>().MIrrorDir);
            return;
            //iTween.Stop();
        }
        else
        {
            //iTween.Stop();
            EndMove();
        }
    }
}
