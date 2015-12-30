using UnityEngine;
using System.Collections;

public class PotAttack : MonoBehaviour {
    public GameObject AffectEffect;

    GameObject temp;

    float CreateTime;
	// Use this for initialization
	void Start () {
        CreateTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        CreateTime += Time.deltaTime;
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (!GameObject.Find("Boss").GetComponent<PotMotion>().GetEnd())
                return;
            if (CreateTime > 1)
            {
                temp = (GameObject)Instantiate(AffectEffect, transform.position, Quaternion.identity);
                temp.transform.parent = transform;
                GameObject.Find("Boss").GetComponent<PotMotion>().DamagedStart();
                GameObject.Find("Boss").GetComponent<PotMotion>().SetBossHp(10); // 7
                GameObject.Find("Boss").GetComponent<PotHelpWindow>().SetHpBar(10); //7
                GameObject.Find("Boss").GetComponent<PotMotion>().End();
                
                CreateTime = 0.0f;
            }
            // GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
    }
}
