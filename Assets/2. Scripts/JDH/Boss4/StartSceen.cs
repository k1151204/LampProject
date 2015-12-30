using UnityEngine;
using System.Collections;

public class StartSceen : MonoBehaviour {
    public GameObject Boss;
	// Use this for initialization
	void Start () {
        GameObject.Find("Main Camera").GetComponent<CameraController>().setCameraZoom(CameraZoom.bossZoom); //카메라는 무조건 보스줌
        GameObject.Find("Lamp").GetComponent<LampInfo>().setControl(false);
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetMoveY(0.7f); //한칸올리기

	}
	
	// Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("Lamp").GetComponent<LampInfo>().setControl(false);
            //UIController.PrintMessage("BOSS Start", 45);
            CameraController.SetShake(12.5f, 0.06f); // 6.5초동안 모든애니메이션진행
            Invoke("BoxDelete", 3f);
            Invoke("MoveBoss", 3.5f);
            Invoke("SetOffCamera", 12.5f); // 씬 끝날떄 
        }
    }
    void MoveBoss() // 보스이동
    {

        Hashtable ht = new Hashtable();
        ht.Add("y",2.2f);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("speed",0.85f );
        iTween.MoveTo(Boss, ht);
        
    }
    void SetOffCamera() // 씬끝나고
    {
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = GameObject.Find("Lamp").transform;
        GameObject.Find("Lamp").GetComponent<LampInfo>().setControl(true);
        GameObject.Find("Boss3").GetComponent<Boss3Motion>().SetCreateEff();
        GameObject.Find("Boss3").GetComponent<Boss3Motion>().StratBoss();    
        DestroyObject(GameObject.Find("StartSccenEvent").gameObject);
        GameObject.Find("Boss3").GetComponent<BossUI>().StartHpBar();
        
    }
    void BoxDelete()// 박스 없어지기
    {
        ExplodeObject[] test = GameObject.Find("MoveBox").GetComponentsInChildren<ExplodeObject>();
        for (int i = 0; i < test.Length; i++)
            test[i].StartEffect();
    }
}
