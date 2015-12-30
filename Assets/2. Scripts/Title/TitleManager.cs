using UnityEngine;

using System.Collections;
using System.IO;
using System;
using System.Threading;
using UnityEngine.UI;
public enum TitleState { main, play, option };

public class TitleManager : MonoBehaviour {
    public TitleState nowState = TitleState.main;
    public UIPanel mainPanel;
    public UIPanel selectStagePanel;

    public GameObject[] title_3dUI; // 타이틀 화면 램프(2015-06-16) 
    public GameObject playButton;

    public AudioClip titleBgm;// 2015-09-10 새로운 사운드 매니저 추가
    public AudioClip clickSound;// 2015-09-10 새로운 사운드 매니저 추가

    public GameObject settingObject;
    bool IsOption = true;
    bool IsOptionOnOff = true;
    public Animator option_Animation;
    bool IsSoundOnOff = true;
    public GameObject soundButton;
    public Sprite soundOff;
    public Sprite soundOn;

    public GameObject LoadingObject;
    public GameObject RandomFunction;

    int IsPlayButton = 0;
    float NextTime;
    public void FileLampLifetDAta()
    {
        StreamReader w = new StreamReader(Application.persistentDataPath + "/LampLife.txt");
        string a = w.ReadLine();
        LampLifeManager.SetNowCount(Int32.Parse(a));
        a = w.ReadLine();
        string[] a1 = a.Split(' ');
        LampLifeManager.SetNowMin(Int32.Parse(a1[0]));
        LampLifeManager.SetNowSec(Int32.Parse(a1[1]));
        a = w.ReadLine();
        a1 = a.Split(' ');
        LampLifeManager.SumTime(Int32.Parse(a1[0]), Int32.Parse(a1[1]), Int32.Parse(a1[2]), Int32.Parse(a1[3]), Int32.Parse(a1[4]), Int32.Parse(a1[5]));
        w.Close();
    }
    void Start()
    {
       
        LampLifeManager.Setting(selectStagePanel.gameObject.GetComponent<GameModeController>().TestMode);
        FileLampLifetDAta();
        LampLifeManager.SaveData();

        NextTime = 0.7f;
        NewSoundMgr.instance.PlayBgm(titleBgm);
        if (LightDataManager.GetGoToStageSelete())
        {
            NextTime = 0f;
            SetPlayState();
            settingObject.SetActive(false);
            LightDataManager.SetGoToStageSelete(false);
        }
    }

    void Update()
    {
        if (settingObject.activeSelf == true)
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "PlayButton")
                {
                    playButton.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
                }
            }
  
        }
        else
        {
            playButton.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
        }

    }
    void LoadingState()
    {
        LoadingObject.SetActive(true);
        RandomFunction.GetComponent<LoadingTipRandom>().SendMessage("LodaingTipTextRandomStart", SendMessageOptions.DontRequireReceiver);
        mainPanel.gameObject.SetActive(false);
        for (int i = 0; i < title_3dUI.Length; i++)
        {
            title_3dUI[i].SetActive(false);
        }
        settingObject.SetActive(false);
        Invoke("StageSelectPanelStart", 3.0f);
    }

    void SetMainState()
    {
        nowState = TitleState.main;
        mainPanel.gameObject.SetActive(true);
        selectStagePanel.gameObject.SetActive(false);

        //SoundManager.manager.playSound(SoundIndex.click);//2015-07-22 변경
    }
    void SetPlayState()
    {
        NewSoundMgr.instance.PlaySingle(clickSound);
        if(option_Animation.enabled == false)
        {
            //StageSelectPanelStart();
            LoadingState();
            SettingAnimationOff();
        }
        else if(option_Animation.enabled == true)
        {
            if(!option_Animation.GetBool("SettingOnOff"))
            {
                option_Animation.SetBool("SettingOnOff", true);
                Invoke("LoadingState", NextTime);
                Invoke("SettingAnimationOff", NextTime);
            }
            else if(option_Animation.GetBool("SettingOnOff"))
            {
                //StageSelectPanelStart();
                LoadingState(); 
                SettingAnimationOff();
            }
        }
    }
    void SettingAnimationOff()
    {
        option_Animation.enabled = false;
        IsOption = true;
        IsOptionOnOff = true;
    }
    void StageSelectPanelStart()
    {
        nowState = TitleState.play;

        //mainPanel.gameObject.SetActive(false);
        LoadingObject.SetActive(false);
        selectStagePanel.gameObject.SetActive(true);

        selectStagePanel.GetComponent<GameModeController>().Loading();
        selectStagePanel.gameObject.transform.FindChild("ChooseStage").gameObject.GetComponent<StageSelectMgr>().Sett();

        //for (int i = 0; i < title_3dUI.Length; i++)
        //{
        //    title_3dUI[i].SetActive(false);
        //}

        //settingObject.SetActive(false);
    }
    public void SetOptionState()
    {
        nowState = TitleState.option;
        //mainPanel.gameObject.SetActive(false);
        //selectStagePanel.gameObject.SetActive(false);
        //optionPanel.gameObject.SetActive(true);

        //SoundManager.manager.playSound(SoundIndex.click); //2015-07-22 변경
        NewSoundMgr.instance.PlaySingle(clickSound);// 2015-09-10 새로운 사운드 매니저 추가

        if(IsOption)
        {
            option_Animation.enabled = true;
            IsOption = false;
        }
        else if (!IsOption)
        {
            if (IsOptionOnOff)
            {
                option_Animation.SetBool("SettingOnOff", true);
                IsOptionOnOff = false;
            }
            else if (!IsOptionOnOff)
            {
                option_Animation.SetBool("SettingOnOff", false);
                IsOptionOnOff = true;
            }

        }
    }
    public void SoundOnOff()
    {
        if (IsOptionOnOff)
        {
            if (IsSoundOnOff)
            {
                NewSoundMgr.instance.BgmSoundOff();
                IsSoundOnOff = false;
                soundButton.GetComponent<Image>().sprite = soundOff;
              
            }
            else if(!IsSoundOnOff)
            {
                NewSoundMgr.instance.BgmSoundOn();
                IsSoundOnOff = true;
                soundButton.GetComponent<Image>().sprite = soundOn;
            }
        }
        
    }
    void GoToLoading()
    {
        //SoundManager.manager.playSound(SoundIndex.click);//2015-07-22 변경
        NewSoundMgr.instance.PlaySingle(clickSound);// 2015-09-10 새로운 사운드 매니저 추가
        Application.LoadLevel("Loading");
    }
    public void QuitGame()
    {
        if (IsOptionOnOff)
        {
            Application.Quit();
            Debug.Log("종료");
        }
    }


}
