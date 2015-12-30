using UnityEngine;
using System.Collections;

public class BossUI : MonoBehaviour {
    Boss3Motion BossMotion;
    UIController Boss;
	// Use this for initialization
	void Start () {
        Boss = GameObject.Find("UIController").GetComponent<UIController>();
        BossMotion = GetComponent<Boss3Motion>();
        Boss.BossHpReSetting();
	}
    public void StartHpBar()
    {
        Boss.BossHpStart();
    }
    public void EndHpBar()
    {
        Boss.BossHpEnd();
    }
    public void ShowHpBar(int _hpBar)
    {
        Boss.BossHpDownSet(_hpBar);

    }
    public void off()
    {
        Boss.BossHpEnd();
    }
    
	// Update is called once per frame
	void Update () {
	
	}
    
    IEnumerator StartEffectText(UISprite sprite)
    {

        for (float i = 0.4f; i <= 1; i += 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(EndEffectText(sprite));
    }
    IEnumerator EndEffectText(UISprite sprite)
    {
        for (float i = 1f; i >= 0.4f; i -= 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            sprite.color = color;
            yield return 0;
        }
        StartCoroutine(StartEffectText(sprite));
    }
}
