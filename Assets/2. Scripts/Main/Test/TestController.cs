using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour 
{
    LampInfo lampInfo;
    public GameObject RightMove;
    public GameObject LeftMove;

    void Start()
    {
        lampInfo = gameObject.GetComponent<LampInfo>();
    }
	
	void Update () 
	{
        if (lampInfo.isPosibleControl())
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(moveHorizontal, 0, 0);
            GetComponent<Rigidbody>().AddForce(movement * (lampInfo.speed*1166) * Time.deltaTime);
        }
	}
}