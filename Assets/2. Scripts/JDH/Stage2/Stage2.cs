using UnityEngine;
using System.Collections;

public class Stage2 : MonoBehaviour {
    int coll = 0;
	// Use this for initialization
	void Start () {
        coll = 0;
	}
    public void SetColl()
    {
        coll++;
    }
    public void ReSettingColl()
    {
        coll = 0;
    }
	// Update is called once per frame
	void Update () {
        if (coll >= 2)
        {
            GameObject.Find("GameManager").SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
	}
}
