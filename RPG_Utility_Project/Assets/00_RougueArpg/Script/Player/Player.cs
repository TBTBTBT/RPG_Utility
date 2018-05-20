using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : RPGCharacter, IDamageable, IParamater, ICanPickItem
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
    Vector2 _moveTo = new Vector2(0, 0);

    private int damageTime = 0;
    private int attackTime = 0;

    private GameObject _target;
    private TargetType _targetType;


    //   GameObject _attackTarget;//戦闘中の敵
    //   GameObject _talkTarget;
    //public WeaponManager _weapon;
    #region パラメーター
    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }
    #endregion
    int _coin = 0;
    // Use this for initialization
    void Start()
    {
        _MaxHp = 10;
        _Hp = _MaxHp;
        _Attack = 2;

        //state初期化
        SetStates<BehaviourState>();
        EventManager.OnTouchBegin.AddListener(InputTouch);
        EventManager.OnTouchMove.AddListener(InputTouch);

        _moveTo = transform.position;
        UISpawner.Spawn(this.gameObject, this);

    }
    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (IsArriveMoveToPoint())
        {
            _moveTo = transform.position;
        }
        TargetAttack();
        Talk();
        ChangeDirection();
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





    #region アイテむ
    public bool GetItem(ItemType type, int id, int num)
    {
        switch (type)
        {
            case ItemType.Coin:
                _coin += num;
                MessageManager.AddMsg(num + " コイン ひろった");
                return true;
        }
        return false;
    }
    #endregion
    #region 入力

    void InputTouch(int i)
    {

        Vector2 touch = (Vector2)TouchManager.Instance.GetTouchWorldPos(0);
        _moveTo = touch;
        FindTarget();
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
   
    #endregion

    #region ターゲット
    void SetTarget(TargetType type, GameObject target)
    {
        _target = target;
        _targetType = type;
    }
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
    //NPCよう
    public GameObject AttackTarget(){
        if (_target.tag == "Enemy")
        {
            return _target;
        }
        return null;
    }
    void FindTarget()
    {
        GameObject target = PointerManager.GetTarget();
        TargetType type = TargetType.None;
        if (target != null)
        {
            bool attackable = IsContainInterface<IDamageable>(target);
            bool talkable = IsContainInterface<ITalkable>(target);
            if (talkable) type = TargetType.Talk;
            else if (attackable) type = TargetType.Attack;



        }
        SetTarget(type, target);
    }

    #endregion
    #region 汎用
    bool IsContainInterface<T>(GameObject go)
    {
        return go.GetComponent(typeof(T)) != null;
    }
    #endregion
    #region 攻撃
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
    void Attack()
    {
        if (_target && _targetType == TargetType.Attack)
        {
            Vector2 distance = _target.transform.position - transform.position;
            attackTime = 30;
            AttackManager.Attack(gameObject, distance.normalized * 0.1f, 10, 0.2f, _Attack, false);
            EffectManager.MoveEffect("Attack_0", transform.position + new Vector3(0, 0.15f, 0), distance.normalized);
            //向かせる
            _moveDirection = distance;
            //EffectManager.AnchoredMoveEffect("Attack_00", transform, distance.normalized);
        }
    }
    #endregion


    void Talk()
    {
        if (_target && _targetType == TargetType.Talk)
        {
            _moveTo = _target.transform.position;
            bool IsRange = ((Vector2)transform.position - (Vector2)_target.transform.position).magnitude < 2;
            if (IsRange)
            {
                ((ITalkable)_target.GetComponent(typeof(ITalkable))).Talk();
                _moveTo = transform.position;
                Vector2 distance = _target.transform.position - transform.position;
                //向かせる
                _moveDirection = distance;
            }
        }

    }
    #region Damageable

    void KnockBack(Vector2 f)
    {
        force += f;

    }

    public void TakeDamage(int damage, Vector2 knockback, GameObject attacker)
    {



        KnockBack(knockback.normalized / 10);
        if (damageTime == 0)
        {

            _Hp -= damage;

            KnockBack(knockback);

            EffectManager.EffectText("DamageText", (Vector2)transform.position + new Vector2(0, 0.5f) + knockback / 15, "" + damage);
            if (_Hp <= 0) Destroy(this.gameObject);
            damageTime = 30;
        }
    }
    #endregion
    // Update is called once per frame
   

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

}
