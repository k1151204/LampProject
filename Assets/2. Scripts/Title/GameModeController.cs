using UnityEngine;
using System.Collections;
using System.Text;

public class GameModeController : MonoBehaviour {

    public int initModeNum = 1;
    public int maxModeCnt = 2;
    public int MaxCharCount = 2;
    public int MaxMapCount = 29;
    public bool TestMode = true; // 트루면 다열림
    int nowModeNum = 1;
    bool Shop;
    public StageSelectController stageSelectController;
    public GameObject loadingPanel;
    public string[] gameModeDirectory;
    public GameObject setting;
    public GameObject _Loading;
    public GameObject stageLock;

    void Start()
    {
        Shop = false;
      
    }

    // 530픽셀씩 이동
    void Update()
    {
        if (_Loading.activeSelf)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Shop)
                return;
            if (transform.FindChild("ChooseStage").gameObject.GetComponent<StageSelectMgr>().Gettest() == false)
                return;
            BackButton();
        }
    }

    public void BackButton()
    {
        if (_Loading.activeSelf)
            return;
        GameObject.Find("TitleManager").GetComponent<TitleManager>().nowState = TitleState.main;
        //메인 타이틀 활성화
        // transform.parent.parent.parent.FindChild("Main").gameObject.SetActive(true);
        //SelectStage 비활성화
        for (int i = 0; i < GameObject.Find("3D_UI").transform.childCount; i++)
        {
            if (GameObject.Find("3D_UI").transform.GetChild(i).name != "ShopUI")
                GameObject.Find("3D_UI").transform.GetChild(i).gameObject.SetActive(true);
        }
        transform.FindChild("ChooseStage").gameObject.GetComponent<StageSelectMgr>().Stop();
        transform.FindChild("ChooseStage").gameObject.SetActive(false);
        gameObject.SetActive(false);
        transform.parent.FindChild("Main").gameObject.SetActive(true);
        GameObject.Find("Canvas(StageSelect)").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Canvas(StageSelect)").transform.GetChild(1).gameObject.SetActive(false);
        setting.SetActive(true);
        stageLock.SetActive(false);

    }
    public bool GetShop()
    {
        return Shop;
    }
    public void SetShop(bool _set)
    {
        Shop = _set;
    }

    public void Loading()
    {
        int ChCount = transform.FindChild("ChooseStage").gameObject.GetComponent<StageSelectMgr>().chapterButton.Length;
        //로딩 화면 활성화
        //loadingPanel.SetActive(true);
        if (LightDataManager.GetCheck() == true)
        {
            LightDataManager.Setting(MaxMapCount, ChCount, nowModeNum, TestMode);
          
            if (LightDataManager.GetCreate() == true)
            {
                if (TestMode == false)
                {
                    for (int i = 1; i < MaxMapCount-7; i++)
                        LightDataManager.SetClear(i, 0);
                    for (int i = MaxMapCount - 7; i < MaxMapCount;i++ )
                        LightDataManager.SetClear(i, 1);

                        for (int I = 1; I < ChCount; I++)
                        {
                            if (I == ChCount - 1)
                                LightDataManager.SetChListOk(I, 1);
                            else
                                LightDataManager.SetChListOk(I, 0);
                        }

                    LightDataManager.SavaData();
                }
            }
        }
        LampCharData.Setting(MaxCharCount,TestMode);
      
        LoadStage();
    }

    void LoadStage()
    {
        //선택한 게임모드 변수를 업데이트
        SettingValue.GetData.gameMode = nowModeNum;
        SettingValue.GetData.Setting(MaxMapCount);
        //if (settingValue.isLoaedStage() == false)
        //{
        //    //모든 게임모드의 디렉토리에 접근해서 프리팹 로드
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < gameModeDirectory.Length; ++i)
        //    {
        //        sb.AppendFormat("MainGame/Prefab/Stage/{0}", gameModeDirectory[i]);
        //        Object[] tempStageList = Resources.LoadAll(sb.ToString());
        //        sb.Remove(0, sb.Length);

        //        //게임 오브젝트형으로 형 변환
        //        int maxStageCnt = tempStageList.Length;
        //        GameObject[] stageList = new GameObject[maxStageCnt];
        //        for (int j = 0; j < maxStageCnt; ++j)
        //        {
        //            stageList[j] = tempStageList[j] as GameObject;
        //        }

        //        //셋팅값에 로드한 스테이지 프리팹을 설정
        //        settingValue.setStageList(stageList);
        //    }
        //}
 
        ////셋팅값의 스테이지 개수를 현재 게임모드의 스테이지 개수로 갱신
        //settingValue.setStageMaxCnt(nowModeNum);

        //화면 전환 및 스테이지 버튼 활성화
        stageSelectController.gameObject.transform.parent.gameObject.SetActive(true);
        stageSelectController.ActiveStageButton();
        //transform.parent.gameObject.SetActive(false);
        //로딩 화면 종료
        //loadingPanel.SetActive(false);
    }

}
