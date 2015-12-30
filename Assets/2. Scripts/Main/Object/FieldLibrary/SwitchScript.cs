using UnityEngine;
using System.Collections;

public enum Command { Start, Stop };
public enum SwitchTarget { Etc, MovingWall, CameraWalk, BossWalk };

public class SwitchScript : MonoBehaviour {

    /*
      스위치를 지나가면 설정된 타겟에게 작동시작, 작동중지 명령을 전달한다.
      (옵션을 통해 스위치를 한번 눌르면 계속 적용할지, 계속 누르고 있어야 적용되는지 변경)
     */

    public GameObject targetObject;
    public Command Action = Command.Stop;
    public bool PressOnlyOne = true;      //스위치에서 떨어지면 Release되는지 여부 (단 1번만 눌린다.)
    public bool OnlyPlayer = true;
    public bool ActiveSwitch = false;
    bool CommandExecuted = false;       

    GameObject normal;
    GameObject pressed;

    [HideInInspector]
    public SwitchTarget switchTarget = SwitchTarget.Etc;
    //무빙 월 관련 옵션
    [HideInInspector]
    public bool changeWhenEnter = false;
    [HideInInspector]
    public bool changeWhenExit = false;
    [HideInInspector]
    public int enterPhase = 0;
    [HideInInspector]
    public int exitPhase = 0;
    //카메라 컨트롤 관련 옵션
    [HideInInspector]
    public CameraWalkScript cameraWalkScript;
    bool findCamera = false;
    GameObject mainCamera;
    
    
    void Start()
    {
        normal = transform.FindChild("Normal").gameObject;
        pressed = transform.FindChild("Pressed").gameObject;
        ReleaseSwitch(null,true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (OnlyPlayer) {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<LampInfo>().isPosibleControl() && CommandExecuted == false)
                {
                    PressSwitch(other);
                }
            }
        }
        else if (CommandExecuted == false)
        {
            PressSwitch(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (OnlyPlayer)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<LampInfo>().isPosibleControl() && PressOnlyOne == false)
                {
                    ReleaseSwitch(other);
                }
            }
        }
        else if (PressOnlyOne == false)
        {
            ReleaseSwitch(other);
        }
    }

    void PressSwitch(Collider other)
    {
   
        normal.SetActive(false);
        pressed.SetActive(true);

        if( Action == Command.Start ){
            if (ActiveSwitch == false)
            {

                //스위치의 타겟에 따른 처리
                switch (switchTarget)
                {
                    case SwitchTarget.Etc:
                        targetObject.SendMessage("ActivateStart", SendMessageOptions.DontRequireReceiver);
                        break;
                    case SwitchTarget.MovingWall:
                        targetObject.SendMessage("ActivateStart", SendMessageOptions.DontRequireReceiver);
                        if (changeWhenEnter)
                            targetObject.SendMessage("ChangePhase", enterPhase, SendMessageOptions.DontRequireReceiver);
                        break;
                    case SwitchTarget.CameraWalk:
                        if (other.CompareTag("Player"))
                        {

                            FindCamera();
                            mainCamera.SendMessage("startCameraWalk", targetObject.GetComponent<CameraWalkScript>().cameraWalk, SendMessageOptions.DontRequireReceiver);
                        
                        }
                        break;
               
                }
            }
            else
            {
                targetObject.SetActive(true);
            }
        }
        else if( Action == Command.Stop ){
            if (ActiveSwitch == false)
            {
                //스위치의 타겟에 따른 처리

                switch (switchTarget)
                {
                    case SwitchTarget.Etc:
                        targetObject.SendMessage("ActivateStop", SendMessageOptions.DontRequireReceiver);
                        break;
                    case SwitchTarget.MovingWall:
                        if (changeWhenEnter)
                        {
                            targetObject.SendMessage("ChangePhase", enterPhase, SendMessageOptions.DontRequireReceiver);
                        }
                        else
                        {
                            targetObject.SendMessage("ActivateStop", SendMessageOptions.DontRequireReceiver);
                        }
                        break;
                    case SwitchTarget.CameraWalk:
                        if (other.CompareTag("Player"))
                        {
                        

                            FindCamera();

                          
                            mainCamera.SendMessage("stopCameraWalk", SendMessageOptions.DontRequireReceiver);
                        }
                        break;
                }
            }
            else
            {
                targetObject.SetActive(false);
            }
        }
        CommandExecuted = true;
    }
    void ReleaseSwitch(Collider other,bool isInit=false)
    {
        normal.SetActive(true);
        pressed.SetActive(false);

        if (isInit == false)
        {
            if (Action == Command.Start)
            {
                if (ActiveSwitch == false)
                {
                    //스위치의 타겟에 따른 처리
                    switch (switchTarget)
                    {
                        case SwitchTarget.Etc:
                            targetObject.SendMessage("ActivateStop", SendMessageOptions.DontRequireReceiver);
                          
                            break;
                        case SwitchTarget.MovingWall:
                            if (changeWhenExit)
                            {
                                targetObject.SendMessage("ChangePhase", exitPhase, SendMessageOptions.DontRequireReceiver);
                            }
                            else
                            {
                                targetObject.SendMessage("ActivateStop", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case SwitchTarget.CameraWalk:
                            if (other.CompareTag("Player"))
                            {
                                FindCamera();
                                mainCamera.SendMessage("stopCameraWalk", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                    }
                }
                else
                {
                    targetObject.SetActive(false);
                }
            }
            else if (Action == Command.Stop)
            {

                if (ActiveSwitch == false)
                {
                    //스위치의 타겟에 따른 처리
                    switch (switchTarget)
                    {
                        case SwitchTarget.Etc:
                            targetObject.SendMessage("ActivateStart", SendMessageOptions.DontRequireReceiver);
                            break;
                        case SwitchTarget.MovingWall:

                            if (changeWhenExit)
                            {
                                targetObject.SendMessage("ChangePhase", exitPhase, SendMessageOptions.DontRequireReceiver);
                            }
                            else
                            {
                                targetObject.SendMessage("ActivateStart", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case SwitchTarget.CameraWalk:
                            if (other.CompareTag("3"))
                            {
                                FindCamera();
                                mainCamera.SendMessage("startCameraWalk", targetObject.GetComponent<CameraWalkScript>().cameraWalk, SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                    }
                }
                else
                {
                    targetObject.SetActive(true);
                }
            }
        }
        CommandExecuted = false;
    }


    void FindCamera()
    {
        if (findCamera == false)
        {
            mainCamera = GameObject.Find("Main Camera");
            findCamera = true;
        }
    }

}
