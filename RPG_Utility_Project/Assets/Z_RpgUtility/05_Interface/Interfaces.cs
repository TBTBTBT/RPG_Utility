using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage, Vector2 knockback,GameObject attacker = null);
}
public interface IParamater
{
    int _MaxHp { get; set; }
    int _Hp { get; set; }
    int _Attack { get; set; }
    int _Speed { get; set; }
    int _Deffence { get; set; }
    int _Magic { get; set; }
    int _HpRegen { get; set; }
}

public interface ITalkable
{
    string Talk();
}

namespace UnityEngine
{
    public enum TargetType
    {
        None,
        Attack,
        Talk,
        Stair
    }
    public enum ItemType
    {
        None,
        Coin,
        Weapon,
        Item
    }
}

public interface ITargetable
{

    TargetType GetTargetType();
}
public interface ICanPickItem
{
    bool GetItem(ItemType type,int id,int num);

}