using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_00 : EnemyBase
{
    public float _maxMoveSpeed = 2f;
    public float _quick = 0.95f;
    float _jump = 1f;
    public GameObject _jumpRoot;
    public Animator _anim;

    protected override void Init()
	{
        _MaxHp = 10;
        _Attack = 1;


       
        //SetStates<BehaviourState>();
	}
    protected override void ChangeNotice(GameObject player)
	{
        if (!player) return;
        if(((Vector2)player.transform.position - (Vector2)transform.position).magnitude < 1)
        isNotice = true;
	}
	protected override void MoveActive(Vector2 playerPos)
    {
        _moveDirection = (playerPos - (Vector2)transform.position);
        _moveSpeed *= _quick;
        if(_moveSpeed < 0.05f){
            StartCoroutine("Jump");
            _moveSpeed = _maxMoveSpeed;

        }
    }
    protected override void MovePassive()
    {
        
        _moveSpeed *= _quick;
        if (_moveSpeed < 0.05f)
        {
            _moveDirection = MathUtil.RandomAngle(1);
            StartCoroutine("Jump");
            _moveSpeed = _maxMoveSpeed;

        }
    }
    IEnumerator Jump(){
        _jump = 1f;
//        Debug.Log(Mathf.Cos(Mathf.PI * _jump));

        _anim.SetTrigger("Jump");

        while(_jump > 0){
            _jumpRoot.transform.localPosition = new Vector2(0, Mathf.Sin(Mathf.PI * _jump)) * _maxMoveSpeed / 10f;
            _jump -= (1 - _quick);
            yield return null;
        }
        _jumpRoot.transform.localPosition = new Vector2(0, Mathf.Sin(0)) * _maxMoveSpeed / 10f;
        _anim.SetTrigger("Land");

    }
}