using UnityEngine;
using System.Collections;

public class LampMoveManager : MonoBehaviour {
    public bool CehckJump = false; // 점프시 특별한상황인지 z축? (20스테이지)

     GameObject JumpEff;
    public float jumpDelay = 0.35f;
	GameObject lamp;
	LampInfo lampInfo;
	
	UIJoystick JoystickInfo;
	UISprite PadeSprite;
	UISprite BaseSprite;

	bool JoystickCheck;  // 현재 조이스틱 누른상태인지 아닌지
	
	float TempPadeColorA;
    float TempBaseColorA;
	float MoveNumber = 0; // 가속도를위한값;
	public bool JoystickIng;  // 조이스틱이 누를수있는 상태? 없는상태?
    float jumptemptime;
    int MoveDir;
    int TempDir;
     GameObject joyStcikTutorial;
     GameObject right_JoyStcikTutorial;
     GameObject left_JoyStcikTutorial;
     bool TJumpCheck;
     float joystickWeight;

    bool ChageJump;
    GameObject JumpTr;
    int type; // 0이면 기본 / 1이면 물 (18스테이지위해추가) // 2015 8 3
    int typecheck;


	// Use this for initialization
	void Start()
	{
        JumpTr = GameObject.Find("Tutorial").transform.FindChild("JumpLable").gameObject;
        TJumpCheck = true;
        type = 0;
        typecheck = 0;
        if (joyStcikTutorial == null)
        {
            joyStcikTutorial = GameObject.Find("Tutorial").transform.FindChild("JoyStickTutorial").gameObject;
            right_JoyStcikTutorial = joyStcikTutorial.transform.FindChild("JoyStickTutorial(Right)").gameObject;
            left_JoyStcikTutorial = joyStcikTutorial.transform.FindChild("JoyStickTutorial(Left)").gameObject;
        }
        if (JumpEff == null)
        {
            JumpEff = LampCharChage.GetData.GetEff((int)LampChar.JUMP);
        }
        ChageJump = true;
        MoveDir = 0;
        TempDir = 0;
		lamp = gameObject;

		lampInfo = GetComponent<LampInfo>();
		JoystickInfo = GameObject.Find("JoyStick").GetComponent<UIJoystick>();
		PadeSprite = GameObject.Find("Pade").GetComponent<UISprite>();
        BaseSprite = GameObject.Find("Base").GetComponent<UISprite>();

		MoveNumber = (1 / JoystickInfo.radius);
		JoystickCheck = false;
		JoystickIng = true;
		TempPadeColorA = PadeSprite.color.a;
        TempBaseColorA = BaseSprite.color.a;
        jumptemptime = 0.0f;
        lampInfo.setJumpState(false);
	}
    public void SetType(int _type)
    {
        type = _type;
    }
        
    public void SetChageJump(bool _set)
    {
        ChageJump = _set;
    }
    // Update is called once per frame
  
    void Update()
    {
        if (!lampInfo.isPosibleControl())
            return;
        
        if (JoystickInfo.GetPress())
        {
            JoystickDown();
        }
        else
            JoystickUp();


        if (Input.GetKey(KeyCode.Space))
        {
            OnJumpButton();
        }
        if (type == 1)
        {
            if (typecheck == 0)
            {
                if (lampInfo.isJump() == true)
                {
                    Invoke("ResettingJump", 0.3f);
                    typecheck = 1;
                }
            }
        }
      
    }
    public void ResettingJump()
    {
        if (typecheck == 1)
        {
            lampInfo.setJumpState(false);
            typecheck = 0;
        }

    }

    void JoystickDown()
    {
        if (StoryTeller.Instanace.IsOpenedStoryTeller())
            return;
        if (!JoystickIng)
            return;
        PadeSprite.transform.localPosition = new Vector3(PadeSprite.transform.localPosition.x, 0, 0);
        MoveLamp(PadeSprite.transform.localPosition.x);
        StartCoroutine(StartFadaIn(PadeSprite, TempPadeColorA, 1F));
        StartCoroutine(StartFadaIn(BaseSprite, TempBaseColorA, 0.7f));

		JoystickCheck = true;
	}
	void JoystickUp()
	{
		if (!JoystickCheck)
			return;

        Invoke("DownFade", 0.7f);
		JoystickCheck = false;

        MoveDir = 0;
        TempDir = 0;
	}
    void DownFade()
    {
        StartCoroutine(EndFadaIn(PadeSprite, 1F, TempPadeColorA));
        StartCoroutine(EndFadaIn(BaseSprite, 0.7f, TempBaseColorA));
    }
	void MoveLamp(float _x)
	{
        if (MoveNumber * _x < 0)
        {
            MoveDir = 1;
            if (joyStcikTutorial.activeSelf == true)
            {
                if (left_JoyStcikTutorial.activeSelf == true)
                {
                    left_JoyStcikTutorial.SetActive(false);
                }
                else if (right_JoyStcikTutorial.activeSelf == true)
                {
                    right_JoyStcikTutorial.SetActive(false);

                }
            }
        }
        else if (MoveNumber * _x > 0)
        {
            MoveDir = 2;
            if (joyStcikTutorial.activeSelf == true)
            {
                if (right_JoyStcikTutorial.activeSelf == true)
                {
                    right_JoyStcikTutorial.SetActive(false);
                }
                else if (left_JoyStcikTutorial.activeSelf == true)
                {
                    left_JoyStcikTutorial.SetActive(false);
                }
            }
        }

        //if (MoveDir != TempDir)
        //{
        //    //if(lampInfo.isJump() == false)
        //    //lamp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            
        //}

        if(lampInfo.isPosibleControl()) //2015-07-23 추가 (램프 컨트롤 가능 여부 확인)
            lamp.GetComponent<Rigidbody>().AddForce(new Vector3(MoveNumber * _x * lampInfo.speed, 0, 0), ForceMode.VelocityChange);

        TempDir = MoveDir;
	}

    //2015-07-23 추가 (조이스틱 가중치 반환)
    public float GetJoystickWeight()
    {
        joystickWeight = MoveNumber * PadeSprite.transform.localPosition.x;
        return joystickWeight;
    }

	public void SetJoystick(bool set)
	{
		JoystickIng = set;
	}
	public bool GetJoystick()
	{
		return JoystickIng;
	}
    IEnumerator StartFadaIn(UISprite sprite, float a, float end)
    {
        for (float i = a; i <= end; i += 0.03f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }

    }
	IEnumerator EndFadaIn(UISprite sprite, float a,float end)
	{
		for (float i = a; i >= end; i -= 0.03f)
		{
			Color color = new Vector4(1, 1, 1, i);
			sprite.color = color;
			yield return 0;
		}
	}
	
	
	//점프  //    //                 점프 후 바닥닿았을떄                                         
	void OnCollisionEnter(Collision other)
	{
        if (type == 1)
            return;
        if (lampInfo.isJump() == false)
            return;
        else
        {
            if (other.transform.tag == "GROUND" && other.gameObject.GetComponent<Collider>().isTrigger == false)
            {
                lampInfo.setJumpState(false);

            }
        }
	}
    void OnTriggerEnter(Collider other)
    {

        if (type == 1)
            return;
        if (lampInfo.isJump() == false)
            return;
        else
        {
            if (other.transform.tag == "GROUND" && other.gameObject.GetComponent<Collider>().isTrigger == false)
            {
                lampInfo.setJumpState(false);

            }
        }
    }
    public void SetJump(bool _set)
    {
        if (lampInfo != null)
        {
            lampInfo.setJumpState(_set);
        }
    }
    public void TJump(bool _set)
    {
        TJumpCheck = _set;
    }
    public bool GetJump()
    {
        return lampInfo.isJump();
    }
    public void OnJumpButton() // 점프 
    {
        
        if (StoryTeller.Instanace.IsOpenedStoryTeller())
            return;
        if (TJumpCheck == false)
            return;
        if (lampInfo.isJump() )
            return;

        if (CehckJump)
        {
            lamp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
            if (ChageJump)
            {              
                lamp.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, 1.0f), ForceMode.VelocityChange);
            }
            else
            {
                lamp.GetComponent<Rigidbody>().AddForce(new Vector3(0, -3f, 1.0f), ForceMode.VelocityChange);
            }


            Invoke("Z_FreezePosition", 1.0f);

        }
        else
        {
            
            //lamp.rigidbody.AddForce(Vector3.up * (lampInfo.jumpPower), ForceMode.VelocityChange);
            if (ChageJump)
                lamp.GetComponent<Rigidbody>().velocity = new
                    Vector3(lamp.GetComponent<Rigidbody>().velocity.x, (lampInfo.jumpPower), lamp.GetComponent<Rigidbody>().velocity.z);
            else
            {
                lamp.GetComponent<Rigidbody>().velocity = new
                   Vector3(lamp.GetComponent<Rigidbody>().velocity.x, (-lampInfo.jumpPower), lamp.GetComponent<Rigidbody>().velocity.z);
            }

        }
        lampInfo.setJumpState(true);
        GameObject Temp = (GameObject)Instantiate(JumpEff, new Vector3(0, 0, 0), Quaternion.identity);
        Temp.transform.position = lamp.transform.position;
        if (JumpTr.activeSelf == true)
        {
            JumpTr.gameObject.SetActive(false);
        }
    }
    void Z_FreezePosition()
    {
        lamp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
}
