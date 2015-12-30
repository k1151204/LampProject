using UnityEngine;
using System.Collections;

public class PotHelpWindow : MonoBehaviour {
    GameObject AttackHelp;
    UIController BossUI;
	// Use this for initialization
	void Start () {
        AttackHelp = GameObject.Find("PotHelp").transform.FindChild("AttackHelp").gameObject;
      
        AttackHelp.SetActive(false);
        BossUI = GameObject.Find("UIController").GetComponent<UIController>();
        BossUI.BossHpReSetting();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetHpBar(int _power)
    {
        BossUI.BossHpDownSet(_power);
    }
    public void Off()
    {
        AttackHelp.SetActive(false);
        BossUI.BossHpEnd();

    }
    public void OnHpBar()
    {
        BossUI.BossHpStart();


    }
    public void OffHpBar()
    {
        BossUI.BossHpEnd();
    }
    public void OnAttackHelp()
    {
        AttackHelp.SetActive(true);
        StartCoroutine(StartEffectText(AttackHelp));
    }
    public void OffAttackHelp()
    {
        StopCoroutine(StartEffectText(AttackHelp));
        StopCoroutine(EndEffectText(AttackHelp));
        AttackHelp.SetActive(false);
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
    IEnumerator StartEffectText(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();
        for (float i = 0.4f; i <= 1; i += 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;
            yield return 0;
        }
        StartCoroutine(EndEffectText(sprite));
    }
    IEnumerator EndEffectText(GameObject sprite)
    {
        UILabel colorspirte = sprite.GetComponent<UILabel>();
        for (float i = 1f; i >= 0.4f; i -= 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            colorspirte.color = color;
            yield return 0;
        }
        StartCoroutine(StartEffectText(sprite));
    }
}
