using UnityEngine;
using System.Collections;

public class PathMove : MonoBehaviour
{
    public float MoveTime = 8f;
    public GameObject MoveObject;
    public string PathName;
    public GameObject[] ReMoveObject;
    public string[] NextPathName;
    public float[] NextPathTime;
    public bool Check = false;
    public GameObject[] Setton ;
    public bool JumpTest = false;
    public bool FirstJump = false;
    int index;
    int Max;
    float MaxTime;
    // Use this for initialization
    void Start()
    {
        index = 0;
        Max = NextPathName.Length +1   ;
        if(MoveObject == null)
        {
            MoveObject = GameObject.Find("Lamp");
        }
    }
    public void PathPause()
    {
        iTween.Pause(this.gameObject);
    }
    public void PathReStart()
    {
        iTween.Resume(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void StartMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(PathName)
               , "time", MoveTime
               , "easetype", iTween.EaseType.linear, "onupdatetarget", gameObject, "onupdate", "MoveIng"
               , "oncompletetarget", gameObject, "oncomplete", "MoveEnd"

               ));
    }
    void End()
    {
        GameObject.Find("Lamp").GetComponent<LampInfo>().setJumpState(false);
    }
    void MoveEnd()
    {

        index++;
        if (JumpTest == true)
        {
            if (Max - 1 == index)
            {
                End();
            }
           
        }
        if (FirstJump == true)
        {
            if (Max == index)
            {
                End();
            }
        }
        if (Max > index)
        {
                iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(NextPathName[index-1])
               , "time", NextPathTime[index-1]
               , "easetype", iTween.EaseType.linear, "onupdatetarget", gameObject, "onupdate", "MoveIng", "oncomplete", "MoveEnd"
               ));
        }
        if (index == 1)
        {
            if (Check == true)
            {
                for (int i = 0; i < Setton.Length;i++ )
                {
                    Setton[i].SetActive(!Setton[i].activeSelf);
                }
            }
            if (ReMoveObject != null)
            {
                for (int i = 0; i < ReMoveObject.Length; i++)
                {
                    ReMoveObject[i].GetComponent<ExplodeObject>().StartEffect();
                }
            }
        }
    }
    void MoveIng()
    {
        MoveObject.transform.LookAt(gameObject.transform);
        MoveObject.transform.position = gameObject.transform.position;
    }
}
