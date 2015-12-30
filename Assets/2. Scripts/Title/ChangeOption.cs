using UnityEngine;
using System.Collections;

public class ChangeOption : MonoBehaviour {
    public enum Option { BGM_ON_OFF, SE_ON_OFF };
    public Option howOption;


    public AudioClip OptionButtonSound;// 2015-09-10 새로운 사운드 매니저 추가

    UIButton button;

    void Start()
    {
        button = gameObject.GetComponent<UIButton>();

        if (howOption == Option.BGM_ON_OFF)
        {
            if (SettingValue.GetData.isEnableBGM == false)
            {
                ChangeButtonSprite();
            }
        }

        if (howOption == Option.SE_ON_OFF)
        {
            if (SettingValue.GetData.isEnableSE == false)
            {
                ChangeButtonSprite();
            }
        }
    }
    void OnClick()
    {
        switch (howOption)
        {
            case Option.BGM_ON_OFF:
                OnOffBgm();
                break;
            case Option.SE_ON_OFF:
                OnOffSE();
                break;
        }
    }

    public void OnOffBgm()
    {
        ChangeButtonSprite();
        SettingValue.GetData.isEnableBGM = !SettingValue.GetData.isEnableBGM;

        /********************** 2015-09-10 새로운 사운드 매니저 추가 ****************************/
        NewSoundMgr.instance.PlaySingle(OptionButtonSound);
        if (howOption == Option.BGM_ON_OFF)
        {
            if (NewSoundMgr.instance._BgmSoundOn)
            {
                NewSoundMgr.instance.BgmSoundOff();
            }
            else if (!NewSoundMgr.instance._BgmSoundOn)
            {
                NewSoundMgr.instance.BgmSoundOn();
            }
        }
    }
    public void OnOffSE()
    {
        ChangeButtonSprite();
        SettingValue.GetData.isEnableSE = !SettingValue.GetData.isEnableSE;

        /********************** 2015-09-10 새로운 사운드 매니저 추가 ****************************/
        NewSoundMgr.instance.EfxSoundOff();
        //NewSoundMgr.instance.PlaySingle(OptionButtonSound);
        if (howOption == Option.SE_ON_OFF)
        {
            if (NewSoundMgr.instance._SESoundOn)
            {
                NewSoundMgr.instance.EfxSoundOff();
            }
            else if (!NewSoundMgr.instance._SESoundOn)
            {
                NewSoundMgr.instance.EfxSoundOn();
            }
        }
    }

    void ChangeButtonSprite()
    {
        ////string tempStr = button.normalSprite;
        //button.normalSprite = button.pressedSprite;
        //button.hoverSprite = button.pressedSprite;
        //button.pressedSprite = tempStr;
    }
}
