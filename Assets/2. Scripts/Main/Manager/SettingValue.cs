using UnityEngine;
using System.Collections;

public class SettingValue : MonoBehaviour {
    public bool isEnableBGM;
    public bool isEnableSE;
    public int gameMode;
    public int startStage = -1;
    int maxStageCnt;
    bool isLoadStage;
    GameObject[] SaveStage;
    public void Setting(int _max)
    {
        if(SaveStage == null)
        SaveStage = new GameObject[_max+1];

        maxStageCnt = _max;
        isLoadStage = true;
    }
    public GameObject GetStage(int _stagenumber)
    {
        return SaveStage[_stagenumber];
    }
    public void SetStage(GameObject _stage,int _stagenumber)
    {
        SaveStage[_stagenumber] = _stage;
    }
    public int GetStageCount()
    {
        return maxStageCnt;
    }
    public GameObject[] GetStageS()
    {
        return SaveStage;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        isEnableBGM = true;
        isEnableSE = true;
        gameMode = 1;
        startStage = -1;
        isLoadStage = false;
    }
    public bool isLoaedStage() { return isLoadStage; }



    public static SettingValue SSetingValue;
    public static SettingValue GetData
    {
        get
        {
            if (SSetingValue == null)
            {
                SSetingValue = FindObjectOfType(typeof(SettingValue)) as SettingValue;

                if (SSetingValue == null)
                    Debug.Log("ERROR");
            }
            return SSetingValue;

        }
    }
}
