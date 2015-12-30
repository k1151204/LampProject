using UnityEngine;
using System.Collections;

public class AnimationSwitch : MonoBehaviour
{
    public Animator target = null;
    public float startTime = 0.1f;
  
    public enum ActiveON_OFF { NONE, ON, OFF }
    public ActiveON_OFF active = ActiveON_OFF.NONE;

    public GameObject[] activeTarget;
    public float activeTime = 0.1f;

    public enum AnimationChange { NONE, ON, OFF}
    public AnimationChange changeState = AnimationChange.NONE;

    public enum SwitchOnOffMode { NONE, PLAYER, LIGHTBOOM}
    public SwitchOnOffMode switchMode = SwitchOnOffMode.NONE;

    public enum SoundList
    {
        None,
        Lever
    }
    public SoundList _soundList = SoundList.None; 



    void Start()
    {
        if (changeState == AnimationChange.ON)
        {
            target.enabled = true;
            target.SetBool("lever_On", true);
        }

        if (changeState == AnimationChange.OFF)
        {
            target.enabled = true;
            target.SetBool("lever_On", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (switchMode == SwitchOnOffMode.NONE)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("LightBoom"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        else if(switchMode == SwitchOnOffMode.PLAYER)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        else if(switchMode == SwitchOnOffMode.LIGHTBOOM)
        {
            if (other.gameObject.CompareTag("LightBoom"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (switchMode == SwitchOnOffMode.NONE)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("LightBoom"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (switchMode == SwitchOnOffMode.PLAYER)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (switchMode == SwitchOnOffMode.LIGHTBOOM)
        {
            if (other.gameObject.CompareTag("LightBoom"))
            {
                if (active == ActiveON_OFF.ON || active == ActiveON_OFF.OFF)
                {
                    Invoke("ObjectActive", activeTime);
                }
                if (target != null)
                {
                    Invoke("AnimationStart", startTime);
                    if (changeState != AnimationChange.NONE)
                    {
                        if (target.GetBool("lever_On") == true)
                        {
                            target.SetBool("lever_On", false);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(false);
                                    activeTarget[1].SetActive(true);

                                }
                            }
                        }

                        else if (target.GetBool("lever_On") == false)
                        {
                            target.SetBool("lever_On", true);
                            for (int i = 0; i < activeTarget.Length; i++)
                            {
                                if (!activeTarget[i] != null)
                                {
                                    activeTarget[0].SetActive(true);
                                    activeTarget[1].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void AnimationStart()
    {
        target.enabled = true;
        //SoundManager.manager.playSound(SoundIndex.Lever);
    }

    void ObjectActive()
    {
        for (int i = 0; i < activeTarget.Length; i++)
        {
            if (active == ActiveON_OFF.ON)
            {
                activeTarget[i].SetActive(true);
            }
            else if (active == ActiveON_OFF.OFF)
            {
                activeTarget[i].SetActive(false);
            }
        }
    }

}

