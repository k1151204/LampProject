using UnityEngine;
using System.Collections;

public class ExplosionTrap : MonoBehaviour
{
    public GameObject explosionEffect;
    public float StandTime = 3.0f;
    public float ExplosionDurationTime = 0.5f;
    public float StartDelay = 0;
    public bool ActivateOnAwake = true;
    public bool use3DSound = true;

    enum TrapState { STAND, FIRE };
    TrapState trapState = TrapState.STAND;
    GameObject gameManager;
    float rotateY;
    bool setRotate;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        if (gameManager.GetComponent<GameManager>().isEnableSE() == false || use3DSound == false)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }

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
        //setRotate가 활성화되면 오브젝트가 회전한다.
        if (setRotate)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, rotateY, 0));
            rotateY = (rotateY + 15.0f) % 360;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //트랩이 Fire상태일 때 플레이어가 올라오면 게임오버 처리
        if (trapState == TrapState.FIRE)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void CreateExplosionEffect()
    {
        //이펙트 생성
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity) as GameObject;
        effect.transform.parent = transform;
        //사운드 재생
        GetComponent<AudioSource>().Play();
        //상태 변화
        trapState = TrapState.FIRE;
        Invoke("SetStand", ExplosionDurationTime);
        EnableRotate();
        Invoke("DisableRotate", 1.5f);
    }
    void SetStand()
    {
        trapState = TrapState.STAND;
        Invoke("CreateExplosionEffect", StandTime);
    }

    void EnableRotate() { setRotate = true; }
    void DisableRotate() { setRotate = false; }

    public void ActivateStart()
    {
        Invoke("CreateExplosionEffect", StartDelay);
    }
    public void ActivateStop()
    {
        CancelInvoke();
    }
}