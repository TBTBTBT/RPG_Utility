using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviourCanDestroy<EffectManager>
{
    public GameObject _damageText;
    public GameObject _attack;
    private Dictionary<string, GameObject> _effects = new Dictionary<string, GameObject>();
	// Use this for initialization
	void Start () {
		_effects.Add("DamageText",_damageText);
	    _effects.Add("Attack_00", _attack);
    }

    public static void Effect(string name,Vector2 pos)
    {
        if (Instance)
        {
            Instance.InstantiateEffect(Instance._effects[name],pos);
        }
    }

    public static void MoveEffect(string name, Vector2 pos ,Vector2 move)
    {
        if (Instance)
        {
            Instantiate(Instance._effects[name], pos, Quaternion.AngleAxis(MathUtil.GetAim(Vector2.zero,move),new Vector3(0,0,1)));
        }
    }
    void InstantiateEffect(GameObject prefab,Vector2 pos)
    {
        Instantiate(prefab,pos,Quaternion.identity);
    }
}
