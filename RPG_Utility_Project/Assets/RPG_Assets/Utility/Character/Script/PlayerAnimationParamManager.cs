using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimationParamManager : MonoBehaviour
{
    private Animator anim;
    public float hairnum;
    public float headnum;
    public float bodynum;
    public float legnum;
    // Use this for initialization
    void Start ()
	{
	    anim = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		
	}

    public int GetAnimationNum(int i)
    {
        switch (i)
        {
            case 0:
                return (int)Mathf.Floor(hairnum);
                break;
            case 1:
                return (int)Mathf.Floor(headnum);
                break;
            case 2:
                return (int)Mathf.Floor(bodynum);
                break;
            case 3:
                return (int)Mathf.Floor(legnum);
                break;
        }

        return 0;
    }

    public int GetDirection()
    {
        return anim.GetInteger("Direction");
    }
}
