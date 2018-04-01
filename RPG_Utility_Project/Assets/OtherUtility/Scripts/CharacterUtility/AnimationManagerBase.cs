using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/// <summary>
/// 未完成
/// </summary>
public class AnimationManagerBase : MonoBehaviour
{
    protected Dictionary<string,bool> _behaviourStates = new Dictionary<string, bool>();
    //protected string[] _directionStates;
    protected int _nowDirectionState = 0;
    protected int _nowBehaviourState = 0;
    public Animator _anim;
    public CharacterBase _animateChara;//キャラクターにアニメーションを付加する場合
    //void ChangeState(string state)
    //{
    //    _anim.SetTrigger(state);
    //}


    protected virtual void Start()
    {

		//キャラクターからデータをもらう
        if (_animateChara)
        {
          //  _animateChara.OnChangeBehaviourState.AddListener(ChangeBehaviourState);
            _animateChara.OnChangeDirectionState.AddListener(ChangeDirectionState);
            _behaviourStates = _animateChara.GetBehaviourStates();

        }
    }
    protected void ChangeBehaviourState(string state)
    {
        if (_behaviourStates.ContainsKey(state))
        {
            _anim.SetTrigger(state);
        }
    }
    protected void ChangeDirectionState(int state)
    {
        _nowDirectionState = state;
        _anim.SetInteger("Direction",state);
    }
}
