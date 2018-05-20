using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyBase : RPGCharacter,IDamageable,IParamater,ITargetable
{
    protected float _moveSpeed = 1f;

    private int damageTime = 0;
    private int attackTime = 0;

    protected bool isNotice = false;


    public string _Name { get; set; }
    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }

    public List<ItemTable> _itemTable;
    protected GameObject _target;
    #region ITarget

    public TargetType GetTargetType()
    {
        TargetType t = TargetType.Attack;
        return t;
    }

    #endregion
    protected virtual void MoveActive(Vector2 playerPos){

    }
    protected virtual void MovePassive(){
        
    }

	// Use this for initialization
	void Start () {
        
        UISpawner.Spawn(this.gameObject, this);
        Init();
        _Hp = _MaxHp;
	}
    protected virtual void Init(){
        
    }
    protected virtual void ChangeNotice(GameObject player){
        
    }
	// Update is called once per frame
	void FixedUpdate () {
        //_ = GameObject.FindWithTag("Player");
        if (_target != null)
        {
            ChangeNotice(_target);
        }
        if (_target && isNotice)
        {
            MoveActive(_target.transform.position);
        }
        else
        {
            MovePassive();
        }
        //move-----------
        if (damageTime == 0 && attackTime == 0)
        {
            rig.velocity = (Vector2)_moveDirection.normalized * _moveSpeed;
        }

        rig.velocity += force;
        force *= 0.9f;
        //---------------
	}

    public void TakeDamage(int damage,Vector2 kb, GameObject attacker)
    {
        KnockBack(kb);
        _Hp -= damage;
        if (_Hp <= 0)
        {
            KillSelf();


        }
        _target = attacker;
        EffectManager.EffectText("DamageText",(Vector2)transform.position + new Vector2(0,0.4f) + kb/15,""+damage);
    }

    void KillSelf()
    {
        MessageManager.AddMsg(" を たおした");
        EffectManager.Effect("Destroy_0", transform.position);
        ItemManager.Spawn(transform.position, _itemTable);
        Destroy(this.gameObject);
    }
    void KnockBack(Vector2 f)
    {
        force += f;

    }
    private void OnTriggerStay2D(Collider2D c)
    {
        //Damageableに対して処理
        c.transform.root.GetInterface<IDamageable>((damagable)=>{
            if (c.tag == "Player" || c.tag == "NPC")
            {

                damagable.TakeDamage(_Attack, -(transform.position - c.transform.position).normalized * 10,gameObject);
            }
            if (c.tag == "Enemy")
            {
                KnockBack((transform.position - c.transform.position).normalized / 10);
            }
        });
       // IDamageable damagable = c.transform.root.GetInterface<IDamageable>();
        //if (damagable != null)
        //{
            
        //}
    }
}