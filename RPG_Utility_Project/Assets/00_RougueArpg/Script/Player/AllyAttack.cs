using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class AllyAttack : MonoBehaviour
{
    private float _radius = 0.1f;
    private int _time = 10;
    Vector2 _move = new Vector2(0,0);
    private bool _penetration = false;
    int _attack = 0;
    public CircleCollider2D _collision;
    GameObject _attacker;

    public void SetAttack( Vector2 move, int time, float rad,int attack,bool penet,GameObject attacker)
    {
        _move = move;
        _time = time;
        _radius = rad;
        _collision.radius = _radius;
        _penetration = penet;
        _attack = attack;
        _attacker = attacker;
    }
	// Update is called once per frame
	void Update ()
	{
	    _time--;
	    transform.position += (Vector3)_move;
	    if (_time <= 0)
	    {
            Destroy(this.gameObject);
	    }
	}
    private void OnTriggerEnter2D(Collider2D c)
    {
        c.transform.root.GetInterface<IDamageable>((damagable) =>
        {
            if (c.tag == "Enemy")
            {
                damagable.TakeDamage(_attack, -(transform.position - c.transform.position).normalized * 5,_attacker);

                if (!_penetration) Destroy(gameObject);
            }
        });
    }
}
