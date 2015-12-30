using UnityEngine;
using System.Collections;

public class ArrowTrapScript : MonoBehaviour {

    public float arrowStartPower = 0.15f;
    public float arrowAirFriction = 0.01f;
    public float standTime = 2.0f;
    public int arrowNum = 5;
    public bool infinityArrow;
    public bool activeOnAwake = true;
    public bool use3DSound = true;
    public GameObject arrowPrefab;

    int remainArrow;
    float timer;
    bool isActivate;
    GameObject gameManager;
    Transform goalPos;

	void Start () {
        gameManager = GameObject.Find("GameManager");
        goalPos = transform.FindChild("ArrowGoalPos");
        remainArrow = arrowNum;
        if (infinityArrow)
        {
            remainArrow = 1;
        }

        if (gameManager.GetComponent<GameManager>().isEnableSE() == false || use3DSound == false)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }

        if (activeOnAwake)
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
            timer += Time.deltaTime;
            if (timer >= standTime)
            {
                //화살이 남아있을 경우에만 발사
                if (remainArrow > 0)
                {
                    if (infinityArrow == false)
                    {
                        remainArrow -= 1;
                    }
                    //화살 생성
                    GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity) as GameObject;
                    arrow.transform.parent = transform;
                    ArrowScript script = arrow.GetComponent<ArrowScript>();
                    script.goalPos = goalPos;
                    script.arrowStartPower = arrowStartPower;
                    script.arrowAirFriction = arrowAirFriction;
                    script.gameManager = gameManager;
                }
                timer = 0;
            }
        }
	}

    public void ActivateStart()
    {
        isActivate = true;
        timer = standTime;
    }
    public void ActivateStop()
    {
        isActivate = false;
    }

}
