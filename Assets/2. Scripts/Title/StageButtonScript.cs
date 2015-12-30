using UnityEngine;
using System.Collections;
using System.Text;

public class StageButtonScript : MonoBehaviour 
{
    public GameObject LoadingObject;
    public GameObject RandomFunction;
    public UIPanel[] UIs;
    public Camera Camera;
    public UISprite Fade;
    public GameObject _Loading;
    public  int NowStage = 0;
    bool Check = true;
    Coroutine Temp;
    bool CheckColl = false;
    public bool Open = true;
    bool Touch = false;
    public AudioClip stageButtonSound;// 2015-09-10 새로운 사운드 매니저 추가
  
    public void Stop()
    {
        if (Temp != null)
        {
            StopCoroutine(Temp);
            Temp = null;
        }
    }
    void OnClick()
    {
        if (LoadingObject.activeSelf)
            return;
        if (Touch == true)
            return;
        if (LampLifeManager.GetNowLieft() <= 0)
            return;
        LampLifeManager.SetNowCount(LampLifeManager.GetNowLieft() - 1);
        LampLifeManager.SaveData();
        NewSoundMgr.instance.PlaySingle(stageButtonSound);// 2015-09-10 새로운 사운드 매니저 추가
        Go();
        Touch = true;
    }
    public void Go()
    {
        
        StringBuilder sb = new StringBuilder();
        sb.Append(gameObject.name);
        sb.Remove(0, 11);
        SettingValue.GetData.startStage = int.Parse(sb.ToString());
        //StartCoroutine(ClearLight(Camera.GetComponent<Camera>()));
        //StartCoroutine(ClearBack(Fade));
        StartCoroutine(LodingState());
    }
    public void NoOpen()
    {
        if(this.gameObject.activeSelf == false)
        Open = false;

    }
    public void Setting()
    {
        if (Open == false)
            return;
        Check = false;
        Touch = false;
        GetComponent<BoxCollider>().enabled = false;
        CheckColl = false;
        transform.GetChild(0).gameObject.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
    }
    public void SettingOk()
    {
        if (Open == false)
            return;
        Check = true;
        Touch = false;
        GetComponent<BoxCollider>().enabled = true;
        CheckColl = true;
        transform.GetChild(0).gameObject.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);

    }
    public void NextPadeClearButton()
    {
        if (Open == false)
            return;
        UISprite _sprite = transform.GetChild(0).gameObject.GetComponent<UISprite>();
        if (Check == true)
            Temp = StartCoroutine(ClearButton(_sprite, 1));
        else
            Temp = StartCoroutine(ClearButton(_sprite, 0.1f));

    }
    public void NextPadeReButton()
    {
        if (Open == false)
            return;
        UISprite _sprite = transform.GetChild(0).gameObject.GetComponent<UISprite>();
        if (Check == true)
            Temp =  StartCoroutine(ReButton(_sprite, 1));
        else
            Temp =  StartCoroutine(ReButton(_sprite, 0.1f));

    }
    //IEnumerable StartMain()
    //{
    //    LightDataManager.SetNowStage(NowStage);
    //    AsyncOperation async = Application.LoadLevelAsync("Main");
    //    yield return async;
    //    //Application.LoadLevelAsync("Main");
    //}
    IEnumerator ClearButton(UISprite sprite,float _max)
    {
        GetComponent<BoxCollider>().enabled = false;
        for (float i = _max; i > 0; i -= 0.05f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0);

    }
    IEnumerator ReButton(UISprite sprite, float _max)
    {
        for (float i = 0; i < _max; i += 0.05f)
        {
            Color color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, i);
            sprite.color = color;
            yield return 0;
        }
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, _max);
        GetComponent<BoxCollider>().enabled = CheckColl;

    }
    IEnumerator ClearBack(UISprite sprite)
    {
        for (float i = 0; i <= 1f; i += 0.055f)
        {
            Color color = new Vector4(i, i, i, i);
            sprite.color = color;
            yield return 0;
        }
        //StartMain();
        LodingState();

    }
    IEnumerator ClearLight(Camera sprite)
    {
        for (float i = 0; i <= 1f; i += 0.055f)
        {
            Color color = new Vector4(i, i, i, sprite.backgroundColor.a);
            sprite.backgroundColor = color;
            yield return 0;
        }

    }

    IEnumerator LodingState()
    {
     
        LoadingObject.SetActive(true);
        RandomFunction.GetComponent<LoadingTipRandom>().SendMessage("LodaingTipTextRandomStart", SendMessageOptions.DontRequireReceiver);
        //Invoke("StartMain", 4.0f);
        LightDataManager.SetNowStage(NowStage);
        AsyncOperation async = Application.LoadLevelAsync("Main");
        while (true)
        {
            if (async.progress >= 0.7F )
            {
                async.allowSceneActivation = false;
                break;
            }
        }
        yield return new WaitForSeconds(3.0F);
        async.allowSceneActivation = true;
    
    }
   
}
