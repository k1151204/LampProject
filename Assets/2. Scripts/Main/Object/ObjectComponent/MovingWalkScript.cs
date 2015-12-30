using UnityEngine;
using System.Collections;

public class MovingWalkScript : MonoBehaviour
{

    public enum MoveDir { Right, Left, Front, Back };
    public MoveDir moveDir;
    public float speed;
    Vector3 dirVec;

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag != "Player")
            return;
        if (moveDir == MoveDir.Right)
        {
            dirVec = Vector3.right;
        }
        else if (moveDir == MoveDir.Left)
        {
            dirVec = Vector3.left;
        }
        else if (moveDir == MoveDir.Front)
        {
            dirVec = Vector3.forward;
        }
        else if (moveDir == MoveDir.Back)
        {
            dirVec = Vector3.back;
        }
        other.transform.position += dirVec * speed * Time.deltaTime;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Player")
            return;
        if (moveDir == MoveDir.Right)
        {
            dirVec = Vector3.right;
        }
        else if (moveDir == MoveDir.Left)
        {
            dirVec = Vector3.left;
        }
        else if (moveDir == MoveDir.Front)
        {
            dirVec = Vector3.forward;
        }
        else if (moveDir == MoveDir.Back)
        {
            dirVec = Vector3.back;
        }
        other.transform.position += dirVec * speed * Time.deltaTime;

    }



}

