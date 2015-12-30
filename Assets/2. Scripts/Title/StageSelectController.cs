using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Threading;
public class StageSelectController : MonoBehaviour {

    SettingValue settingValue;
    StringBuilder sb = new StringBuilder();
    int maxStageCnt;
    void Awake()
    {

        settingValue = GameObject.FindGameObjectWithTag("setting").GetComponent<SettingValue>();
        maxStageCnt = settingValue.GetStageCount();

    }
  
    public void ActiveStageButton()
    {
        FileLoad();
        FileLampData();
       
    }
   
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
    public void FileLampData()
    {
        StreamReader w = new StreamReader(Application.persistentDataPath + "/CharData.txt");
        string a = w.ReadLine();
        string[] a1 = a.Split(' ');
        LampCharData.SetNowCharData(Int32.Parse(a1[0]));
        for (int i = 0; i < LampCharData.GetCount(); i++)
        {
            string s = w.ReadLine();
            string[] test = s.Split(' ');
            LampCharData.SetChar(i, Int32.Parse(test[0]));
        }
        w.Close();
    }
   
    public void FileLoad() // 현재파일 얻어오기
    {

        StreamReader w = new StreamReader(Application.persistentDataPath + LightDataManager.GetNowModeName());
        string a = w.ReadLine();
        LightDataManager.SetSumLight(Int32.Parse(a), 1);
        GameObject.Find("SumLightLabel").GetComponent<UILabel>().text = LightDataManager.GetSumLight().ToString();
       a = w.ReadLine();

       string[] test1 = a.Split(' ');
       for (int i = 0; i < test1.Length; i++)
       {
           LightDataManager.SetChListOk(i, Int32.Parse(test1[i]));
           Debug.Log(Int32.Parse(test1[i]));
       }
        for (int i = 0; i < LightDataManager.GetMaxStage(); i++)
        {
            string s = w.ReadLine();
            string[] test = s.Split(' ');
            LightDataManager.SetClear(i, Int32.Parse(test[0]));
            LightDataManager.SetStar(i, Int32.Parse(test[1]));
        }

        LightDataManager.ReSettingLightCount();
        w.Close();

    }
}
