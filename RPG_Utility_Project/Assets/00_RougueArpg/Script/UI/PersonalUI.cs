using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalUI : MonoBehaviour {
    public SpriteRenderer _hpBarBase;
    public SpriteRenderer _hpBar;
    GameObject _anchor;
    IParamater _data;
	// Use this for initialization
    public void Init(IParamater paramater) {
        _data = paramater;
	}
    private void Update()
    {
        if(_data != null)
        if (_data._MaxHp != 0)
        {
            _hpBar.size = new Vector2(((float)_data._Hp / _data._MaxHp), 0.2f);
            //_hpBarBase.size = new Vector2(((float)_data._MaxHp) / 50, 0.2f);
        }
    }
}
