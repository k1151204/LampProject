using UnityEngine;
using System.Collections;

public class AnimationStartTime : MonoBehaviour
{
    public Animator Target;
    public float StartTime = 0;

    void Start()
    {
        if (Target == null)
        {
            Target = this.GetComponent<Animator>();
        }
        Invoke("AnimationStart", StartTime);
    }

    void AnimationStart()
    {
        Target.enabled = true;
    }
}