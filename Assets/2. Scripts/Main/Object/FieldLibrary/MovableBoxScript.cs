using UnityEngine;
using System.Collections;

public class MovableBoxScript : MonoBehaviour {

    public bool SupportForce = false;
    public float baseMass = 0.03f;
    public float supportedMass = 0.1f;
    GameObject _light;
    public Rigidbody targetRig = null;

	void Start () {
        _light = transform.FindChild("Light").gameObject;
        _light.SetActive(false);
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _light.SetActive(true);
            if (SupportForce)
            {
                GetComponent<Rigidbody>().mass = supportedMass;
                targetRig = collision.gameObject.GetComponent<Rigidbody>();
            }
            else
            {
                GetComponent<Rigidbody>().mass = baseMass;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (targetRig != null)
        {
            int sign = 1;
            if (targetRig.velocity.x < 0)
                sign = -1;

            GetComponent<Rigidbody>().velocity += Vector3.right * sign * 0.02f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _light.SetActive(false);
            targetRig = null;
        }
    }
}
