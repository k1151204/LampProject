using UnityEngine;

using System.Collections;
using System.IO;
using System;
using System.Threading;
public static class LightDataManager {
  
    private static int NowStage = 0; // 현재스테이지
    private static int[] ClearStage; // 스테이지 클리어가능했는지 안했는지?
    private static int[] NowLight; // 현재 그 스테이지의 라이트닝 갯수? 누른
    private static int MaxStage; // 스테이지 총갯수
    private static String NowModeName;
    private static int nowMoad;
    private static int NowLightCount; // 세이브스톤 전에꺼 키기
// 클리어시 상태
    private static int[] NowLightIndex; // 현재 스테이지의 각점등이 눌렸는지?
    private static int[] StatCount;
    private static int SumLight; // 총라이트갯수
    private static bool check =  true;
	// Use this for initialization
    private static bool CreateCheck = false;
    private static bool GoToStageSelete = false;
    private static int[] ChListBuyOk;
    public static int GetSumLight()
    {
        return SumLight;
    }
    public static bool GetGoToStageSelete()
    {
        return GoToStageSelete;
    }
    public static void SetGoToStageSelete(bool _set)
    {
        GoToStageSelete = _set;
    }
    public static void SetSumLight(int _set,int test = 0)
    {
        if (test == 0)
            SumLight += _set;
        else
            SumLight = _set;
            
    }
    public static void Setting(int _max,int _maxCh,int _nowMoad,bool TestMode) // 스테이지 총갯수
    {

        if (check)
        {
            ClearStage = new int[_max];
            NowLight = new int[_max];
            StatCount = new int[_max];
            ChListBuyOk = new int[_maxCh];
            NowLightIndex = new int[7];
            MaxStage = _max;
            nowMoad = _nowMoad;
            NowLightCount = 0;

            if (nowMoad == 1)
            {
                NowModeName = "/1.txt";
            }
            else if (nowMoad == 2)
            {
                NowModeName = "/2.txt";

            }
            else if (nowMoad == 3)
            {
                NowModeName = "/3.txt";

            }
            FileInfo FI = new FileInfo(Application.persistentDataPath + NowModeName);
            if (TestMode == true)
            {
                FI.Create().Close();
                ReSetting();
                if (CreateCheck == false)
                    CreateCheck = true;
            }
            else
            {
                if (!FI.Exists)
                {
                    FI.Create().Close();
                    ReSetting();
                    if (CreateCheck == false)
                        CreateCheck = true;
                }
            }
            check = false;
        }
    }
    public static bool GetCheck()
    {
        return check;
    }
    
    public static bool GetCreate()
    {
        return CreateCheck;
    }
    public static void SetStar(int _stage, int _check)
    {
        StatCount[_stage] = _check;
    }
    public static int GetStar(int _stage)
    {
        return StatCount[_stage];
    }
    public static int GetNowLightCount()
    {
        return NowLightCount;
    }
    public static void SetNowLightCount(int _set)
    {
        NowLightCount = _set;
    }
    public static void ReSettingLightCount(int _set)
    {
        for (int i = _set+1; i < 7; i++)
            NowLightIndex[i] = 0;
    }
    public static void ReSettingLightCount()
    {
        for(int i= 0 ;i  < 7 ; i++)
            NowLightIndex[i] = 0;
     
    }
    public static string GetNowModeName()
    {
        return NowModeName;
    }
    public static void SetClear(int _stage,int _check)
    {
        ClearStage[_stage] = _check;
    }
    public static int GetClear(int _stage)
    {
        return ClearStage[_stage];
    }
    public static void SetLight(int _stage,int _conut)
    {
        NowLight[_stage] = _conut;
    }
    public static int GetLightIndex(int _index)
    {
        return NowLightIndex[_index];
    }
    public static void SetLightIndex(int _index,int _count)
    {
        NowLightIndex[ _index] = _count;
    }
    public static int GetLight()
    {
        return NowLight[NowStage];
    }
    public static int GetMaxStage()
    {
        return MaxStage;
    }
    public static void SetNowStage(int _now)
    {
        NowStage = _now; // 현재 스테이지
    }
    public static int GetNowStage()
    {
        return NowStage;
    }
    public static int GetChListOk(int _number)
    {
        return ChListBuyOk[_number];
    }
    public static void SetChListOk(int _number, int _set)
    {
        ChListBuyOk[_number] = _set;
    }
    public static void ReSettingSumLight()
    {
        SumLight = 600;
    }
    public static void ReSetting()
    {
        SumLight = 600;
        for (int i = 0; i < MaxStage; i++) // 총 갯수
        {
            ClearStage[i] = 1;
          
            NowLight[i] = 0;
            StatCount[i] = 0;
        }
        ClearStage[0] = 1;
        for (int i = 0; i < ChListBuyOk.Length; i++)
        {
            ChListBuyOk[i] = 1;
        }
        for (int j = 0; j < 7; j++)
            NowLightIndex[ j] = 0;


        SavaData();
    }
    public static void SavaData()
    {
        String FileName = Application.persistentDataPath + NowModeName;
        var sr = File.CreateText(FileName);
        sr.WriteLine(SumLight.ToString());
        String t = ChListBuyOk[0].ToString() + " ";
        for (int i = 1; i < ChListBuyOk.Length; i++)
        {
            if (ChListBuyOk.Length - 1 == i)
                t += ChListBuyOk[i].ToString();
            else
                t += ChListBuyOk[i].ToString() + " ";
        }
        sr.WriteLine(t);
        for (int i = 0; i < MaxStage; i++)
        {
            String saveData = ClearStage[i] + " " + StatCount[i];
            sr.WriteLine(saveData);
        }
        sr.Flush();
        sr.Close();
    }
    

}
