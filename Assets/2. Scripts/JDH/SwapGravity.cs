using UnityEngine;
using System.Collections;

public class SwapGravity : MonoBehaviour {
    public float ChageGravityNumber = -9.81f;
    public float LampJumpNumber = 3.5f;
    public bool DestryCheck = false;

    GameObject Lamp;
    LampMoveManager LampMove;
    LampInfo LampInfo;
    static Vector3 NomalGravity = new Vector3(0,-9.81f,0);
	// Use this for initialization
    public static Vector3 GetNoamlGravity()
    {
        return NomalGravity;
    }
	void Start () {
        Lamp = GameObject.Find("Lamp");
        LampMove = Lamp.GetComponent<LampMoveManager>();
        LampInfo = Lamp.GetComponent<LampInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "LightBoom")
        {
            ChageGravity();

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "LightBoom")
        {
            ChageGravity();

        }
    }
    void ChageGravity()
    {
        if (new Vector3(0, ChageGravityNumber, 0) == Physics.gravity)
        {
            return;
        }
        Physics.gravity = new Vector3(0, ChageGravityNumber, 0);
        if (ChageGravityNumber > 0)
        {
            LampMove.SetChageJump(false);
        }
        else
        {
            LampMove.SetChageJump(true);
        }
        LampInfo.SetJumpPower(LampJumpNumber);
        if (DestryCheck)
            DestroyObject(this.gameObject);
    }
}
