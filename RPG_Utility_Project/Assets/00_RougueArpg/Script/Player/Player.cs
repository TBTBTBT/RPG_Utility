using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : RPGCharacter, IDamageable, IParamater
{
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
    private Vector2 force;
    private int damageTime = 0;
    private int attackTime = 0;

    private GameObject _target;
    private TargetType _targetType;

    void SetTarget(TargetType type, GameObject target)
    {
        _target = target;
        _targetType = type;
    }
 //   GameObject _attackTarget;//戦闘中の敵
 //   GameObject _talkTarget;
    //public WeaponManager _weapon;

    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }
    // Use this for initialization
    void Start()
    {
        _MaxHp = 10;
        _Hp = _MaxHp;
        _Attack = 2;
        EventManager.OnTouchMove.AddListener(InputTouch);
        rig = GetComponent<Rigidbody2D>();
        //state初期化
        SetStates<BehaviourState>();
        _moveTo = transform.position;
        UISpawner.Spawn(this.gameObject, this);
        Debug.Log(NameGenerator.Generate());
    }

    #region 入力

    void InputTouch(int i)
    {
        //attackTime = 0;
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
    bool IsArriveMoveToPoint()
    {
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
        else
        {
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
    void TargetAttack()
    {
        if (_target && _targetType == TargetType.Attack)
        {
            _moveTo = _target.transform.position;
            bool IsRange = ((Vector2)transform.position - (Vector2)_target.transform.position).magnitude <
                           _attackRange;
            if (IsRange || attackTime > 0)
            {

                _moveTo = transform.position;
            }

            if (IsRange)
            {

                if (attackTime == 0)
                {
                    Attack();


                }
            }
        }
    }
    #endregion
#if false
#region Weapon関係
    void ChangeWeaponState()
    {
        if (_weapon != null)
        {
            _weapon.ChangeDirection((int)AccToMoveDirection());
        }
    }
#endregion
#endif
    public TargetType GetTargetType()
    {
        return _targetType;
    }

    public bool IsTarget()
    {
        return _target != null;
    }
    public Vector2 TargetPos()
    {
        return _target.transform.position;
    }
    bool IsContainInterface<T>(GameObject go)
    {
        return go.GetComponent(typeof(T)) != null;
    }
    void FindTargetEnemy()
    {
        /*
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").Where((go) => (_moveTo - (Vector2)go.transform.position).magnitude < 0.7f).OrderBy((go) => ((Vector2)transform.position - (Vector2)go.transform.position).magnitude);
        if (enemies.Count() > 0)
        {
            foreach (var enemy in enemies)
            {//(go) =>  ((Vector2)go.transform.position - (Vector2)transform.position).magnitude < _attackRange)){
                _attackTarget = enemy;
                break;
                //if((_moveTo - (Vector2)enemy.transform.position).magnitude < _attackRange)
                //_moveTo = transform.position;
            }
        }
        else
        {
            attackTime = 0;
            _attackTarget = null;
        }*/

        GameObject target = PointerManager.GetTarget();
        TargetType type = TargetType.None;
        if (target != null)
        {
            bool attackable = IsContainInterface<IDamageable>(target);
            bool talkable = IsContainInterface<ITalkable>(target);
            type = TargetType.Attack;



        }
        SetTarget(type, target);
    }
    private void FixedUpdate()
    {
        Move();
        //TargetAttack();
    }

    void Attack()
    {
        if (_target && _targetType == TargetType.Attack)
        {
            Vector2 distance = _target.transform.position - transform.position;
            attackTime = 30;
            AttackManager.Attack(transform.position, distance.normalized * 0.1f, 10, 0.2f,_Attack, false);
            EffectManager.MoveEffect("Attack_0", transform.position + new Vector3(0, 0.15f, 0), distance.normalized);
            //向かせる
            _moveDirection = distance;
            //EffectManager.AnchoredMoveEffect("Attack_00", transform, distance.normalized);
        }
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

        ChangeDirection();
        //ChangeWeaponState();
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
    }


#region old
#if false
    //old---
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
#endif
#endregion



    void KnockBack(Vector2 f)
    {
        force += f;

    }

    public void TakeDamage(int damage,Vector2 knockback)
    {



        KnockBack(knockback.normalized/10);
        if (damageTime == 0)
        {
            
            _Hp -= damage;

            KnockBack(knockback);

            EffectManager.EffectText("DamageText", (Vector2)transform.position + new Vector2(0, 0.5f) + knockback / 15,""+damage);
            if (_Hp <= 0) Destroy(this.gameObject);
            damageTime = 30;
        }
    }

}
