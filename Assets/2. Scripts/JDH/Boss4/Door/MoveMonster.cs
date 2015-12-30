using UnityEngine;
using System.Collections;
// - 2.75 ~ 2.75
public class MoveMonster : MonoBehaviour {
    public GameObject MoveEff; // 램프와 붙였을때 보스로 빨려들어가는 임펙트?
    float speedTime; // 몬스터 속도
    int Dir; //방향

    public float lifetime = 9f; // 탄생후 9초동안 안죽이면 보스 hp회복
    float time;
    Boss3Motion t;

	// Use this for initialization
	void Start () {
        speedTime = Random.Range(1.5f, 2.5f);
        DirCheck();
        time = 0;
        t = GameObject.Find("Boss3").GetComponent<Boss3Motion>();

        Move();
	}
    void Move()
    {
        Hashtable ht = new Hashtable();
        if(Dir == 0)
        ht.Add("x", transform.position.x - 1.4f);
        else if(Dir == 1)
            ht.Add("x", transform.position.x + 1.4f);
        ht.Add("easetype", iTween.EaseType.linear);
        ht.Add("time", speedTime); //속도
        ht.Add("oncompletetarget", gameObject);
        ht.Add("oncomplete", "MoveEnd");
        iTween.MoveTo(gameObject, ht);
    }
    void MoveEnd()
    {
        DirCheck();
        Move();
    }
	// Update is called once per frame
    void Update()
    {
        if (!t.GetBossNextIng())
            DestroyObject(this.gameObject);
        time += Time.deltaTime;
        if (time > lifetime)
        {
            CreateEff();
        }
    }
    public void HitedLight()
    {
        DestroyObject(this.gameObject);
    }
    void DirCheck()
    {
        if (transform.position.x - 1.4f < -2.75f)
            Dir = 1; // 오른쪽으로
        else if (transform.position.x + 1.4f > 2.75f)
            Dir = 0; // 왼쪽으로
        else
            Dir = Random.Range(0, 2);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CreateEff();
        }
    }
    public void CreateEff()
    {
        GameObject Temp = (GameObject)Instantiate(MoveEff);
        Temp.transform.parent = GameObject.Find("BossManagerEff").transform;
        Temp.transform.position = transform.position;
        DestroyObject(this.gameObject);

    }
}
