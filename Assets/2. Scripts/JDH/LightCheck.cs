using UnityEngine;
using System.Collections;

public class LightCheck : MonoBehaviour {
    
    public GameObject[] Light;
    Light LampLight;
    UILabel CountText;
    int Count;
    int MaxIndex;
	// Use this for initialization
    public void ReSetting()
    {
        MaxIndex = 0;
    }
    public int SetMaxIndex()
    {
        return MaxIndex;
    }

    public int GetLightCount() // 현재 킨 라이트닝 갯수얻어오기
    {
        int max = 0;

        for (int i = 0; i < Light.Length; i++)
        {
            if (Light[i].activeSelf)
            {
                max++;
            }
        }
        return max;
    }
    void Start()
    {
        LampLight = GameObject.Find("Lamp").transform.FindChild("light").gameObject.GetComponent<Light>();
        
            Count = 0;
            MaxIndex = 0;
            CountText = GameObject.Find("LampHelp").transform.FindChild("LampCount").GetComponent<UILabel>();
            for (int i = 0; i < Light.Length; i++)
            {
                Count++;
                if (LightDataManager.GetLightIndex(i) == 1)
                {
                    Setting(Light[i]);
                    Light[i].SetActive(true);
                    Count--;  //현재 꺼야할 갯수
                    if (LightDataManager.GetNowLightCount() < i)
                        LightDataManager.SetNowLightCount(i);
                }
            }
            OnText();
	}
    public void Setting(GameObject Light)
    {
        Light.tag = "TestLight";
        Light.transform.parent.gameObject.GetComponent<SwitchAbsorption>().BullCheck = true;
        Light.GetComponent<Light>().color = LampLight.color;
        Light.GetComponent<Light>().intensity = 8;
        Light.GetComponent<Light>().range = 5;
        Light.GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;
        Light.AddComponent<SphereCollider>();
        Light.AddComponent<Rigidbody>();
        Light.GetComponent<SphereCollider>().radius = 1.3f;
        Light.GetComponent<SphereCollider>().isTrigger = true;
        Light.GetComponent<Rigidbody>().isKinematic = false;
        Light.GetComponent<Rigidbody>().useGravity = false;
    }
    // Update is called once per frame
    void Update()
    {
	}
    public void OnText()
    {
        CountText.text = Count.ToString() + " / " + Light.Length.ToString();
    }
    public string GetText()
    {
        return CountText.text;
    }
    public void OnCheck()
    {
		
						for (int i = 0; i < Light.Length; i++) {
								if (Light [i].activeSelf) {
										if (LightDataManager.GetLightIndex (i) != 1) {
												LightDataManager.SetLightIndex (i, 1);
												LightDataManager.SetLight (LightDataManager.GetNowStage (), LightDataManager.GetLight () + 1);
												MaxIndex = i;
												Count--;
												OnText ();
										}
								}
						}
    }
}
