using UnityEngine;
using System.Collections;

public class pressButtonUpdate : MonoBehaviour {

    //해당 버튼이 press되면 지속적으로 Target의 함수를 실행한다.
    public GameObject target;
    public string functionName;
    public UIButton button;
	
	// Update is called once per frame
	void Update () {
        if (button.state == UIButton.State.Pressed)
        {
            target.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
        }
	}
}
