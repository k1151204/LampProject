using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    GameObject lamp;
    LampInfo lampInfo;
    GameObject uiController;
    
    static bool jumpTrg = false;
    Rigidbody lampRig;

    // Use this for initialization
    void Start()
    {
        lamp = GameObject.Find("Lamp");
        lampInfo = lamp.GetComponent<LampInfo>();
        uiController = GameObject.Find("UIController");
        
        lampRig = lamp.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //뒤로가기 누르면 메뉴 활성화
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    uiController.SendMessage("onoffMenu");
        //}

        //if (lampInfo.isJump())
        //{
        //    if ((-0.000001f <= lampRig.velocity.y && lampRig.velocity.y <= 0.000001f))
        //    {
        //        lampInfo.setJumpState(false);
        //    }
        //}
    }

    //void rightMoveLamp()
    //{
    //    if (lampInfo.isPosibleControl())
    //        lamp.rigidbody.AddForce(Vector3.right * lampInfo.speed * Time.deltaTime);
    //}
    //void leftMoveLamp()
    //{
    //    if (lampInfo.isPosibleControl())
    //        lamp.rigidbody.AddForce(Vector3.left * lampInfo.speed * Time.deltaTime);
    //}

    //void jumpLamp()
    //{
    //    if (lampInfo.isPosibleControl())
    //    {
    //        if ( !lampInfo.isJump() || jumpTrg)
    //        {
    //            lamp.rigidbody.AddForce(Vector3.up * lampInfo.jumpPower, ForceMode.VelocityChange);
    //            lampInfo.setJumpState(true);
    //            jumpTrg = false;
    //        }
    //    }
    //}

    static public void activeJumpTrg() { jumpTrg = true; }

}