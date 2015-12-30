using UnityEngine;
using System.Collections;

public enum CameraView { quaterView, centerView, bossView};
public enum CameraZoom { zoomIn, zoomOut, bossZoom, fullScreen };

[System.Serializable]
public class CameraWalk
{
    public Transform target;
    public CameraView view;
    public CameraZoom zoom;
    public float speed;
    public float waitTime;
}

public class CameraController : MonoBehaviour
{
    static bool isStickyCamera = false;

    public Transform target;
    public float addSpeed = 0.01f;
    public CameraView view = CameraView.quaterView;
    public CameraZoom zoom = CameraZoom.zoomIn;
    public float[] viewAngleList = { 25, 5, 10};
    public float[] viewOffsetList = { 1.5f, 0.3f, 1.5f};
    public float[] zoomSizeList = { -2, -3, -2.5f,-4.5f };

    CameraView lastView = CameraView.quaterView;
    float offsetSpeed = 0.0f;

    float baseZAxis = -3;
    float offsetY = 1.5f;
    float moveSpeed = 0;

    float viewAngle = 25;
    Quaternion viewRotation = Quaternion.identity;

    //카메라 워크 관련
    bool beginCameraWalk = false;
    CameraWalk[] cameraWalk;
    int nowCameraWalk;
    CameraWalk preInfo;
    bool reControlLamp = false;

    //카메라 진동
    static Transform camTransform;
    static float shake = 0f;   //진동 시간
    static float shakeAmount = 0.7f;   //진동 강도
    static Vector3 originalPos;
    static bool isShake = false;
    float decreaseFactor = 1.0f;    //진동 시간이 감소하는 배율
    float MoveY = 0;
    bool CameraViewChageCheck;
    bool MoveCheck;
    Vector3 tempTagerVectr;
     GameObject Tager;//처음 카메라이동할때 이동타겟
     GameObject MoveTager; //이동 타켓 카메라가 따라다닐 타겟
     Vector3 TempCamaer;
     Quaternion Temp;
     Transform TempParent;
       float MoveTime =1f;
     float RotaionSpeed =1f;
     float TempMoveTime = 0f;
     bool test = false;
     bool DoorMove = false;
     public bool GetMoveCheck()
     {
         return MoveCheck;
     }
     public void SettingTag(GameObject Tag)
     {
         if (CameraViewChageCheck == false)
             target = Tag.transform;
         else
         {
             this.transform.parent = Tag.transform;
             this.transform.localPosition = TempCamaer;
             this.transform.localRotation = Temp;

         }
     }
     public void SettingTag()
     {
         if (CameraViewChageCheck == false)
             target = GameObject.Find("Lamp").transform;
         else
         {
             this.transform.parent = TempParent;
             this.transform.localPosition = TempCamaer;
             this.transform.localRotation = Temp;

         }
     }
    public void SetView(bool _set)
    {
        CameraViewChageCheck = _set;
    }
    void Awake()
    {
        target = GameObject.Find("Lamp").transform;
        CameraViewChageCheck = false;
        camTransform = transform;
        //카메라 기본 회전각도 설정
        viewAngle = viewAngleList[(int)view];
        offsetY = viewOffsetList[(int)view];
        MoveCheck = false;
        //카메라 초기 위치 설정
        transform.position = new Vector3(target.position.x, target.position.y + offsetY, baseZAxis);

        //초기화
        preInfo = new CameraWalk();
        preInfo.target = target;
        preInfo.view = view;
        preInfo.zoom = zoom;
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
    public void SetMoveY(float _y)
    {
        MoveY = _y;
    }
   static  public void Endshake()
    {
        shake = 0;
        isShake = false;
    }
   void Update()
   {
       if (DoorMove == false)
       {
           if (CameraViewChageCheck == false)
           {
               //카메라 진동
               if (isShake)
               {
                   if (shake > 0)
                   {
                       originalPos = new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis);
                       camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                       shake -= Time.deltaTime * decreaseFactor;
                   }
                   else
                   {
                       shake = 0f;
                       camTransform.localPosition = originalPos;
                       isShake = false;
                   }
               }
               else
               {
                   //설정된 뷰에 맞춰 카메라 회전
                   if (viewAngle != viewAngleList[(int)view])
                   {
                       float angleDistance = viewAngleList[(int)view] - viewAngle;
                       if (Mathf.Abs(angleDistance) > 0.25f)
                       {
                           if (angleDistance >= 0)
                           {
                               viewAngle += 0.25f;
                               offsetY += 0.25f * offsetSpeed;
                           }
                           else
                           {
                               viewAngle -= 0.25f;
                               offsetY -= 0.25f * offsetSpeed;
                           }
                       }
                       else
                       {
                           viewAngle = viewAngleList[(int)view];
                           offsetY = viewOffsetList[(int)view];
                       }
                   }
                   else
                   {
                       lastView = view;
                   }
                   viewRotation.eulerAngles = new Vector3(viewAngle, 0, 0);
                   transform.rotation = viewRotation;

                   //설정된 타겟에게 카메라 이동 (줌 적용)
                   baseZAxis = zoomSizeList[(int)zoom];
                   if (isStickyCamera)
                   {
                       if (beginCameraWalk == false)
                           moveSpeed += addSpeed;
                       Vector3 targetPos = new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis);
                       Vector3 moveVec = (targetPos - transform.position).normalized * moveSpeed;

                       if ((targetPos - transform.position).sqrMagnitude > moveVec.sqrMagnitude)
                       {
                           transform.Translate(moveVec);
                       }
                       else
                       {
                           transform.position = new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis);
                           moveSpeed = 0;
                           isStickyCamera = false;
                           //카메라 워크 중일 경우 타겟에 도달하면 일정 시간 대기 후 타겟을 변경해준다.
                           if (beginCameraWalk)
                           {
                               Invoke("nextCameraWalk", cameraWalk[nowCameraWalk].waitTime);
                           }
                           else if (reControlLamp)
                           {
                               if (!preInfo.target.GetComponent<Rigidbody>().isKinematic)
                               {
                                   preInfo.target.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                                   preInfo.target.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                                   preInfo.target.GetComponent<LampInfo>().setControl(true);
                               }
                           }
                       }
                   }
                   else
                   {
                       transform.position = new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis);
                   }
               }
           }
           else
           {
               if (test == false)
               {
                   if (MoveCheck)
                   {
                       TempMoveTime += Time.deltaTime;
                       if (TempMoveTime >= MoveTime)
                       {
                           EndMove();
                           MoveCheck = false;
                           TempMoveTime = 0.0f;
                       }
                       MoveUpdateCamare();
                   }
               }
               else
               {
                   ReTrunCamare();
                   Invoke("EndRetrun", 1.7f);
               }
               if (isShake)
               {
                   if (shake > 0)
                   {
						originalPos = new Vector3(Tager.transform.position.x + (MoveTager.transform.position.x - tempTagerVectr.x),
						                                     Tager.transform.position.y + (MoveTager.transform.position.y - tempTagerVectr.y),
						                                     Tager.transform.position.z + (MoveTager.transform.position.z - tempTagerVectr.z));
                       camTransform.position = originalPos + Random.insideUnitSphere * shakeAmount;

                       shake -= Time.deltaTime * decreaseFactor;
                   }
                   else
                   {
                       shake = 0f;
						camTransform.position = originalPos;
                       isShake = false;
                   }
               }
               if (isStickyCamera == true)
                   isStickyCamera = false;

           }
       }
   }
    static public void SetStickyMode(bool set = true) { isStickyCamera = set; }
    static public bool isStickyMode() { return isStickyCamera; }
    static public void SetShake(float shakeTime, float strenth=0.1f)
    {
        shake = shakeTime;
        shakeAmount = strenth;
        isShake = true;
        originalPos = camTransform.position;
    }

    public void setCameraView(CameraView setView)
    {
        view = setView;
        //오프셋 차이 / 각도 차이
        float offset = Mathf.Abs(viewOffsetList[(int)lastView] - viewOffsetList[(int)view]);
        float angle = Mathf.Abs(viewAngleList[(int)lastView]-viewAngleList[(int)view]);
        if( angle != 0 ){
            offsetSpeed = offset / angle;
        }
        else
        {
            offsetSpeed = 0.06f;
        }
    }
    public void setCameraZoom(CameraZoom setZoom)
    {
        zoom = setZoom;
        SetStickyMode();
    }

    public void startCameraWalk(CameraWalk[] _cameraWalk){
        cameraWalk = _cameraWalk;
        beginCameraWalk = true;
        nowCameraWalk = 0;
        preInfo.target = target;
        preInfo.view = view;
        preInfo.zoom = zoom;
        //카메라 워크 시작
        target = cameraWalk[0].target;
        setCameraView(cameraWalk[0].view);
        setCameraZoom(cameraWalk[0].zoom);
        moveSpeed = cameraWalk[0].speed;
        GameObject.Find("GameManager").SendMessage("DisableLightGauge", SendMessageOptions.DontRequireReceiver);

        if (preInfo.target.CompareTag("Player"))
        {
            if (!preInfo.target.GetComponent<Rigidbody>().isKinematic)
            {
                preInfo.target.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                preInfo.target.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                preInfo.target.GetComponent<LampInfo>().setControl(false);
            }
        }
    }
    void nextCameraWalk()
    {
        nowCameraWalk += 1;
        if (nowCameraWalk < cameraWalk.Length)
        {
            target = cameraWalk[nowCameraWalk].target;
            setCameraView(cameraWalk[nowCameraWalk].view);
            setCameraZoom(cameraWalk[nowCameraWalk].zoom);
            moveSpeed = cameraWalk[nowCameraWalk].speed;
        }
        else
        {
            StopCameraWalk();
        }
    }
    public void StopCameraWalk()
    {
        //카메라 워크 종료 및 원래 타겟으로 되돌아가기
        beginCameraWalk = false;
        moveSpeed = 0;
   

        target = preInfo.target;
        setCameraView(preInfo.view);
        setCameraZoom(preInfo.zoom);
        if (preInfo.target.CompareTag("Player"))
        {
            reControlLamp = true;
        }
        
    }
    // 추가부분

  
    IEnumerator XRotion()
    {
        float x = Tager.transform.rotation.eulerAngles.x;
        if (x > 0)
        {
            for (float i = 0; i < x; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(i, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
        else
        {
            for (float i = x; i > 0; i -= RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(i, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
    }
    IEnumerator YRotion()
    {

        float y = Tager.transform.rotation.eulerAngles.y;
        if (y > 0)
        {
            for (float i = 0; i < y; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, i, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
        else
        {
            for (float i = y; i > 0; i -= RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, i, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
    }
    IEnumerator ZRotion()
    {
        float z = Tager.transform.rotation.eulerAngles.z;
        if (z > 0)
        {
            for (float i = 0; i < z; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, i);
                yield return 0;
            }
        }
        else
        {
            for (float i = z; i > 0; i -= RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, i);
                yield return 0;
            }
        }
    }
    public void DoorMoveC()
    {
        DoorMove = true;
        Invoke("StartC", 0.2f);
    }
    void StartC()
    {
        Hashtable ht = new Hashtable();
        if (CameraViewChageCheck == true)
        {
            ht.Add("position", new Vector3(Tager.transform.position.x + (MoveTager.transform.position.x - tempTagerVectr.x),
                   Tager.transform.position.y + (MoveTager.transform.position.y - tempTagerVectr.y),
                   Tager.transform.position.z + (MoveTager.transform.position.z - tempTagerVectr.z)));
        }
        else
        {
            ht.Add("position", new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis));
        }
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 0.5f);
        ht.Add("oncompletetarget", gameObject);
        ht.Add("oncomplete", "EndDoor");
        iTween.MoveTo(gameObject, ht);
    }
    void EndDoor()
    {
        DoorMove = false;
    }
    public void Setting(GameObject T, GameObject M, float _MoveTime, float _RotaionSpeed)
    {
        Tager = T; // 이동타겟
        MoveTager = M; // 무브타겟
        MoveTime = _MoveTime;
        RotaionSpeed = _RotaionSpeed;

    }
    public void StartCamaer()
    {

        MoveCheck = false;
        CameraViewChageCheck = true;
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, 0));
        StartCoroutine(XRotion());
        StartCoroutine(YRotion());
        StartCoroutine(ZRotion());
        this.transform.parent = MoveTager.transform;
        MoveCamare();
    }
    
    void MoveCamare()
    {
        tempTagerVectr = MoveTager.transform.position;
        MoveCheck = true;
        //Hashtable ht = new Hashtable();
        //ht.Add("position", new Vector3(Tager.transform.position.x + (MoveTager.transform.position.x - tempTagerVectr.x),
        //       Tager.transform.position.y + (MoveTager.transform.position.y - tempTagerVectr.y),
        //       Tager.transform.position.z + (MoveTager.transform.position.z - tempTagerVectr.z)));
        //ht.Add("easetype", iTween.EaseType.linear);
        //ht.Add("time", MoveTime);
        //ht.Add("oncompletetarget", gameObject);
        //ht.Add("oncomplete", "EndMove");
        //iTween.MoveTo(gameObject, ht);
    }
    void MoveUpdateCamare()
    {
        
        Hashtable ht = new Hashtable();
        ht.Add("position", new Vector3(Tager.transform.position.x + (MoveTager.transform.position.x - tempTagerVectr.x),
               Tager.transform.position.y + (MoveTager.transform.position.y - tempTagerVectr.y),
               Tager.transform.position.z + (MoveTager.transform.position.z - tempTagerVectr.z)));
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", MoveTime);
        iTween.MoveUpdate(this.gameObject, ht);
    }
    void EndMove()
    {
        TempParent = this.transform.parent;
        TempCamaer = this.transform.localPosition ;
        Temp = this.transform.localRotation;
    }
    public void ReSetCamaer()
    {
        if (CameraViewChageCheck == true)
        {
            CameraViewChageCheck = false;
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(20, 0, 0));
            this.transform.parent = null;
            test = false;
        }
    }
    public void ReSetSleepCamaer()
    {
        if (CameraViewChageCheck == true)
        {
            StartCoroutine(SleepYRotion());
            StartCoroutine(SleepZRotion());
            StartCoroutine(SleepXRotion());
            this.transform.parent = null;
            test = true;
        }
    }
    void ReTrunCamare()
    {
        Vector3 targetPos = new Vector3(target.position.x, (target.position.y + offsetY) + MoveY, target.position.z + baseZAxis);
        Hashtable ht = new Hashtable();
        ht.Add("position", targetPos);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 1.5f);
        iTween.MoveUpdate(this.gameObject, ht);
    }
    void EndRetrun()
    {
        CameraViewChageCheck = false;
        test = false;
    }
    IEnumerator SleepXRotion()
    {
        float x = Tager.transform.rotation.eulerAngles.x;
        if (x > 0)
        {
            for (float i = x; i > 20.0f; i -= RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(i, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
        else
        {
            for (float i = x; i < 20.0f; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(i, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
    }
    IEnumerator SleepYRotion()
    {

        float y = Tager.transform.rotation.eulerAngles.y;
        if (y > 0)
        {
            for (float i = y; i > 0; i -= RotaionSpeed)
            {

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, i, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
        else
        {
            for (float i = y; i < 0; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, i, transform.rotation.eulerAngles.z);
                yield return 0;
            }
        }
    }
    IEnumerator SleepZRotion()
    {
        float z = Tager.transform.rotation.eulerAngles.z;
        if (z > 0)
        {
            for (float i = z; i > 0; i -= RotaionSpeed)
            {

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, i);
                yield return 0;
            }
        }
        else
        {
            for (float i = z; i < 0; i += RotaionSpeed)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, i);
                yield return 0;
            }
        }
    }
}
