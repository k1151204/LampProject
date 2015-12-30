using UnityEngine;
using System.Collections;

public class StageInfo : MonoBehaviour {
    public enum SpawnObjType { Lamp, RuneStone };

    public SpawnObjType StartLamp = SpawnObjType.RuneStone; 
    public float LightCost = 1;
    public int LightCount = 7;
    public int SumLightCount = 10; //10개걧수
    public AudioClip stageBgm;

    void Start()
    {
        NewSoundMgr.instance.PlayBgm(stageBgm);
    }
}
