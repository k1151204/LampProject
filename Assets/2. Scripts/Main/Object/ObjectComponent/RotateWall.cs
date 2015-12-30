using UnityEngine;
using System.Collections;


public enum RotateType { center,left,right, leftTop, leftBottom, rightTop, rightBottom };

[System.Serializable]
public class RotateInfo
{
    public float waitTime;
    public float angle;
    public float speed;

    //설정한 angle에 상관없이 초기 상태로 돌아가게 된다.
    public bool isResetRotation;
}


public class RotateWall : MonoBehaviour {

    public bool ActivateOnAwake = true;
    public RotateType rotateType;
    public RotateInfo[] rotatePhase;        //에디터에서 정한 페이즈대로 회전된다.
    int nowPhase;
    float rotateZ;
    float initRotateZ;                      //초기 회전 정보 저장
    GameObject wall;
    Vector3 initPos;                        //초기 위치 저장
    int dirSign;                            //방향 부호
    bool isEndPhase;                        //현재 페이즈가 끝났는지
    bool isActivate;

    void Start()
    {
        //변수 초기화
        dirSign = 1;
        nowPhase = 0;
        initPos = transform.localPosition;
        initRotateZ = transform.localRotation.eulerAngles.z;
        rotateZ = initRotateZ;
        wall = transform.FindChild("Wall").gameObject;

        //회전 타입에 맞게 위치 조정
        setupPosition();
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

	void Update () {
        if (isActivate)
        {
            //현재 페이즈 정보대로 벽을 회전한다.
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));

            //지금 회전각도와 목표 회전각도의 갭이 Speed보다 작으면 보정해준다.
            float gap = Mathf.Abs(rotateZ - rotatePhase[nowPhase].angle);
            if (gap < rotatePhase[nowPhase].speed)
            {
                rotateZ = rotatePhase[nowPhase].angle;
            }

            //페이즈가 1개밖에 없을 경우에 대한 처리
            if (rotatePhase.Length == 1)
            {
                rotateZ += rotatePhase[nowPhase].speed * dirSign;
            }
            else
            {
                //목표 회전각도가 아니면 회전하고, 목표에 닿으면 페이즈 종료.
                if (rotateZ != rotatePhase[nowPhase].angle)
                {
                    rotateZ += rotatePhase[nowPhase].speed * dirSign;
                }
                else
                {
                    if (isEndPhase == false)
                    {
                        Invoke("moveNextPhase", rotatePhase[nowPhase].waitTime);
                        isEndPhase = true;
                    }
                }
            }
        }

	}


    void moveNextPhase()
    {
        nowPhase = (nowPhase + 1) % rotatePhase.Length;
        initPhase();
    }
    void initPhase()
    {
        isEndPhase = false;

        if (rotatePhase[nowPhase].isResetRotation)
        {
            rotatePhase[nowPhase].angle = initRotateZ;
        }

        //방향부호 설정
        dirSign = (rotatePhase[nowPhase].angle - rotateZ > 0)? 1 : -1;


    }

    void setupPosition()
    {
        float offsetX = wall.transform.localScale.x * 0.5f;
        float offsetY = wall.transform.localScale.y * 0.5f;
        switch (rotateType)
        {
            case RotateType.center:
                transform.localPosition = initPos;
                wall.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case RotateType.left:
                wall.transform.localPosition = new Vector3(offsetX, 0, 0);
                transform.Translate(new Vector3(-offsetX, 0, 0));
                break;
            case RotateType.right:
                wall.transform.localPosition = new Vector3(-offsetX, 0, 0);
                transform.Translate(new Vector3(offsetX, 0, 0));
                break;
            case RotateType.leftTop:
                wall.transform.localPosition = new Vector3(offsetX, -offsetY, 0);
                transform.Translate(new Vector3(-offsetX, offsetY, 0));
                break;
            case RotateType.leftBottom:
                wall.transform.localPosition = new Vector3(offsetX, offsetY, 0);
                transform.Translate(new Vector3(-offsetX, -offsetY, 0));
                break;
            case RotateType.rightTop:
                wall.transform.localPosition = new Vector3(-offsetX, -offsetY, 0);
                transform.Translate(new Vector3(offsetX, offsetY, 0));
                break;
            case RotateType.rightBottom:
                wall.transform.localPosition = new Vector3(-offsetX, offsetY, 0);
                transform.Translate(new Vector3(offsetX, -offsetY, 0));
                break;
        }
    }

    public void ActivateStart()
    {
        isActivate = true;
    }
    public void ActivateStop()
    {
        isActivate = false;
    }


}
