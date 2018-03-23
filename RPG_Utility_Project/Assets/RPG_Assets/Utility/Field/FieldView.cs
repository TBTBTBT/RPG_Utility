using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum FieldAroundState
{
    PassableLeft,
    PassableUp,
    PassableDown,
    PassableRight,
}
public class FieldView : MonoBehaviour
{
    public Sprite _passsable;

    public Sprite _wall;

    public SpriteRenderer _renderer;


    Dictionary<FieldParam,bool> _state = new Dictionary<FieldParam, bool>();
    Dictionary<FieldAroundState,bool> _aroundState = new Dictionary<FieldAroundState, bool>();
	// Use this for initialization

    public void ChangeSprite(FieldInfo f)
    {
        foreach (FieldParam p in Enum.GetValues(typeof(FieldParam)))
        {
            _state[p] = f.GetFieldState(p);
        }

        if (_state[FieldParam.IsPassable])
        {
            _renderer.sprite = _passsable;
        }
        else
        {
            _renderer.sprite = _wall;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
