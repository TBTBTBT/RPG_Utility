using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_00 : EnemyBase
{
    float _maxMoveSpeed = 2f;
    float _jump = 1f;
    public GameObject _jumpRoot;
    public Animator _anim;
    protected override void Init()
	{
        //SetStates<BehaviourState>();
	}
	protected override void Move(Vector2 playerPos)
    {
        _moveDirection = (playerPos - (Vector2)transform.position);
        _moveSpeed *= 0.95f;
        if(_moveSpeed < 0.05f){
            StartCoroutine("Jump");
            _moveSpeed = _maxMoveSpeed;

        }
        base.Move(playerPos);
    }
    IEnumerator Jump(){
        _jump = 1f;
//        Debug.Log(Mathf.Cos(Mathf.PI * _jump));

        _anim.SetTrigger("Jump");

        while(_jump > 0){
            _jump -= 0.05f;

            _jumpRoot.transform.localPosition = new Vector2(0, Mathf.Sin(Mathf.PI * _jump ))* _maxMoveSpeed / 10f;
            yield return null;
        }

        _anim.SetTrigger("Land");

    }
}