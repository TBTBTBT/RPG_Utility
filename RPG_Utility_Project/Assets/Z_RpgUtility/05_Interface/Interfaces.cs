using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage,Vector2 knockback );
}
public interface IParamater
{
    int _MaxHp { get; set; }
    int _Hp { get; set; }
    int _Attack { get; set; }
    int _Speed { get; set; }
    int _Deffence { get; set; }
}