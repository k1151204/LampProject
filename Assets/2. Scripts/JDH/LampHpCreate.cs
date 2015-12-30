using UnityEngine;
using System.Collections;

public class LampHpCreate : MonoBehaviour {
    public int Count;
     GameObject LampHp;
    GameObject[] LampObject;
    public void CreateObject()
    {

        if(LampHp == null)
            LampHp = LampCharChage.GetData.GetEff((int)LampChar.LAMPHP);
        LampObject = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            LampObject[i] = (GameObject)Instantiate(LampHp);
            LampObject[i].GetComponent<LampHpMove>().SetSpeed(((i+1) * 0.7f));
            LampObject[i].GetComponent<LampHpMove>().SetRot((((i * 0.5f) + 0.5f)));

        }
    }
   
 
    public void DeleteObject()
    {
        if (LampObject != null)
        {
            for (int i = 0; i < LampObject.Length; i++)
            {
                if (LampObject[i] != null)
                    DestroyObject(LampObject[i]);
            }
        }
    }
    public void SetMoveCheck(bool _set)
    {
        for (int i = 0; i < Count; i++)
        {
            LampObject[i].GetComponent<LampHpMove>().SetMoveCheck(_set);
        }
    }

    
}
