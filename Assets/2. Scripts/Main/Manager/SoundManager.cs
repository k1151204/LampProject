//using UnityEngine;
//using System.Collections;

//public enum SoundIndex
//{
//    // 타이틀 , 오프닝 
//    Title_Lamp, selectStage, Book_Write, StartPaging, Paging,
   
//    // 인 게임
//    StageClaer, PlayerJump, Lever,
//    click, click2, openningDropLamp, openningEnterLamp, lightArrow, lightArrowExplosion,
//    lightSE, ClickSE, LampDieSE, PressJumpPadSE, UpJumpPadSE,
//    UseBrighten, BrokenLamp, 
//};

//public enum BgmIndex { None, Bgm_01, Bgm_02, Bgm_03, Bgm_04, Bgm_05, Bgm_06, Bgm_07, Bgm_08, Bgm_09 };

//public class SoundManager : MonoBehaviour {

//    AudioSource AudioBGM;
//    AudioSource AudioSE;

//    AudioClip[] SoundList;
//    AudioClip[] BgmList;
//    int maxSoundNum = 40;
//    int maxBgmNum = 10;

//    SettingValue settingValue;
//    BgmIndex nextBgm;

//    //싱글톤 패턴 적용
//    public static SoundManager manager;
//    public static void Init()
//    {
//        if (manager == null)
//        {
//            manager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
//            //SoundManager 게임오브젝트를 찾았는데도 없는 경우
//            if (manager == null)
//            {
//                //사운드매니저 게임오브젝트를 생성한다.
//                manager = new GameObject("SoundManager",
//                    typeof(SoundManager)).GetComponent<SoundManager>();
//                //오디오 소스 컴포넌트 추가
//                manager.AudioBGM = manager.gameObject.AddComponent<AudioSource>();
//                manager.AudioBGM.loop = true;
//                manager.AudioSE = manager.gameObject.AddComponent<AudioSource>();
//                //환경설정 받아오기
//                GameObject settingValueObject = GameObject.FindGameObjectWithTag("setting") as GameObject;
//                if (settingValueObject == null)
//                {
//                    GameObject tempResource = Resources.Load("MainGame/Prefab/GlobalObject/SettingValue") as GameObject;
//                    GameObject temp = Instantiate(tempResource, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
//                    manager.settingValue = temp.GetComponent<SettingValue>();
//                }
//                else
//                {
//                    manager.settingValue = settingValueObject.GetComponent<SettingValue>();
//                }

//                manager.SoundList = new AudioClip[manager.maxSoundNum];
//                manager.BgmList = new AudioClip[manager.maxBgmNum];

//                // BGM 9EA(2015-09-09)
//                manager.BgmList[(int)BgmIndex.Bgm_01] = Resources.Load("_Sounds/BGM/bgm_01") as AudioClip; // 타이틀
//                manager.BgmList[(int)BgmIndex.Bgm_02] = Resources.Load("_Sounds/BGM/bgm_02") as AudioClip; // 광산
//                manager.BgmList[(int)BgmIndex.Bgm_03] = Resources.Load("_Sounds/BGM/bgm_03") as AudioClip; // 야외
//                manager.BgmList[(int)BgmIndex.Bgm_04] = Resources.Load("_Sounds/BGM/bgm_04") as AudioClip; // 무덤
//                manager.BgmList[(int)BgmIndex.Bgm_05] = Resources.Load("_Sounds/BGM/bgm_05") as AudioClip; // 신전
//                manager.BgmList[(int)BgmIndex.Bgm_06] = Resources.Load("_Sounds/BGM/bgm_06") as AudioClip; // Boss_01
//                manager.BgmList[(int)BgmIndex.Bgm_07] = Resources.Load("_Sounds/BGM/bgm_07") as AudioClip; // 오프닝 Bgm(임시), 크리스탈
//                manager.BgmList[(int)BgmIndex.Bgm_08] = Resources.Load("_Sounds/BGM/bgm_08") as AudioClip; // Boss_02
//                manager.BgmList[(int)BgmIndex.Bgm_09] = Resources.Load("_Sounds/BGM/bgm_09") as AudioClip; // Boss_03

//                // Effect_Sound(2015-09-09 이전)
//                manager.SoundList[(int)SoundIndex.click] = Resources.Load("_Sounds/SE/click") as AudioClip;
//                manager.SoundList[(int)SoundIndex.click2] = Resources.Load("_Sounds/SE/click2") as AudioClip;
//                manager.SoundList[(int)SoundIndex.selectStage] = Resources.Load("_Sounds/SE/selectStage") as AudioClip;
//                manager.SoundList[(int)SoundIndex.openningDropLamp] = Resources.Load("_Sounds/SE(new)/Openning_Lamp_Drop") as AudioClip;
//                manager.SoundList[(int)SoundIndex.openningEnterLamp] = Resources.Load("_Sounds/SE/Laser") as AudioClip;
//                manager.SoundList[(int)SoundIndex.lightArrow] = Resources.Load("_Sounds/SE/Laser_1") as AudioClip;
//                manager.SoundList[(int)SoundIndex.lightArrowExplosion] = Resources.Load("_Sounds/SE/useBrighten2") as AudioClip;

//                manager.SoundList[(int)SoundIndex.lightSE] = Resources.Load("_Sounds/SE/lightSE") as AudioClip;
//                manager.SoundList[(int)SoundIndex.ClickSE] = Resources.Load("_Sounds/SE/clickSE") as AudioClip;
//                manager.SoundList[(int)SoundIndex.LampDieSE] = Resources.Load("_Sounds/SE/lampDie1") as AudioClip;
//                manager.SoundList[(int)SoundIndex.PressJumpPadSE] = Resources.Load("_Sounds/SE/pressJumpPadSE") as AudioClip;
//                manager.SoundList[(int)SoundIndex.UpJumpPadSE] = Resources.Load("_Sounds/SE/upJumpPadSE") as AudioClip;
//                manager.SoundList[(int)SoundIndex.UseBrighten] = Resources.Load("_Sounds/SE/useBrighten2") as AudioClip;
//                manager.SoundList[(int)SoundIndex.BrokenLamp] = Resources.Load("_Sounds/SE/brokenLamp") as AudioClip;

//                // Effect_Sound(2015-09-09 이후)
//                manager.SoundList[(int)SoundIndex.StageClaer] = Resources.Load("_Sounds/SE(new)/StageClear") as AudioClip;  // Stage Claer 사운드
//                manager.SoundList[(int)SoundIndex.openningDropLamp] = Resources.Load("_Sounds/SE(new)/OpenningDropLamp") as AudioClip;
//                manager.SoundList[(int)SoundIndex.PlayerJump] = Resources.Load("_Sounds/SE(new)/PlayerJump") as AudioClip; // Player 점프 사운드
//                manager.SoundList[(int)SoundIndex.Lever] = Resources.Load("_Sounds/SE(new)/lever") as AudioClip; // Lever 작동 사운드

//                // 타이틀 , 오프닝 사운드(2015-09-08 이후)
//                manager.SoundList[(int)SoundIndex.Title_Lamp] = Resources.Load("_Sounds/SE(new)/Title_Lamp") as AudioClip; // 타이틀화면 Lamp 움직이는 사운드
//                manager.SoundList[(int)SoundIndex.selectStage] = Resources.Load("_Sounds/SE(new)/selectStage") as AudioClip; // 선택 사운드
//                manager.SoundList[(int)SoundIndex.Book_Write] = Resources.Load("_Sounds/SE(new)/Book_Write") as AudioClip; // 오프닝 Lamp 글씨쓰기
//                manager.SoundList[(int)SoundIndex.StartPaging] = Resources.Load("_Sounds/SE(new)/StartPaging") as AudioClip; // 오프닝 책 첫장 넘기기
//                manager.SoundList[(int)SoundIndex.Paging] = Resources.Load("_Sounds/SE(new)/Paging") as AudioClip; // 오프닝 책 중간장 넘기기

//                //씬이 변경되도 제거되지 않도록 설정.
//                DontDestroyOnLoad(manager.gameObject);
//            }
//        }
//    }

//    public void playSound(SoundIndex index,float volume = 1.0f)
//    {
//        if (settingValue.isEnableSE)
//        {
//            AudioSE.volume = volume;
//            AudioSE.PlayOneShot(SoundList[(int)index]);
//        }
//    }
//    public void playBgm(BgmIndex index, bool isPadeOut = true)
//    {
//        if (settingValue.isEnableBGM && index != BgmIndex.None)
//        {
//            //기존에 padeOut, padeIn 중인 배경음악이 있다면 Invoke를 종료한다.
//            CancelInvoke("padeOutBgm");
//            CancelInvoke("padeInBgm");

//            //다음 재생 될 배경음악을 갱신한다.
//            nextBgm = index;

//            if (isPadeOut)
//            {
//                //4초에 걸쳐 volume을 padeOut 한 뒤 새로운 배경음악을 재생한다.
//                padeOutBgm();
//            }
//            else
//            {
//                AudioBGM.volume = 0.0f;
//                AudioBGM.clip = BgmList[(int)nextBgm];
//                AudioBGM.Play();
//                padeInBgm();
//            }
//        }
//        else if( index == BgmIndex.None)
//        {
//            CancelInvoke("padeOutBgm");
//            CancelInvoke("padeInBgm");
//            nextBgm = index;
//            if (isPadeOut)
//            {
//                //4초에 걸쳐 volume을 padeOut 한 뒤 새로운 배경음악을 재생한다.
//                padeOutBgm();
//            }
//            else
//            {
//                AudioBGM.volume = 0.0f;
//            }
//        }
//    }
//    void padeOutBgm()
//    {
//        //4초에 걸쳐 배경음악 페이드 아웃
//        AudioBGM.volume -= 0.025f;
//        if (AudioBGM.volume > 0.0f)
//        {
//            Invoke("padeOutBgm", 0.1f);
//        }
//        else
//        {
//            //배경음악 클립 변경
//            AudioBGM.clip = BgmList[(int)nextBgm];
//            AudioBGM.Play();
//            padeInBgm();
//        }
//    }
//    void padeInBgm()
//    {
//        //2초에 걸쳐 배경음악 페이드 인
//        AudioBGM.volume += 0.05f;
//        if (AudioBGM.volume < 1.0f)
//            Invoke("padeInBgm", 0.1f);
//    }
//    public void ChangeMuteSetupBGM()
//    {
//        AudioBGM.mute = !AudioBGM.mute;
//    }
//    public void ChangeMuteSetupSE()
//    {
//        AudioSE.mute = !AudioSE.mute;
//    }
//    public void SetBgmVolume(float set)
//    {
//        AudioBGM.volume = set;
//    }

//}
