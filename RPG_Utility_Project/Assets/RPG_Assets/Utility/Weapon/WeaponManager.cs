using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("ひもづけ")]
    public RPGCharacter _character;

    private List<WeaponBase> _weapons = new List<WeaponBase>();

    public List<GameObject> _weaponroot;
    public List<SpriteRenderer> _renderer;

	// Use this for initialization
	void Start ()
	{
        _weapons.Add(new WeaponBase(0));
	    _weapons.Add(new WeaponBase(0));
        for (int i = 0; i < _weapons.Count; i++)
	    {
	        _renderer[i].sprite = _weapons[i].GetSprite();
	    }

	    _character.OnChangeDirectionState.AddListener(ChangeDirection);
	}

    public WeaponBase WeponInfo(int i)
    {
        if (i < _weapons.Count)
        {
            return _weapons[i];
        }

        return null;
    }
    void ChangeDirection(int d)
    {
        float x = 0;
        float z = 0;
        switch (d)
        {
            case 0:
                x = 0;
                z = -1;
                break;
            case 1:
                x = -0.2f;
                z = 1;
                break;
            case 2:
                x = 0;
                z = 1;
                break;
            case 3:
                x = 0.2f;
                z = -1;
                break;
        }

        for ( int i = 0 ; i < _weaponroot.Count;i ++ )
        {
            if (i % 2 != 0)
            {
                x = -x;
                if (d == 0 || d == 2)
                {
                    z = -z;
                }
            }
            _weaponroot[i].transform.localPosition = new Vector3(
                x,
                _weaponroot[i].transform.localPosition.y,
                z);
        }
    }
    void BeginPushA()
    {
        Debug.Log("A");
        Action(0);
    }
    void EndPushA(){
        
    }
    void Action(int i){
        if (_weapons.Count > 0)
        {
            
            switch (_weapons[i].type)
            {
                case WeaponType.Sword:
                   
                    _weaponroot[i].transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 0, 1));
                    break;
            }
        }
    }

}
