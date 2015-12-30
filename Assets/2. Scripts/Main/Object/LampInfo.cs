using UnityEngine;
using System.Collections;

public class LampInfo : MonoBehaviour {

    public float speed;
    public float jumpPower;
    float totalLight;
    bool isControl;
    bool iJump; // true 점프불가 false 점프가능상태
    bool DieCheck;
	// Use this for initialization
    void Start()
    {
        isControl = false;
        iJump = false;
        DieCheck = false;
    }
    public void SetDieCheck(bool _set)
    {
        DieCheck = true;
    }
    public bool GetDieCheck()
    {
        return DieCheck;
    }
    public bool isPosibleControl()
    {
        return isControl;
    }
    public void setControl(bool set) { isControl = set; }
    public void restoreLight() { totalLight = 100; GameObject.Find("UIController").GetComponent<UIController>().ReSet(); }
    public bool setLight(float set) {
        if (totalLight < 5) return false;
        totalLight += set;
        return true;
    }
    public void addLight(float add) { 
        totalLight += add;
        if (totalLight > 100) totalLight = 100;
        else if (totalLight <= 0) totalLight = 0;
    }
    public float getLight() { return totalLight; }
    public bool isJump() { return iJump; }
    public void setJumpState(bool set) { iJump = set; }
    public float GetJumpPower()
    {
        return jumpPower;
    }
    public void SetJumpPower(float _set)
    {
        jumpPower = _set;
    }
}
