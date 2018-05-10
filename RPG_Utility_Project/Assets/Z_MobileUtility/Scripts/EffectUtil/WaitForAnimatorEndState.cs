using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForAnimatorEndState : CustomYieldInstruction {
    private bool _keepWaiting;
    private Animator _anim;
    public override bool keepWaiting
    {
        get { return !_anim.GetCurrentAnimatorStateInfo(0).IsName("End"); }
    }

    public WaitForAnimatorEndState(Animator anim)
    {
        _anim = anim;
    }
}
