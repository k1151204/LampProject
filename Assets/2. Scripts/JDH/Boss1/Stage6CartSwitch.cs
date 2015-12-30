using UnityEngine;
using System.Collections;

public class Stage6CartSwitch : MonoBehaviour 
{
    public GameObject Path = null;
    bool CollCheck = false; // 한번만 충돌을위해
    public GameObject[] ReMoveObject;
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player"))
        {
            if (Path != null)
            {
                if (!CollCheck)
                {
                    Path.GetComponent<PathMove>().StartMove();
                    for (int i = 0; i < ReMoveObject.Length; i++)
                    {
                        if (ReMoveObject[i] != null)
                        {
                            DestroyObject(ReMoveObject[i]);
                        }
                    }
                }
                CollCheck = true;
            }
        }
    }
}
