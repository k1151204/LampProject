using UnityEngine;
using System.Collections;

public class LightStoneScript : MonoBehaviour {
    public GameObject pointLight;
    public GameObject effect;
    bool isLightOn = false;

    void OnTriggerEnter(Collider collider)
    {
        if (isLightOn == false)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                if (collider.gameObject.GetComponent<LampInfo>().getLight() > 0)
                {
                    pointLight.SetActive(true);
                    effect.SetActive(true);
                    isLightOn = true;
                }
            }
        }
    }
}
