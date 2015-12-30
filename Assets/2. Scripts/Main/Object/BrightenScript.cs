using UnityEngine;
using System.Collections;

public class BrightenScript : MonoBehaviour
{
    GameObject lamp;
    public GameObject effect;
    bool isEnable;

    LampInfo lampInfo;

    // Use this for initialization
    void Start()
    {
        lamp = GameObject.Find("Lamp");
        lampInfo = lamp.GetComponent<LampInfo>();
        DisableBrighten();
        isEnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lamp.transform.position;
        if (lampInfo.getLight() > 0)
        {
            if (!isEnable)
                EnableBrighten();
        }
        else
        {
            DisableBrighten();
        }
    }

    public void EnableBrighten()
    {
        effect.SetActive(true);
        isEnable = true;
    }
    public void DisableBrighten()
    {
        effect.SetActive(false);
        isEnable = false;
    }
    public bool isEnabled()
    {
        return isEnable;
    }

    public void BrightenOn()
    {
        effect.GetComponent<Collider>().enabled = true;
    }
}
