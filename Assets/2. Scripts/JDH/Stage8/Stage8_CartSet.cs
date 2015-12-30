using UnityEngine;
using System.Collections;

public class Stage8_CartSet : MonoBehaviour
{
    GameObject LightButtInfo;
    CameraController Camare;
    PathMove PathMoveTemp;

    public bool IsStage8 = true;
    void Start()
    {
        LightButtInfo = GameObject.Find("UIController");
        Camare = GameObject.Find("Main Camera").GetComponent<CameraController>();
        if (IsStage8 == true)
        {
            PathMoveTemp = GameObject.Find("PathMove1").GetComponent<PathMove>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LightButtInfo.GetComponent<LightButt>().BulletZ = 15.0f;
            LightButtInfo.GetComponent<LightButt>().SetMoveBoomSpeed(20.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LightButtInfo.GetComponent<LightButt>().ReSetting();

            Camare.ReSetCamaer();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LightButtInfo.GetComponent<LightButt>().BulletZ = 15.0f;
            LightButtInfo.GetComponent<LightButt>().SetMoveBoomSpeed(20.0f);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LightButtInfo.GetComponent<LightButt>().ReSetting();
            Camare.ReSetCamaer();
        }
    }

}
