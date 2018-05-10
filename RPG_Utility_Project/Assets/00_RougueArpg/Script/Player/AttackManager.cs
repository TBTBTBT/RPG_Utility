using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : SingletonMonoBehaviourCanDestroy<AttackManager>
{
    public GameObject _AllyAttack;
    public static void Attack(Vector2 pos, Vector2 move, int time, float rad,bool penet)
    {
        if (Instance)
        {
            AllyAttack a = GameObject.Instantiate(Instance._AllyAttack, pos, Quaternion.identity)
                .GetComponent<AllyAttack>();
            a.SetAttack(move,time,rad,penet);
        }
    }
}
