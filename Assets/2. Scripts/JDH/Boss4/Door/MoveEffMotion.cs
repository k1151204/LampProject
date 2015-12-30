using UnityEngine;
using System.Collections;

public class MoveEffMotion : MonoBehaviour {
    GameObject Boss;
    Boss3Motion t;
	// Use this for initialization
	void Start () {
        Boss = GameObject.Find("Boss3");
        t = GameObject.Find("Boss3").GetComponent<Boss3Motion>();
        Move();
	}
	
	// Update is called once per frame
	void Update () {
        if (!t.GetBossNextIng() )
            DestroyObject(this.gameObject);
	}
    void Move()
    {
        Hashtable ht = new Hashtable();
        ht.Add("position", Boss.transform.position);
   
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", 1f); //속도
        ht.Add("oncompletetarget", gameObject);
        ht.Add("oncomplete", "MoveEnd");
        iTween.MoveTo(gameObject, ht);
    }
    void MoveEnd()
    {
        DestroyObject(this.gameObject);
        if (Boss.GetComponent<Boss3Motion>().GetBossHp() < 100)
        {
            Boss.GetComponent<Boss3Motion>().SetBossHp(Boss.GetComponent<Boss3Motion>().GetBossHp() + 5);
            Boss.GetComponent<BossUI>().ShowHpBar(-5);
        }   
    }
}
