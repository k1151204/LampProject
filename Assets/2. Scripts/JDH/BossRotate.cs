using UnityEngine;
using System.Collections;

public class BossRotate : MonoBehaviour
{

    public float xRot, yRot, zRot;

    void Start()
    {

    }

    void Update()
    {

        transform.Rotate(xRot, yRot, zRot);
    }
}
