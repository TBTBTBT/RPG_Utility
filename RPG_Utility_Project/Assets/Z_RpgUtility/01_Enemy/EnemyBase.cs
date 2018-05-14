using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyBase : RPGCharacter,IDamageable,IParamater {
    protected float _moveSpeed = 1f;
    protected Vector2 _moveDirection = new Vector2(0,0);
    private Vector2 force = new Vector2(0, 0);
    private int damageTime = 0;
    private int attackTime = 0;

    protected bool isNotice = false;

    private Rigidbody2D rig;

    public string _Name { get; set; }
    public int _MaxHp { get; set; }
    public int _Hp { get; set; }
    public int _Attack { get; set; }
    public int _Speed { get; set; }
    public int _Deffence { get; set; }
    public int _Magic { get; set; }
    public int _HpRegen { get; set; }
    protected virtual void MoveActive(Vector2 playerPos){

    }
    protected virtual void MovePassive(){
        
    }
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
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
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            ChangeNotice(p);
        }
        if (p && isNotice)
        {
            MoveActive(p.transform.position);
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

    public void TakeDamage(int damage,Vector2 kb)
    {
        KnockBack(kb);
        _Hp -= damage;
        if (_Hp <= 0)
        {
            KillSelf();


        }
        EffectManager.EffectText("DamageText",(Vector2)transform.position + new Vector2(0,0.4f) + kb/15,""+damage);
    }

    void KillSelf()
    {
        MessageManager.AddMsg(" を たおした");
        EffectManager.Effect("Destroy_0", transform.position);
        Destroy(this.gameObject);
    }
    void KnockBack(Vector2 f)
    {
        force += f;

    }
    private void OnTriggerStay2D(Collider2D c)
    {
        IDamageable damagable = (IDamageable) c.transform.root.GetComponent(typeof(IDamageable));
        if (damagable != null)
        {
            if (c.tag == "Player")
            {

                damagable.TakeDamage(_Attack, -(transform.position - c.transform.position).normalized * 10);
            }
            if(c.tag == "Enemy"){
                KnockBack((transform.position - c.transform.position).normalized/10);
            }
        }
    }
}