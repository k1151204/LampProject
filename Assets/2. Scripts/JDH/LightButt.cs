using UnityEngine;
using System.Collections;

public class LightButt : MonoBehaviour {
    public float BulletZ = 0; //  기본값 0으로 하시면 원래 원상복귀됩니다.
     GameObject LightEff;
    GameObject Temp;
    GameObject Lamp;

    Transform Cam;
    float MoveBoomSpeed = 0;
    public void SetMoveBoomSpeed(float _speed)
    {
        MoveBoomSpeed = _speed;
    }
    public float GetMoveBoomSpeed()
    {
        return MoveBoomSpeed;
    }
    public void ReSetting()
    {
        BulletZ = 0;
        MoveBoomSpeed = 0.0f;
    }
    // Use this for initialization
   
	void Start () {
        LightEff = LampCharChage.GetData.GetEff((int)LampChar.BULLET);
        Lamp = GameObject.Find("Lamp");
        Cam = GameObject.Find("Main Camera").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    }
    public void CreateLight(float _power)
    {
        if (Cam.gameObject.GetComponent<CameraController>().GetMoveCheck() == false)
        {
            if (BulletZ == 0)
                BulletZ = -(Cam.position.z - (Lamp.transform.position.z));
            Temp = (GameObject)Instantiate(LightEff, Lamp.transform.position, Quaternion.identity);
            Vector2 mousePos = UICamera.currentTouch.pos;
            Temp.GetComponent<LightMoveBoom>().SetVector(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, BulletZ)));
            Temp.GetComponent<LightMoveBoom>().Create();
        }
    }

}
