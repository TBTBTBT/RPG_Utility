using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : RPGCharacter, ITalkable, ITargetable, IDamageable,IParamater
{
    enum BehaviourState
    {
        Wait,
        Walk,
        Attack01,
        Attack02,
        Damage,
        Dead
    }

    #region パラメーター
    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }
    #endregion
    int damageTime = 0;
    int attackTime = 0;
    protected TargetType _type = TargetType.Talk;
    private float _moveSpeed = 2.5f;
    float _attackRange = 1f;
    protected Vector2 _moveTo = new Vector2(0, 0);
    protected GameObject _target;
    protected Player _player;

    bool _isAlly = false;
    // Use this for initialization
    void Start()
    {
        //dummy
        _Hp = 20;
        _MaxHp = 20;
        _Attack = 3;
        UISpawner.Spawn(gameObject, this);
        SetStates<BehaviourState>();
        _moveTo = transform.position;
        Debug.Log(NameGenerator.Generate());
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

    }
    #region Talk
    public string Talk()
    {
        MessageManager.Talk("");
        _isAlly = true;
        return "";
    }
    public TargetType GetTargetType()
    {
        return _type;
    }
    #endregion
    #region 移動
    void Move()
    {
        if ((_moveTo - (Vector2)transform.position).magnitude < 0.5f)
        {
            _moveTo = transform.position;
        }
        _moveDirection = _moveTo - (Vector2)transform.position;


        rig.velocity = (Vector2)_moveDirection.normalized * _moveSpeed;
        rig.velocity += (Vector2)force;
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

    }

    #endregion
    #region ターゲット

    public bool IsTarget()
    {
        return _target != null;
    }
    public Vector2 TargetPos()
    {
        return _target.transform.position;
    }
    #endregion
    int moveChance = 150;
    void RandomMove(){
        if (Time.frameCount % moveChance == 0 )
        {
            _moveTo = (Vector2)transform.position + MathUtil.RandomAngle(1.5f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        TargetAttack();
        if(!_target){
            if(_player.IsTarget()){
                _target = _player.AttackTarget();
            }
        }
        if(_isAlly && !_target){
            if (((Vector2)_player.transform.position- (Vector2)transform.position).magnitude > 1f)
            {
                _moveTo = _player.transform.position;
            }
        }   
        else{
            RandomMove();
        }
        if (damageTime > 0)
        {
            damageTime--;
        }
        if (attackTime > 0)
        {
            attackTime--;
        }
        ChangeDirection();
    }
    private void FixedUpdate()
    {
        Move();
    }
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
    #region damageable
    void KnockBack(Vector2 f)
    {
        force += f;

    }

    public void TakeDamage(int damage, Vector2 knockback,GameObject attacker)
    {



        KnockBack(knockback.normalized / 10);
        if (damageTime == 0)
        {

            _Hp -= damage;

            KnockBack(knockback);

            EffectManager.EffectText("DamageText", (Vector2)transform.position + new Vector2(0, 0.5f) + knockback / 15, "" + damage);
            if (_Hp <= 0) Destroy(this.gameObject);
            damageTime = 30;
            _target = attacker;
        }
    }
#endregion

    #region 攻撃
    void TargetAttack()
    {
        if (_target)
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
        if (_target)
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
    private void OnTriggerStay2D(Collider2D c)
    {

        if (c.tag == "Player" || c.tag == "NPC")
        {

            KnockBack((transform.position - c.transform.position).normalized / 10);
        }
    }
}
