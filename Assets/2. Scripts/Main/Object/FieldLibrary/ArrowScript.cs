using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

    float rotateX;
    Vector3 subVec;
    Vector3 nowPos;
    bool isHit;
    float speed;
    float minSpeed;

    public float arrowStartPower;
    public float arrowAirFriction;
    public Transform goalPos;
    public GameObject gameManager;

	void Start () {
        isHit = false;
        nowPos = transform.position;
        rotateX = 0;
        speed = arrowStartPower;
        minSpeed = 0.05f;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isHit == false)
        {
            gameManager.SendMessage("lampDie", SendMessageOptions.DontRequireReceiver);
        }
        if (collision.gameObject.GetComponent<ArrowScript>() == null)
        {
            if (IsInvoking() == false)
            {
                isHit = true;
                destroyArrow();
            }
        }
    }

    void destroyArrow()
    {
        Destroy(gameObject);
    }

	void Update () {
        if (isHit == false)
        {
            if (speed - arrowAirFriction > minSpeed)
            {
                speed -= arrowAirFriction;
            }
            else
            {
                speed = minSpeed;
            }

            //화살 회전
            transform.rotation = Quaternion.Euler(new Vector3(rotateX, transform.rotation.y, transform.rotation.z));
            rotateX = (rotateX + 40.0f) % 360;

            subVec = goalPos.position - nowPos;
            int dirSign = 1;
            if (goalPos.position.y < transform.position.y)
            {
                dirSign = -1;
            }
            if (goalPos.position.x - transform.position.x < 0)
            {
                dirSign *= -1;
            }
            transform.eulerAngles = new Vector3(-dirSign * Mathf.Acos(subVec.normalized.x) * Mathf.Rad2Deg, 0, dirSign * Mathf.Acos(subVec.normalized.x) * Mathf.Rad2Deg);

            //화살 이동
            if (nowPos == goalPos.position)
            {
                Destroy(gameObject);
            }

            float gap = subVec.sqrMagnitude;
            if (gap < speed * speed)
            {
                nowPos = goalPos.position;
            }
            else
            {
                nowPos += subVec.normalized * speed;
            }
            transform.position = nowPos;
        }
	}
}
