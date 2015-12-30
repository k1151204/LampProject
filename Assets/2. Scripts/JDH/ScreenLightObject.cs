using UnityEngine;
using System.Collections;

public class ScreenLightObject : MonoBehaviour
{
    Collider collider;
    Material material;
    public float HideNumber = 0.1f;
    public float CreateNumber = 0.1f;
    int ChageCheck;
    int WhatType;
    int Count;
    float time;
    Coroutine EndCor;
    string Tag;
    // Use this for initialization
    void Start()
    {
        collider = GetComponent<Collider>();
        material = GetComponent<Renderer>().material;
        if (collider != null)
        {
            material.color = new Vector4(material.color.r, material.color.g, material.color.b, 0);
        }
        collider.isTrigger = true;
        ChageCheck = 0;
        WhatType = 0;
        EndCor = null;
        Tag = tag;
            tag = "Untagged";
        Count = 0;
    }
    void Update()
    {
        if(WhatType == 1)
        {
            time += Time.deltaTime;
            if(time >= 1)
            {
                if (Count == 1)
                {
                 
                    HideObject();
                }
                else
                {
                 
                    Count--;
                }
                time = 0;
            }
        }
    }
    // Update is called once per frame
    public int GetCheck()
    {
        return ChageCheck;
    }
    void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "LightObject")
        {
            if (WhatType == 0)
            {
                collider.isTrigger = false;
            
                if(Count == 0)
                StartCoroutine(CreateToObject());
                ChageCheck = 1;
                WhatType = 1;
                Count++;
            }
            else if (other.collider.tag == "TestLight")
            {
                if (EndCor != null)
                    StopCoroutine(EndCor);
                collider.isTrigger = false;
                if (Count == 0)
                StartCoroutine(CreateToObject());
                ChageCheck = 1;
                WhatType = 2;
                Count++;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
    

        if (other.tag == "LightObject")
        {
            if (WhatType == 0)
            {
                collider.isTrigger = false;
                if (Count == 0)
                StartCoroutine(CreateToObject());
                ChageCheck = 1;
                WhatType = 1;
                Count++;
            }
        }
        else if (other.tag == "TestLight")
        {


            if (EndCor != null)
            StopCoroutine(EndCor);
            collider.isTrigger = false;
            if (Count == 0)
            StartCoroutine(CreateToObject());
            ChageCheck = 1;
            WhatType = 2;
            Count++;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TestLight")
        {
            if (Count == 1)
                HideObject();
            else
                Count--;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "TestLight")
        {
            if (Count == 1)
                HideObject();
            else
                Count--;

        }
    }
    void HideObject()
    {
       EndCor =  StartCoroutine(HideToObject());
    }
    IEnumerator CreateToObject()
    {
        tag = Tag;

        for (float i = 0; i <= 1f; i += CreateNumber)
        {
            Color color = new Vector4(material.color.r, material.color.g, material.color.b, i);
            material.color = color;
            yield return 0;
        }
    }
    IEnumerator HideToObject()
    {

        ChageCheck = 0;
        WhatType = 0;
        Count--;
        for (float i = 1; i >= 0f; i -= HideNumber)
        {
            Color color = new Vector4(material.color.r, material.color.g, material.color.b, i);
            material.color = color;
            yield return 0;
        }
        material.color = new Vector4(material.color.r, material.color.g, material.color.b, 0);
        tag = "Untagged";
        collider.isTrigger = true;
        

    }
}

