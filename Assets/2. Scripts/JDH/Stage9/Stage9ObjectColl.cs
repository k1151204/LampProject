using UnityEngine;
using System.Collections;

public class Stage9ObjectColl : MonoBehaviour {
    public GameObject[] MoveObject;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(On());
        }
    }

     IEnumerator On()// 오브젝트키기
     {
         for (int i = 0; i < MoveObject.Length; i++)
         {
             MoveObject[i].SetActive(true);
             MoveObject[i].GetComponent<Stage9ObjectMove>().Move();
             yield return new WaitForSeconds(0.7f);
         }
         DestroyObject(this.gameObject);
     }
}
