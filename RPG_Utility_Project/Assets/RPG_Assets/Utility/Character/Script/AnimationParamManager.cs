using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationParamManager : MonoBehaviour
{
    private Animator anim;
    public float num;

	// Use this for initialization
	void Start ()
	{
	    anim = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		
	}

    public int GetAnimationNum()
    {
        return (int)Mathf.Floor(num);
    }
}
