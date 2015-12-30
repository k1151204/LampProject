using UnityEngine;
using System.Collections;

public class TransparentSwitch : MonoBehaviour 
{
    public GameObject[] target;

    private Shader transparent;
    private Shader diffuse;

	void Start () 
    {
        transparent = Shader.Find("Transparent/Diffuse");
        diffuse = Shader.Find("Diffuse");
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            for(int i = 0; i < target.Length; i++)
            {
                target[i].GetComponent<Renderer>().material.shader = transparent;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < target.Length; i++)
            {
                target[i].GetComponent<Renderer>().material.shader = diffuse;
            }
        }
    }
}
