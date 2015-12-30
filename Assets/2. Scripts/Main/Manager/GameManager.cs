using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int PlayerLife = 3; // 플레이어 목숨 카운트 3
    int TempPlayerLife;
    public int[] LastChMapStageNumber;
    public enum GameMode { Classic = 1, Master = 2, Challenge = 3 };
    public int MaxMapCount = 29;
    public int startStage = 1;      //시작 스테이지 넘버
    public GameMode gameMode = GameMode.Classic;
    public string[] gameModeDirectory;
    int stageCnt;        //스테이지 총 개수
    LampInfo lampInfo;          //램프 정보
    StageInfo stageInfo;
    int nowStageNum;            //현재 스테이지 넘버
    GameObject startLamp;       //스테이지 시작점 (시작 램프)

    public ArrayList saveStoneList;     //현재 스테이지의 세이브 스톤 리스트
    Vector3 lastSavePos;       //마지막으로 활성화된 세이브 스톤 

    UIController uiController;
    LightButt LightButt;
    CameraController cameraController;
    GameObject lamp;
    Light lampPointLight;
    GameObject stage;

    GameObject lampDieEffect;
    bool isCreateLampDieEffect;
    GameObject LightBombEffect;

    //브라이튼 스킬
    BrightenScript brighten;
    GameObject brightenEffect;

    //현재 재생 중인 BGM 저장
    //BgmIndex nowPlayingBgm = BgmIndex.None;
    bool isFirstBgm = true;
    int activeSaveStoneCnt;
    public GameObject testStage = null;
    GameObject ClearWindown;
    GameObject Star1;
    GameObject Star2;
    GameObject Star3;

 
    bool NextStageCheck = true;

    SettingValue settingValue;

    /*2015-09-10 새로운 사운드 매니저 추가*/
    public AudioClip stageClearSound;
    public AudioClip playerDie1;
    public AudioClip playerDie2;

    public GameObject RetryPopUp;
    public GameObject DiePopUp;

    bool ClearNextCheck = false;

    bool IsDieCheck = false;

    public UILabel playerPlayLife;
    void Awake()
    {
        LightDataManager.Setting(MaxMapCount, LastChMapStageNumber.Length, 1, true);
        LampLifeManager.Setting(false);
        GetComponent<LampChageAddCom>().SettingLamp();
        CreateLamp();
        //환경설정 받아오기
        GameObject settingValueObject = GameObject.FindGameObjectWithTag("setting") as GameObject;
        if (settingValueObject == null)
        {
            GameObject tempResource = Resources.Load("MainGame/Prefab/GlobalObject/SettingValue") as GameObject;
            GameObject temp = Instantiate(tempResource, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            settingValue = temp.GetComponent<SettingValue>();
        }
        else
        {
            settingValue = settingValueObject.GetComponent<SettingValue>();
        }
        //사운드 매니저 초기화
        //SoundManager.Init();
    }
 
    void CreateLamp()
    {
        if (GameObject.Find("Lamp") == null)
        {
            //DestroyObject(GameObject.Find("Lamp").gameObject);
            GameObject Temp;
            Temp = Instantiate(LampCharChage.GetData.GetEff((int)LampChar.LAMP));
            string[] Name = Temp.name.Split('(');
            Temp.name = Name[0];
            Temp = Instantiate(LampCharChage.GetData.GetEff((int)LampChar.BRIGHTEM));
            Name = Temp.name.Split('(');
            Temp.name = Name[0];
        }

    }
    void Start()
    {
        ClearWindown = GameObject.Find("RightTopAnchor").transform.FindChild("Clear").gameObject;
        Star1 = ClearWindown.transform.FindChild("Clear").FindChild("Star1").gameObject;
        Star2 = ClearWindown.transform.FindChild("Clear").FindChild("Star2").gameObject;
        Star3 = ClearWindown.transform.FindChild("Clear").FindChild("Star3").gameObject;
      
   
        stage = new GameObject();
        //  램프 관련 초기화
        lamp = GameObject.Find("Lamp");
        lampPointLight = lamp.transform.FindChild("light").GetComponent<Light>();
        lampInfo = lamp.GetComponent<LampInfo>();
        lampDieEffect = Resources.Load("MainGame/Prefab/Effect/DestroyLamp") as GameObject;
        LightBombEffect = Resources.Load("MainGame/Prefab/Effect/LightBomb") as GameObject;
        isCreateLampDieEffect = false;
        //  UI컨트롤러 초기화
        uiController = GameObject.Find("UIController").GetComponent<UIController>();
        LightButt = GameObject.Find("UIController").GetComponent<LightButt>();
        //  카메라 컨트롤러 초기화
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        //  세이브 스톤 리스트 초기화
        saveStoneList = new ArrayList();
        //  브라이튼
        brighten = GameObject.Find("Brighten").GetComponent<BrightenScript>();
        brightenEffect = Resources.Load("MainGame/Prefab/Effect/ActiveBrighten") as GameObject;
        //스테이지 로드
        if (SettingValue.GetData.isLoaedStage() == false)
        {
            nowStageNum = startStage;
            SettingValue.GetData.Setting(MaxMapCount);
        }
        else
        {
            nowStageNum = SettingValue.GetData.startStage;
            gameMode = (GameMode)SettingValue.GetData.gameMode;
        }
      
        StageInit(false);

        playerPlayLife.text = "x " + PlayerLife;
        TempPlayerLife = PlayerLife;
    }

    void Update()
    {
        //라이트 게이지 업데이트
        //현재 라이트 게이지에 따라 빛이 조정됨.

        lampPointLight.intensity = lampInfo.getLight() * 0.05f;
    }
    public int GetSaveCnt()
    {
        return activeSaveStoneCnt;
    }
    //현재 스테이지 초기화
    void StageInit(bool isRestart = true)
    {
        bool[] isActiveSaveStone = new bool[saveStoneList.Count];
        activeSaveStoneCnt = 0;
        CameraController.Endshake();
        LoadStagePrefab(nowStageNum);
        //스테이지 재시작일 경우 이전 스테이지에서 활성화했던 세이브스톤의 인덱스를 저장한다.
        if (isRestart)
        {
            for (int i = 0; i < isActiveSaveStone.Length; ++i)
            {
                if (((GameObject)saveStoneList[i]).GetComponent<SaveStoneScript>().isActivated())
                {
                    isActiveSaveStone[i] = true;
                    activeSaveStoneCnt += 1;
                }
                else
                {
                    isActiveSaveStone[i] = false;
                }
            }
        }
        lamp.transform.parent = null;        //어태치 버그 수정
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("Stage{0}", nowStageNum);
        if (testStage == null)
        {
            //스테이지 삭제
            Destroy(stage);
            System.GC.Collect();
            stage = (GameObject)Instantiate(SettingValue.GetData.GetStage(nowStageNum), new Vector3(0, 0, 0), Quaternion.identity);
            //가비지 컬렉터 작동
            //System.GC.Collect();
            ////stage 게임오브젝트를 현재 스테이지로 변경한다.
            //for (int i = 0; i < settingValue.GetStageCount(); ++i)
            //{
            //    if (stageList[i])
            //    {
            //        if (stageList[i].name == sb.ToString())
            //        {
            //            stage = (GameObject)Instantiate(stageList[i], new Vector3(0, 0, 0), Quaternion.identity);
            //            break;
            //        }
            //    }
            //}
        }
        else
        {
            stage = testStage;
        }
        //새로운 세이브스톤으로 갱신
        InitSaveStone();
        //활성화된 세이브스톤이 없을 경우 스타트램프에서 재시작
      
        if (activeSaveStoneCnt <= 0)
        {

            LightDataManager.ReSettingLightCount();
            stageInfo = stage.transform.FindChild("StageInfo").GetComponent<StageInfo>();
            if (stageInfo.StartLamp == StageInfo.SpawnObjType.Lamp)
            {
                startLamp = stage.transform.FindChild("StartLamp").gameObject;
                //해당 스테이지의 전등을 킨다.
                Transform lampBulb = startLamp.transform.FindChild("LampBulb");
                lampBulb.FindChild("LampLight").gameObject.SetActive(true);
                lampBulb.FindChild("Glass").gameObject.SetActive(true);
                startLamp.transform.FindChild("Halogen01").gameObject.SetActive(true);
                Invoke("TurnOffStartLamp", 1.0f);
            }
            else if (stageInfo.StartLamp == StageInfo.SpawnObjType.RuneStone)
            {
                startLamp = stage.transform.FindChild("StartStone").gameObject;
            }

            //리스폰 위치로 램프 이동.
            Transform lampLightPos = startLamp.transform.FindChild("LampRespawnPos");
            lamp.transform.position = lampLightPos.position;
            cameraController.ReSetCamaer();
            if (nowStageNum == 1)
            {
                stage.transform.FindChild("StartDirection").gameObject.GetComponent<Stage1StartAction>().StartLampEvent();
                lamp.GetComponent<CreateBoneLamp>().Delete();
                lamp.GetComponent<CreateBoneLamp>().DeleteB();
            
            }
        }
        //세이브 스톤에서 재시작
        else
        {

            LightDataManager.ReSettingLightCount(LightDataManager.GetNowLightCount()); // 세이브스톤 이후 꺼 불끄기
            //저장했던 세이브스톤 활성화 상태에 맞추어 활성화한다.
            for (int i = 0; i < saveStoneList.Count; ++i)
            {
                if (isActiveSaveStone[i])
                {
                    ((GameObject)saveStoneList[i]).GetComponent<SaveStoneScript>().ActivateSaveStone();
                }
            }
            //램프를 세이브 위치로 이동
            lamp.transform.position = lastSavePos;
        }
        Invoke("FallLamp", 1.0f);
        NextStageCheck = true;
        //스테이지 시작 준비
        if (GameObject.Find("RightTo").transform.GetChild(0).gameObject.activeSelf == false)
            GameObject.Find("RightTo").transform.GetChild(0).gameObject.SetActive(true);
        if(GameObject.Find("StoryTellerPanel") != null)
        GameObject.Find("StoryTellerPanel").SetActive(false);
        lampInfo.restoreLight();
        lampInfo.setControl(false);
        lamp.GetComponent<Renderer>().enabled = true;
        lamp.transform.FindChild("Effect").gameObject.SetActive(true);
        lamp.GetComponent<Rigidbody>().isKinematic = false;
        lamp.GetComponent<Rigidbody>().useGravity = false;
        lamp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        lamp.GetComponent<Transform>().localScale = new Vector3(0.25F, 0.25F, 0.25F);
        lamp.GetComponent<Transform>().rotation = Quaternion.identity;
        Physics.gravity = new Vector3(0, -9.81f, 0);
        if (lamp.GetComponent<LampHpCreate>() != null)
            lamp.GetComponent<LampHpCreate>().DeleteObject();
        if (lamp.GetComponent<LampHpCreate>() != null)
            lamp.GetComponent<LampHpCreate>().CreateObject();

        uiController.PadeOutScreen();

        //스테이지 BGM이 변경됬을 경우 새로운 BGM을 재생한다.
        //if (stageInfo.StageBgm != nowPlayingBgm)
        //{
        //    if (isFirstBgm)
        //    {
        //        SoundManager.manager.playBgm(stageInfo.StageBgm, false);
        //        isFirstBgm = false;
        //    }
        //    else
        //    {
        //        SoundManager.manager.playBgm(stageInfo.StageBgm);
        //    }
        //}
        //nowPlayingBgm = stageInfo.StageBgm;

        //스테이지 이름 설정 및 디스플레이
        uiController.SetStageNameLabel(sb.ToString());
        uiController.DisplayStageName();
        //카메라 구도 기본 값 복원
            cameraController.setCameraView(CameraView.quaterView);
            cameraController.setCameraZoom(CameraZoom.bossZoom); // 2015-05-01 기본 줌을 ZoomIn -> bossZoom 으로 바꿈
            CameraController.SetStickyMode(false);
        uiController.disableMenu();
        uiController.disableClearWindow();
        isCreateLampDieEffect = false;
        uiController.ReSet();
      
    }

    //다음 스테이지로 이동
    void ClearNextStage()
    {
        if (NextStageCheck == false)
            return;
        StartCoroutine(ClearWindownOff(ClearWindown.transform.FindChild("Clear").GetComponent<UISprite>()));
        Invoke("TempNextStage", 1f);
        NextStageCheck = false;
    }
    void TempNextStage()
    {
        LightDataManager.SetGoToStageSelete(true);
        Application.LoadLevel("Title");
        CameraController.Endshake();
    }
    //램프를 떨어뜨린다. (스테이지 시작 시 사용)
    void FallLamp()
    {

        if (CameraController.isStickyMode() == false)
        {
            lamp.GetComponent<Rigidbody>().useGravity = true;
            lampInfo.setControl(true);
        }
        else
        {
            Invoke("FallLamp", 0.5f);
        }
    }
    void TurnOffStartLamp()
    {
        //스타트 램프 전등을 끈다.
        Transform lampBulb = startLamp.transform.FindChild("LampBulb");
        lampBulb.FindChild("LampLight").gameObject.SetActive(false);
        lampBulb.FindChild("Glass").gameObject.SetActive(false);
        startLamp.transform.FindChild("Halogen01").gameObject.SetActive(false);
    }

    void lampDie()
    {
        if (lampInfo.GetDieCheck())
            return;
        lampInfo.setControl(false);         //조작 불가
        lampInfo.addLight(-100);            //라이트 모두 소모    
        //램프 폭발 이펙트 생성 및 폭발 시뮬레이션
        if (isCreateLampDieEffect == false)
        {
            lamp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GameObject effect = Instantiate(lampDieEffect, lamp.transform.position, Quaternion.identity) as GameObject;
            effect.transform.parent = lamp.transform;
            Vector3 explosionOffset = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
            lamp.GetComponent<Rigidbody>().AddExplosionForce(2.5f, lamp.transform.position + explosionOffset, 5.5f, 1.5f, ForceMode.Impulse);
            isCreateLampDieEffect = true;
            NewSoundMgr.instance.PlaySingle(playerDie1);   // 2015-09-10 새로운 사운드 매니저 추가
            //1초 뒤 폭발
            Invoke("CreateLightBomb", 1.0f);

            if (PlayerLife == 1) // Player 목숨 카운트 0
            {
                Invoke("DiePopUpOn", 2.5f); //2.5초 후 스테이지 재시작
                IsDieCheck = true;
            }
            else if (PlayerLife >= 2)
            {             
                Invoke("TempStageInit", 2.5f);
                PlayerLife--;
            }

            if (lamp.GetComponent<LampHpCreate>() != null)
                lamp.GetComponent<LampHpCreate>().DeleteObject();
        }
    }

    void CreateLightBomb()
    {
        GameObject effect = Instantiate(LightBombEffect, lamp.transform.position, Quaternion.identity) as GameObject;
        effect.transform.parent = lamp.transform;
        lamp.GetComponent<Rigidbody>().isKinematic = true;
        lamp.GetComponent<Renderer>().enabled = false;
        lamp.transform.FindChild("Effect").gameObject.SetActive(false);
        NewSoundMgr.instance.PlaySingle(playerDie2);// 2015-09-10 새로운 사운드 매니저 추가
    }

    void DiePopUpOn()
    {
        DiePopUpUI();
    }

    public void DiePopUpUI(int Type = 0)
    {
        if (Type == 0)
        {
            DiePopUp.SetActive(true);
        }

        if (Type == 1) // Restart 버튼
        {          
            //DieReStage();
        }
        else if(Type == 2) // Home 버튼
        {
            TempNextStage(); // 스테이지 선택창으로 나가기
        }
    }

    void DieReStage()
    {
        //GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(true);
        //StageInit(false);
        //CameraController.SetStickyMode();
        //DiePopUp.SetActive(false);
        TempStageInit();
    }
    void ClearRestartStage()
    {
        if (ClearNextCheck == false)
            return;
        StartCoroutine(ClearWindownOff(ClearWindown.transform.FindChild("Clear").GetComponent<UISprite>()));
        if (GameObject.Find("JoyStickTutorial") != null)
        {
            if (GameObject.Find("JoyStickTutorial").transform.GetChild(0).gameObject == true)
            {
                GameObject.Find("JoyStickTutorial").transform.GetChild(0).gameObject.SetActive(false);
            }
            if (GameObject.Find("JoyStickTutorial").transform.GetChild(1).gameObject == true)
            {
                GameObject.Find("JoyStickTutorial").transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        Invoke("TempStageInit", 1f);
    }
    public void TempStageInit()
    {
        playerPlayLife.text = "x " + (PlayerLife);
        GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(true);
        StageInit();
        CameraController.SetStickyMode();
        GetComponent<BrightenMgr>().ShowText();
    }
    public void TempStageInit(bool test)
    {
        playerPlayLife.text = "x " + (PlayerLife);
        GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(true);
        StageInit(test);
        CameraController.SetStickyMode();
        GetComponent<BrightenMgr>().ShowText();
    }
    public void RetryPopUpOnOff(int i)
    {
        PlayerLife = TempPlayerLife;
        if(i == 0) // RetryPopUp창 켜기
        {
            RetryPopUp.SetActive(true);
        }
        else if (i == 1) // RetryPopUp창 끄기
        {
            RetryPopUp.SetActive(false);
        }
        else if (i == 2) // RetryPopUp창에서 Ok버튼
        {
            if (!IsDieCheck)
            {
                TempStageInit(false);
            }
            else if(IsDieCheck)
            {
                TempStageInit(true);
                IsDieCheck = false;
            }
            RetryPopUp.SetActive(false);
        }
    }
    //클리어 처리
    void stageClear()
    {
        if (isCreateLampDieEffect == false)
        {
            lamp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            lamp.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            lamp.GetComponent<Rigidbody>().useGravity = false;
            lampInfo.setControl(false);
            ClearWindown.SetActive(true);
            StartCoroutine(ClearWindowon(ClearWindown.transform.FindChild("Clear").GetComponent<UISprite>()));
            GameObject.Find("SetSum").transform.GetChild(0).GetComponent<UILabel>().text = LightDataManager.GetSumLight().ToString();
           
            if (LampLifeManager.GetNowLieft() < 5)
            {
                Debug.Log("eQWEQW");
                LampLifeManager.SetNowCount(LampLifeManager.GetNowLieft() + 1);
                LampLifeManager.SaveData();
            }
            NewSoundMgr.instance.PlaySingle(stageClearSound);// 2015-09-10 새로운 사운드 매니저 추가
        }
    }
    void SetDataWindow() // 수정함
    {
        ClearNextCheck = false;
        int LightCount = (GameObject.Find("StageInfo").GetComponent<LightCheck>().GetLightCount());

        int StarCount = 0;
        int[] LightMid = new int[3];
 
        LightMid[0] = 1;
        if (GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount % 2 == 0)
            LightMid[1] = GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount / 2;
        else
            LightMid[1] = (GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount / 2) + 1;

        LightMid[2] = GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount;
        // 여기다가 점수 계산
        // 
        // 보스면

   
            if (LightCount >= LightMid[0])
            {
                StarCount++;
            }

            if (LightCount >= LightMid[1])
            {
                StarCount++;
            }
            if (LightCount == LightMid[2])
            {
                StarCount++;
            }
            if (LightDataManager.GetStar(nowStageNum - 1) < StarCount)
            {
                int a = (StarCount - LightDataManager.GetStar(nowStageNum - 1)) * stageInfo.SumLightCount;
                GameObject.Find("GetSum").transform.GetChild(0).GetComponent<UILabel>().text = "+" + a.ToString();
     

            }
            StartCoroutine(StartLightCount(StarCount, Star1.GetComponent<UISprite>(), Star2.GetComponent<UISprite>(), Star3.GetComponent<UISprite>(), GameObject.Find("GetLight").transform.GetChild(0).GetComponent<UILabel>()
                , GameObject.Find("LampHelp").transform.FindChild("LampCount").GetComponent<UILabel>()));
      
        
    }
    IEnumerator StartLightCount(int StarCount,UISprite sprite, UISprite sprite1, UISprite sprite2,UILabel test,UILabel _downtest)
    {
        int Count = (GameObject.Find("StageInfo").GetComponent<LightCheck>().GetLightCount());
        int MaxCount = (GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount);
        if (GameObject.Find("StageInfo").GetComponent<StageInfo>().LightCount == 0)
        {
            StarCount = 3;
            test.text = "0 / 0";
        }
        else
        {
            test.text = "0 / " + MaxCount.ToString(); 
            for (int i = 0; i < Count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                _downtest.text = ((MaxCount - Count) + (i+1)).ToString() + " / " + MaxCount.ToString();
                test.text = (i + 1).ToString() + " / " + MaxCount.ToString();
            }
        }
        ClearNextCheck = true;

        if (StarCount == 1)
            StartCoroutine(StarSetting(Star1.GetComponent<UISprite>(), null, null));
        else if (StarCount == 2)
            StartCoroutine(StarSetting(Star1.GetComponent<UISprite>(), Star2.GetComponent<UISprite>(), null));
        else if (StarCount == 3)
            StartCoroutine(StarSetting(Star1.GetComponent<UISprite>(), Star2.GetComponent<UISprite>(), Star3.GetComponent<UISprite>()));

    }
    IEnumerator StartSumLight(UILabel test ,UILabel Down,int temp)
    {
        int t = LightDataManager.GetSumLight();
        for (int i = 1; i <= temp; i += 1)
        {
            test.text = (t + i).ToString();
            Down.text = "+" + (temp - i).ToString();
            yield return 0;
        }
        ClearNextCheck = true;
    }
    IEnumerator StarSetting(UISprite sprite, UISprite sprite1, UISprite sprite2)
    {
        int Star = 1;
        yield return new WaitForSeconds(0.4f);
        for (float i = 0; i <= 1f; i += 0.02f)
        {
            sprite.color = new Vector4(1, 1, 1, i);
            yield return 0;
        }
        yield return new WaitForSeconds(0.4f);
        if (sprite1 != null)
        {
            Star++;

            for (float i = 0; i <= 1f; i += 0.02f)
            {
                sprite1.color = new Vector4(1, 1, 1, i);
                yield return 0;
            }
            yield return new WaitForSeconds(0.4f);
            Star++;

            if (sprite2 != null)
            {
                for (float i = 0; i <= 1f; i += 0.02f)
                {
                    sprite2.color = new Vector4(1, 1, 1, i);
                    yield return 0;
                }
            }
        }
        yield return new WaitForSeconds(0.4f);

        Debug.Log(LightDataManager.GetSumLight());
        bool Tempcheck = false;
        for (int i = 0; i < LastChMapStageNumber.Length; i++)
        {
            if (nowStageNum == LastChMapStageNumber[i])
            {
                Tempcheck = true;
                break;
            }
        }

        //
        if (Tempcheck == false)
            LightDataManager.SetClear(nowStageNum, 1);
        if (LightDataManager.GetStar(nowStageNum - 1) < Star)
        {
            int a = (Star - LightDataManager.GetStar(nowStageNum - 1)) * stageInfo.SumLightCount;
            GameObject.Find("GetSum").transform.GetChild(0).GetComponent<UILabel>().text = "+" + a.ToString();
            StartCoroutine(StartSumLight(GameObject.Find("SetSum").transform.GetChild(0).GetComponent<UILabel>(), GameObject.Find("GetSum").transform.GetChild(0).GetComponent<UILabel>(), a));
            LightDataManager.SetStar(nowStageNum - 1, Star);
            LightDataManager.SetSumLight(a);

        }
        LightDataManager.SavaData();
        LightDataManager.ReSettingLightCount();
        ClearNextCheck = true;
    }
    IEnumerator ClearNameOn(UISprite sprite)
    {
        for (float i = 0.5f; i <= 1f; i += 0.01f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(ClearNameOff(sprite));

    }
    IEnumerator ClearNameOff(UISprite sprite)
    {
        for (float i = 1f; i >= 0.5f; i -= 0.01f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(ClearNameOn(sprite));
    }
    IEnumerator ClearWindowon(UISprite sprite)
    {
        for (float i = 0; i <= 0.9F; i += 0.02f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
 
        SetDataWindow();
    }
    IEnumerator ClearWindownOff(UISprite sprite)
    {

        for (float i = 0.9F; i >= 0; i -= 0.02f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
        Star1.GetComponent<UISprite>().color = new Vector4(1f, 1f, 1f, 0f);
        Star2.GetComponent<UISprite>().color = new Vector4(1f, 1f, 1f, 0f);
        Star3.GetComponent<UISprite>().color = new Vector4(1f, 1f, 1f, 0f);

        ClearWindown.SetActive(false);
    }
    void CleargotoTitle()
    {
        if (ClearNextCheck == false)
            return;
        LightDataManager.SetGoToStageSelete(false);
        StartCoroutine(ClearWindownOff(ClearWindown.transform.FindChild("Clear").GetComponent<UISprite>()));
        Invoke("tempgoto", 1f);
    }
    public void tempgoto()
    {
        LightDataManager.SetGoToStageSelete(true);
        Application.LoadLevel("Title");
        CameraController.Endshake();
        Time.timeScale = 1.0f;
    }
    //현재 마우스 위치에 터치라이트를 생성한다.

    //옵션 설정
    public bool isEnableSE()
    {
        return SettingValue.GetData.isEnableSE;
    }
    public bool isEnableBGM()
    {
        return SettingValue.GetData.isEnableBGM;
    }

    void LoadStagePrefab(int _stage)
    {
        //선택한 게임모드 변수를 업데이트
        SettingValue.GetData.gameMode = (int)gameMode;
        if (SettingValue.GetData.GetStage(_stage) == null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MainGame/Prefab/Stage/Classic/{0}", "Stage" + _stage);
            SettingValue.GetData.SetStage((GameObject)Resources.Load(sb.ToString()), _stage);
        }
        //모든 게임모드의 디렉토리에 접근해서 프리팹 로드
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < gameModeDirectory.Length; ++i)
        //{
        //    sb.AppendFormat("MainGame/Prefab/Stage/{0}", gameModeDirectory[i]);
        //    Object[] tempStageList = Resources.LoadAll(sb.ToString());
        //    sb.Remove(0, sb.Length);

        //    //게임 오브젝트형으로 형 변환
        //    int maxStageCnt = tempStageList.Length;
        //    GameObject[] stages = new GameObject[maxStageCnt];
        //    for (int j = 0; j < maxStageCnt; ++j)
        //    {
        //        stages[j] = tempStageList[j] as GameObject;
        //    }
        //    //셋팅값에 로드된 스테이지 프리팹을 설정
        //    //settingValue.setStageList(stages);
        //}

        //셋팅값의 스테이지 개수를 현재 게임모드의 스테이지 개수로 갱신
        //settingValue.setStageMaxCnt((int)gameMode);
    }

    void UpdateSaveStone(Transform saveStone)
    {
        GetComponent<BrightenMgr>().ShowText();
        lastSavePos = saveStone.position;
        Debug.Log(lastSavePos);
    }
    void InitSaveStone()
    {
        saveStoneList.Clear();
        Transform childObj;
        for (int i = 0; i < stage.transform.childCount; ++i)
        {
            childObj = stage.transform.GetChild(i);
            if (childObj.gameObject.CompareTag("saveStone"))
            {
                saveStoneList.Add(childObj.gameObject);
            }
        }
    }
    void UseTouchLight()
    {
        if (lampInfo.isPosibleControl())
        {
            if (lampInfo.getLight() >= stageInfo.LightCost)
            {
                uiController.LightHpDown(stageInfo.LightCost);
                uiController.TouchUIEff();
                LightButt.CreateLight(stageInfo.LightCost);
                lampInfo.addLight(-stageInfo.LightCost);
                GetComponent<BrightenMgr>().ShowText();
                
            }
        }
    }
}


