using UnityEngine;
using System.Collections;


public class CamareNewView : MonoBehaviour {
    public GameObject Tager;//이동 타겟
	public GameObject MoveTager; //이동 타켓
    public float MoveTime =1f;
    public float RotaionSpeed =1F;
    bool Check;
    CameraController Camare;
	// Use this for initialization
	void Start () {
		if (MoveTager == null)
            MoveTager = GameObject.Find("Brighten");
        Check = false;
        Camare = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}
	
	// Update is called once per frame
    public void CollSetting(GameObject _Tager, GameObject _MoveTager, float _MoveTime, float _RotaionSpeed)
    {
        Camare.Setting(_Tager, _MoveTager, _MoveTime, _RotaionSpeed);
        Camare.StartCamaer();
    }
    void OnCollisionEnter(Collision other)
    {
        if (Check == true)
            return;
        if (other.collider.tag == "Player" )
        {
            Check = true;
            CollSetting(Tager, MoveTager, MoveTime, RotaionSpeed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Check == true)
            return;
        if (other.tag == "Player" )
        {
            Check = true;
            CollSetting(Tager, MoveTager, MoveTime, RotaionSpeed);
        }
    }
}
