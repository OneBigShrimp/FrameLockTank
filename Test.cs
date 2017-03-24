using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Director;

public class Test : MonoBehaviour
{
    public Animator Anim;

    public AnimationClip Clip1;

    public AnimationClip Clip2;

    public AnimationClip Clip3;

    public AnimationClip Clip4;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(Clip1);
            Anim.Play(clipPlayable);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(Clip2);
            Anim.Play(clipPlayable);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(Clip3);
            Anim.Play(clipPlayable);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(Clip4);
            Anim.Play(clipPlayable);
        }
    }

    IEnumerator DelayCall()
    {
        yield return new WaitForEndOfFrame();
        //Anim.CrossFade("Atk2", Duration, 0, 0);
    }


    void LateUpdate()
    {


    }
}


