using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{

    None,
    Sword,
    Magic,
    Shield,
    Hand,
    Bomb,
    Arrow,

}
public class WeaponBase
{
    public WeaponType type = WeaponType.Sword;
    public string name;
    public int number = 0;
    public float dash = 10;
    public float enemyKnockBack = 10;
    public float damage = 10;
    public float blank = 10;
    public float speed = 10;
    public float attackTime = 10;
//    public AnimationClip;
    public WeaponBase(int n)
    {
        number = n;
    }
    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Image/Weapon/weapon_00");
    }
    public virtual void UseWeapon()
    {

    }
    //飛び込み技など親オブジェクトの移動が必要な場合
    protected virtual void SendMessageParent()
    {
        //transform.root.SendMessage("");
    }
}
