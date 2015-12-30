using UnityEngine;
using System.Collections;

public class BgmSwitchScript : MonoBehaviour {

    //public BgmIndex playBgm = BgmIndex.Bgm_06;
    StageInfo stageInfo;

    void Start()
    {
        stageInfo = GameObject.Find("StageInfo").GetComponent<StageInfo>();
    }

    void OnTriggerEnter(Collider other)
    {
        //if( other.CompareTag("Player"))
        //    SoundManager.manager.playBgm(playBgm);
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //    SoundManager.manager.playBgm(stageInfo.StageBgm);
    }
}
