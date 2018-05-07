using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyBase : RPGCharacter {
    float _moveSpeed = 1f;
    Vector2 _moveDirection = new Vector2(0,0);
    private Vector2 force = new Vector2(0, 0);
    private int damageTime = 0;
    private int attackTime = 0;

    private Rigidbody2D rig;

    public enum BehaviourState
    {
        Wait,
        Walk,
        Jump
    }

    protected virtual void Move(Vector2 playerPos){

        //baseの前に移動処理
        _moveDirection = (playerPos - (Vector2)transform.position);
        //FieldManager field = FieldManager.Instance;
        //if (field)
        {

            if (damageTime == 0 && attackTime == 0)
            {
                rig.velocity = (Vector2)_moveDirection.normalized * _moveSpeed;
            }

            rig.velocity += force;
            force *= 0.9f;

            //ステート変更
            if (_moveDirection.magnitude > 0)
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), true);
            }
            else
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), false);
            }
            //古いコリジョン
            /*
            Vector2Int pos = field.PositionToIndex(transform.position);
            bool left = field.IsFieldPassable(pos + new Vector2Int(-1, 0));
            bool up = field.IsFieldPassable(pos + new Vector2Int(0, 1));
            bool right = field.IsFieldPassable(pos + new Vector2Int(1, 0));
            bool down = field.IsFieldPassable(pos + new Vector2Int(0, -1));
            */
            //Gravitation(left, up, right, down, pos, 0.25f, 0.9f);



        }
    }
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
        SetStates<BehaviourState>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GameObject p = GameObject.FindWithTag("Player");
        Move(p.transform.position);
	}
}
