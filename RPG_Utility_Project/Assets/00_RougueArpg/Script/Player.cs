using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : RPGCharacter {
    /// <summary>
    /// ステート一覧
    /// 要編集
    /// 

    /// <summary>
    /// 動作のステート
    /// </summary>
    public enum BehaviourState
    {
        Wait,
        Walk,
        Attack01,
        Attack02,
        Damage,
        Dead
    }

    private float _moveSpeed = 2.5f;

    float _attackRange = 1;
    Vector2 _moveDirection = new Vector2(0, 0);
    Vector2 _moveTo = new Vector2(0, 0);
    private Rigidbody2D rig;
    private bool pushA = false;
    private Vector2 force;
    private int damageTime = 0;
    private int attackTime = 0;
    GameObject _attackTarget;//戦闘中の敵
    public WeaponManager _weapon;

    // Use this for initialization
    void Start()
    {
        EventManager.OnTouchMove.AddListener(InputTouch);
        rig = GetComponent<Rigidbody2D>();
        //state初期化
        SetStates<BehaviourState>();
        _moveTo = transform.position;
        if (_weapon != null)
        {

        }
    }

    #region 入力

    void InputTouch(int i)
    {
        //if (TouchManager.Instance.GetTouchDistance().magnitude > 5f)
        //    _moveDirection = TouchManager.Instance.GetTouchDistance();
        Vector2 touch = (Vector2)TouchManager.Instance.GetTouchWorldPos(0);
        _moveTo = touch;
        FindTargetEnemy();
        //else
        {
        //    _moveDirection = Vector2.zero;
        }
    }

    void InputKey()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * 10;

    }
    bool IsArriveMoveToPoint(){
        Vector2 d = _moveTo - (Vector2)transform.position;
        return d.magnitude <= 0.2f;
    }
    #endregion
    #region 移動
    void Move()
    {
        if (!IsArriveMoveToPoint())
        {
            _moveDirection = _moveTo - (Vector2)transform.position;
        }
        else{
            _moveDirection = Vector2.zero;
        }
        {
           /*
            Vector2Int pos = field.PositionToIndex(transform.position);
            bool left = field.IsFieldPassable(pos + new Vector2Int(-1, 0));
            bool up = field.IsFieldPassable(pos + new Vector2Int(0, 1));
            bool right = field.IsFieldPassable(pos + new Vector2Int(1, 0));
            bool down = field.IsFieldPassable(pos + new Vector2Int(0, -1));
            */
            //if (damageTime == 0 && attackTime == 0)
            {
                rig.velocity = (Vector2)_moveDirection.normalized * _moveSpeed;
            }

            rig.velocity += (Vector2)force;
            force *= 0.9f;
            //Gravitation(left, up, right, down, pos, 0.25f, 0.9f);


            //ステート変更
            if (_moveDirection.magnitude > 0)
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), true);
            }
            else
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), false);
            }

        }
    }
    #endregion

    #region ステート変更

    MoveDirection AccToMoveDirection()
    {
        float angle = MathUtil.GetAim(Vector2.zero, _moveDirection.normalized);
        MoveDirection ret = MoveDirection.None;
        float sin = Mathf.Sin(angle * Mathf.PI / 180);
        float cos = Mathf.Cos(angle * Mathf.PI / 180);
        if (_moveDirection.magnitude > 0.1f)
        {
            if (Mathf.Abs(sin) > Mathf.Abs(cos))
            {
                if (sin >= 0) ret = MoveDirection.Up;
                if (sin < 0) ret = MoveDirection.Down;
            }
            else
            {
                if (cos < 0) ret = MoveDirection.Left;
                if (cos >= 0) ret = MoveDirection.Right;
            }
        }

//        Debug.Log("" + ret);
        return ret;
    }
    void ChangeDirection()
    {
        ChangeDirection(AccToMoveDirection());
    }
    void TargetAttack(){
        if(_attackTarget){
            _moveTo = _attackTarget.transform.position;
            if (((Vector2)transform.position - (Vector2)_attackTarget.transform.position).magnitude < _attackRange)
            {
                _moveTo = transform.position;
            }

        }
    }
    #endregion
    #region Weapon関係
    void ChangeWeaponState(){
        if (_weapon != null)
        {
            _weapon.ChangeDirection((int)AccToMoveDirection());
        }
    }
#endregion
    public bool IsTarget(){
        return _attackTarget != null;
    }
    public Vector2 TargetPos(){
        return _attackTarget.transform.position;
    }
    void FindTargetEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").Where((go) => (_moveTo - (Vector2)go.transform.position).magnitude < 1f);
        if (enemies.Count() > 0)
        {
            foreach (var enemy in enemies)
            {//(go) =>  ((Vector2)go.transform.position - (Vector2)transform.position).magnitude < _attackRange)){
                Debug.Log("a");
                _attackTarget = enemy;
                //if((_moveTo - (Vector2)enemy.transform.position).magnitude < _attackRange)
                //_moveTo = transform.position;
            }
        }else{
            _attackTarget = null;
        }
    }
    private void FixedUpdate()
    {
        Move();
        TargetAttack();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsArriveMoveToPoint())
        {
            _moveTo = transform.position;
        }
        //InputTouch();
        //InputKey();
        TargetAttack();
        if (damageTime == 0 && attackTime == 0)
        {
            ChangeDirection();
            ChangeWeaponState();
        }

        if (attackTime > 0)
        {
            attackTime--;
            ChangeBehaviourState(BehaviourState.Attack01.ToString(), true);
        }
        else
        {
            ChangeBehaviourState(BehaviourState.Attack01.ToString(), false);
        }




        if (damageTime > 0)
        {
            damageTime--;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BeginPushA();
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            EndPushA();

        }
    }
    //use linq
    void EnemySerch(){
        
    }



    //old
    void BeginPushA()
    {
        /*if (!pushA)
        {
            pushA = true;
            Attack01();
            force = _moveDirection.normalized * 0.1f;
            attackTime = 20;
        }*/
        pushA = true;
    }
    void EndPushA()
    {
        if (pushA)
        {
            pushA = false;
            force = _moveDirection.normalized * 0.1f;
            attackTime = 20;
        }
        //pushA = false;
    }

    void BeginPushB()
    {
        pushA = true;
    }
    void EndPushB()
    {
        pushA = false;
    }
    void KnockBack(Vector2 f)
    {
        force += f;

    }
    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.tag == "Enemy")
        {
            KnockBack((transform.position - c.transform.position).normalized * 5);
//            Debug.Log("h");
        }
    }

}
