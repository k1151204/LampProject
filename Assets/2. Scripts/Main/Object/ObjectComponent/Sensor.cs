using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {
    public GameObject sendTarget = null;
    public bool isDestroy = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.parent != null)
            {
                sendTarget.SendMessage("EnterPlayer", SendMessageOptions.DontRequireReceiver);
                if (isDestroy)
                    Destroy(gameObject);
            }
        }
    }
}
