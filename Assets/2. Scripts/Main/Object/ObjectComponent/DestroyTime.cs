using UnityEngine;
using System.Collections;

public class DestroyTime : MonoBehaviour {
    public float time = 5.0f;
    public bool activateOnAwake = true;

	void Start () {
        if (activateOnAwake)
        {
            Invoke("DestroySelf", time);
        }
	}

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void ActivateStart()
    {
        Invoke("DestroySelf", time);
    }
    void ActivateStop()
    {
        CancelInvoke();
    }
}
