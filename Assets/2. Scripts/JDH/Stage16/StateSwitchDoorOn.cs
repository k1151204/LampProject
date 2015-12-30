using UnityEngine;
using System.Collections;

public class StateSwitchDoorOn : MonoBehaviour 
{
    public GameObject[] levers;

    public Animator door;

	void Start () 
    {

	}
	
	void Update () 
    {
        DoorOn(); 
	}

    void DoorOn()
    {
        if (levers[0].activeSelf && levers[1].activeSelf && levers[2].activeSelf && levers[3].activeSelf)
        {
            door.enabled = true;
        }
    }
}
