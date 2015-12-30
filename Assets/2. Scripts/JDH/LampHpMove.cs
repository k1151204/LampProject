using UnityEngine;
using System.Collections;

public class LampHpMove : MonoBehaviour {
    float speed;
    GameObject lamp;
    bool Movecheck;
    float Rot;
    float RotTemp;

	// Use this for initialization
    public void SetSpeed(float _set)
    {
        speed = _set;
    }
    public void SetRot(float _set)
    {
        Rot = _set;
    }
    public void SetMoveCheck(bool _set)
    {
        Movecheck = _set;
    }
    void Start()
    {
        RotTemp = 0;
        Movecheck = true;
        lamp = GameObject.Find("Lamp");

        this.transform.localPosition = new Vector3((lamp.transform.position.x) + Random.Range(-0.3f, 0.3f), (lamp.transform.position.y), lamp.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (Movecheck)
        {
            iTween.MoveUpdate(gameObject, iTween.Hash("position", lamp.transform.position, "time", speed));

        }
        else
        {

            transform.RotateAround(lamp.transform.position, Vector3.up, Rot);
            transform.LookAt(lamp.transform.position);
            //RotTemp += Rot;
            //float distance;
            //if (lamp.transform.parent == null)
            //    distance = Vector3.Distance(lamp.transform.localPosition, this.transform.localPosition); // 기준점과의 거리 저장
            //else
            //    distance = Vector3.Distance(lamp.transform.localPosition + lamp.transform.parent.localPosition, this.transform.localPosition); // 기준점과의 거리 저장

            //Quaternion quat = Quaternion.Euler(new Vector3(0, (RotTemp), 0));
            //transform.rotation = quat;
            //if (lamp.transform.parent == null)
            //transform.position = lamp.transform.localPosition;
            //else
            //    transform.position = lamp.transform.localPosition + lamp.transform.parent.localPosition;

            //transform.position -= quat * Vector3.forward * distance;
            //transform.LookAt(lamp.transform.localPosition);
        }

    }
 
    void OnTriggerEnter(Collider other)
    {
   
        if (lamp != null)
        {

            if (other.tag == lamp.transform.tag)
            {

                Movecheck = false;
                transform.rotation = Quaternion.identity;
                if (transform.position.x < lamp.transform.position.x)
                {

                    transform.position = new Vector3((lamp.transform.position.x) - 0.13f, lamp.transform.position.y, lamp.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3((lamp.transform.position.x) + 0.13f, lamp.transform.position.y, lamp.transform.position.z);

                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (lamp != null)
        {
            if (other.tag == lamp.transform.tag)
            {
                Movecheck = true;
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
