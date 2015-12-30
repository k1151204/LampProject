using UnityEngine;
using System.Collections;

public class AttachLampOnWall : MonoBehaviour {
    public GameObject Path = null;
    ArrayList attachedObj = new ArrayList();
    public bool JumpCheck = true;
    bool CollCheck = false; // 한번만 충돌을위해
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject colObj = collision.gameObject;
            colObj.transform.parent = transform.parent;

            if (Path != null)
            {

                if (!CollCheck)
                {
                    Path.GetComponent<PathMove>().StartMove();
                    GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(JumpCheck);
                }
                CollCheck = true;
            }
            attachedObj.Add(colObj);
            ZaxisCorrection();
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
            if (attachedObj.Contains(collision.gameObject))
            {
                attachedObj.Remove(collision.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject colObj = collision.gameObject;
            colObj.transform.parent = transform.parent;

            if (Path != null)
            {

                if (!CollCheck)
                {
                    Path.GetComponent<PathMove>().StartMove();
                    GameObject.Find("Lamp").GetComponent<LampMoveManager>().SetJump(JumpCheck);
                }
                CollCheck = true;
            }
            attachedObj.Add(colObj);
            ZaxisCorrection();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
            if (attachedObj.Contains(collision.gameObject))
            {
                attachedObj.Remove(collision.gameObject);
            }
        }
    }

    public void ReMove(Collider TEST)
    {
        TEST.gameObject.transform.parent = null;
        if (attachedObj.Contains(TEST.gameObject))
        {
            attachedObj.Remove(TEST.gameObject);
        }
    }
    void ZaxisCorrection()
    {
        GameObject obj;
        for (int i = 0; i < attachedObj.Count; ++i)
        {
            obj = (GameObject)attachedObj[i];
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z);
        }
    }


}
