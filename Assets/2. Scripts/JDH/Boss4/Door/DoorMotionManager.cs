using UnityEngine;
using System.Collections;

public class DoorMotionManager : MonoBehaviour {
    public GameObject CreateMonster;
    public int CreateMin = 4;
    public int CreateMax = 7;
    GameObject[] TempMonster;
    Boss3Motion t;
    
    int max; //총갯수?
	// Use this for initialization
	void Start () {
        t = GameObject.Find("Boss3").GetComponent<Boss3Motion>();

        max = Random.Range(CreateMin, CreateMax+1); // 4~7개생성
        TempMonster = new GameObject[max];
              Hashtable ht = new Hashtable();
              ht.Add("y", -1.915426f);
              ht.Add("islocal", true);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 1f); //1초생성
        ht.Add("oncompletetarget", gameObject);
        ht.Add("oncomplete", "CreateEnd");
        iTween.MoveTo(gameObject, ht);
	}
    void CreateEnd()
    {
        for (int i = 0; i < max; i++)
        {
            StartCoroutine(CreateMonsterMotion(i, i * 1.5f));
        }
    }
    IEnumerator CreateMonsterMotion(int number,float time)
    {
        yield return new WaitForSeconds(time);

        TempMonster[number] = (GameObject)Instantiate(CreateMonster);
        TempMonster[number].transform.parent = GameObject.Find("BossManagerEff").transform;
        TempMonster[number].transform.localPosition = new Vector3(this.transform.localPosition.x, -2.070902f, this.transform.localPosition.z);
        if (number == max - 1)
            DestroyObject(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (!t.GetBossNextIng())
            DestroyObject(this.gameObject);
	}
}
