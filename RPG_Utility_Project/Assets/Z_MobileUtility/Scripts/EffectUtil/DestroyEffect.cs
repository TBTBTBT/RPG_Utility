using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public Animator _anim;
    void Awake()
    {
        StartCoroutine(WaitEnd());
    }

    IEnumerator WaitEnd()
    {
        yield return new WaitForAnimatorEndState(_anim);
        Destroy(this.gameObject);
    }
}
