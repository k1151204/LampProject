using UnityEngine;
using System.Collections;

public class Stage9ObjectMove : MonoBehaviour {
    float TempZ;
    int type = 0;
	// Use this for initialization
	void Start () {
        TempZ = this.gameObject.transform.localPosition.z;
        type = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move()
    {
        Hashtable hash = new Hashtable();
        if (type == 0)
        {
            hash.Add("z", 0.8);
        }
        else
        {
            hash.Add("z", TempZ);

        }
            hash.Add("islocal", true);
        hash.Add("time",1.7f);
        hash.Add("easetype", iTween.EaseType.linear);
        hash.Add("oncompletetarget", gameObject);
        hash.Add("oncomplete", "End");
        iTween.MoveTo(gameObject, hash);
    }
    public void End()
    {
        if (type == 0)
            type = 1;
        else if (type == 1)
            type = 0;
        Move();
    }
}
