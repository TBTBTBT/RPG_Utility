using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : SingletonMonoBehaviourCanDestroy<AttackManager>
{
    public GameObject _AllyAttack;
    public static void Attack(GameObject pos, Vector2 move, int time, float rad,int attack,bool penet)
    {
        if (Instance)
        {
            AllyAttack a = Instantiate(Instance._AllyAttack, pos.transform.position, Quaternion.identity)
                .GetComponent<AllyAttack>();
            a.SetAttack(move,time,rad,attack,penet,pos);
        }
    }
}
