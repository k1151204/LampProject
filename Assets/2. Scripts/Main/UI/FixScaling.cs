using UnityEngine;
using System.Collections;

public class FixScaling : MonoBehaviour {
    public float bestWidthSize = 560;
    public float bestHeightSize = 840;

    //public ScreenOrientation orientation; 

	void Awake () {
        //스크린 모드 설정
        //Screen.orientation = orientation;

        gameObject.GetComponent<Camera>().orthographicSize = (Screen.height * bestWidthSize / bestHeightSize) / Screen.width;
	}
	
}
