using UnityEngine;
using System.Collections;


[System.Serializable]
public class MoveInfo
{
    public float waitTime;
    public Vector3 arrivePos;
    public float speed;

    //설정한 목표 위치에 상관없이 초기 상태로 돌아가게 된다.
    public bool goToStart;
}


public class MovingWall : MonoBehaviour
{
    public bool ActivateOnAwake = true;
    public bool fixWidth = true;
    public MoveInfo[] movingPhase;        //에디터에서 정한 페이즈대로 회전된다.
    Vector3 startPos;                      //초기 위치 저장
    int nowPhase;
    Vector3 nowPos;
    Vector3 dirVec;                       //방향 벡터
    bool isEndPhase;                        //현재 페이즈가 끝났는지
    bool isActivate;

    void Start()
    {
        //움직이는 벽 스케일 고정
        if(fixWidth)
            transform.localScale = new Vector3(1, 1, 1);

        //변수 초기화
        nowPhase = 0;
        startPos = transform.localPosition;
        nowPos = startPos;
        //0번 페이즈로 초기화
        initPhase();

        if (ActivateOnAwake)
        {
            ActivateStart();
        }
        else
        {
            ActivateStop();
        }
    }

    void Update()
    {
        if (isActivate)
        {
            //현재 페이즈 정보대로 벽을 이동한다.
            transform.localPosition = nowPos;

            //지금 위치와 목표 위치의 갭이 Speed보다 작으면 보정해준다.
            float gap = (movingPhase[nowPhase].arrivePos - nowPos).sqrMagnitude;
            if (gap < Mathf.Pow(movingPhase[nowPhase].speed, 2))
            {
                nowPos = movingPhase[nowPhase].arrivePos;
            }

            //목표 지점까지 이동 후 목표에 닿으면 페이즈 종료.
            if (nowPos != movingPhase[nowPhase].arrivePos)
            {
                nowPos += movingPhase[nowPhase].speed * dirVec;
            }
            else
            {
                if (isEndPhase == false)
                {
                    //waitTime이 -1일 경우 대기시간 무한대
                    if (movingPhase[nowPhase].waitTime == -1)
                        movingPhase[nowPhase].waitTime = 999999;
                    Invoke("moveNextPhase", movingPhase[nowPhase].waitTime);
                    isEndPhase = true;
                }
            }
        }
    }

    void moveNextPhase()
    {
        nowPhase = (nowPhase + 1) % movingPhase.Length;
        initPhase();
    }

    void initPhase()
    {
        isEndPhase = false;

        if (movingPhase[nowPhase].goToStart)
        {
            movingPhase[nowPhase].arrivePos = startPos;
        }

        //방향부호 설정
        dirVec = movingPhase[nowPhase].arrivePos - nowPos;
        if ( !(-0.001f <= dirVec.z && dirVec.z <= 0.001f) )
        {
            BroadcastMessage("ZaxisCorrection", SendMessageOptions.DontRequireReceiver);
        }
        dirVec.Normalize();
    }

    public void ActivateStart()
    {
        isActivate = true;
    }
    public void ActivateStop()
    {
        isActivate = false;
    }

    public void ChangePhase(int num)
    {
        if (num < movingPhase.Length)
        {
            nowPhase = num;
            initPhase();
        }
    }

}
