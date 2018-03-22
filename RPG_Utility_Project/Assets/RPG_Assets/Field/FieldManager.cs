using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// FieldInfoで管理する情報リスト (Bool)
/// </summary>
public enum FieldParam
{
    IsPassable,//通行可能か
    IsUnlock,  //通行可能か(可変)
}
/// <summary>
/// 通行できる場所や、床の状態を管理するクラス
/// </summary>
public class FieldInfo
{
    //管理する情報
    private Dictionary<FieldParam, bool> _state = new Dictionary<FieldParam, bool>();

    public FieldInfo()
    {
        foreach(FieldParam p in Enum.GetValues(typeof(FieldParam))){
            _state.Add(p,false);
        }
    }

    public bool GetFieldState(FieldParam param)
    {
        if (_state.ContainsKey(param))
        {
            return _state[param];
        }
        else
        {
            return false;
        }
    }

    public void SetFieldState(FieldParam param, bool state)
    {
        if (_state.ContainsKey(param))
        {
            _state[param] = state;
        }
    }
    
}
public class FieldManager : SingletonMonoBehaviourCanDestroy<FieldManager>
{
    [System.NonSerialized]
    public int _width = 15;
    [System.NonSerialized]
    public int _height = 15;

    private FieldInfo[,] _field;

    //フィールドの見た目の大きさ倍率
    private float _extend = 1.6f;

    //フィールドの内部値が変更されたときに呼び出される
    [System.NonSerialized]
    public UnityEvent OnChangeField = new UnityEvent();


    void Awake()
    {
        base.Awake();
        FieldInit();
        //テスト用
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                SetFieldState(i,j,FieldParam.IsPassable, true);
                SetFieldState(i, j, FieldParam.IsUnlock, true);
            }
        }
    }

    void Start () {
		
	}
	

	void Update () {
		
	}


    #region 初期化

    void FieldInit()
    {
        _field = new FieldInfo[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _field[i, j] = new FieldInfo();
            }
        }
    }

    #endregion


    #region 座標変換


    public Vector2 IndexToPosition(Vector2Int pos)
    {
        float fx = (float)pos.x;
        float fy = (float)pos.y;
        Vector2 ret = new Vector2(fx - _width / 2, -(fy - _height / 2)) * _extend;
        return ret;
    }
    public Vector2Int PositionToIndex(Vector2 pos)
    {
        int fx = (int)((_width / 2f + pos.x / _extend));
        int fy = (int)((_height / 2f - pos.y / _extend));
        fx = IndexModify(fx, 0, _width - 1);
        fy = IndexModify(fy, 0, _height - 1);
        Vector2Int ret = new Vector2Int(fx, fy);
        return ret;
    }
    //はみ出したindexを範囲内にする。
    int IndexModify(int ind, int min, int max)
    {
        if (ind < min) return min;
        if (ind > max) return max;
        return ind;
    }
    #endregion


    #region フィールド情報操作
    /// <summary>
    /// 情報を受け取る
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public bool GetFieldState(Vector2 pos, FieldParam param)
    {
        return GetFieldState(PositionToIndex(pos), param);
    }
    public bool GetFieldState(Vector2Int pos, FieldParam param)
    {
        return _field[pos.x, pos.y].GetFieldState(param);
    }
    public bool GetFieldState(int x,int y,FieldParam param)
    {
        return _field[x,y].GetFieldState(param);
    }

    /// <summary>
    /// 情報を更新する !!!OnChangeFieldイベント発動!!!
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="param"></param>
    /// <param name="state"></param>
    public void SetFieldState(Vector2 pos, FieldParam param, bool state)
    {
        SetFieldState(PositionToIndex(pos), param, state);
    }
    public void SetFieldState(Vector2Int pos, FieldParam param,bool state)
    {
        SetFieldState(pos.x,pos.y,param,state);
    }
    public void SetFieldState(int x, int y, FieldParam param, bool state)
    {
        _field[x, y].SetFieldState(param, state);
        OnChangeField.Invoke();
    }
    #endregion

    #region 通行可能チェック
    //通行可能か isUnlock isPassableがtrueなら通行可能
    public bool IsFieldPassable(Vector2Int pos)
    {
        return IsFieldPassable(pos.x, pos.y);
    }
    public bool IsFieldPassable(int x, int y)
    {

        if (FieldIndexCheck(x, y))
            return GetFieldState(x,y,FieldParam.IsUnlock) && GetFieldState(x, y, FieldParam.IsPassable);
        return false;
    }


    #endregion


    #region フィールド配列チェック
    //trueなら大丈夫 falseならはみ出し
    public bool FieldIndexCheck(Vector2Int pos, Vector2Int move)
    {
        Vector2Int ind = pos + move;
        if (ind.x < 0 || ind.x >= _field.GetLength(0))
        {
            return false;
        }
        if (ind.y < 0 || ind.y >= _field.GetLength(1))
        {
            return false;
        }
        return true;
    }
    public bool FieldIndexCheck(int x, int y)
    {
        if (x < 0 || x >= _field.GetLength(0))
        {
            return false;
        }
        if (y < 0 || y >= _field.GetLength(1))
        {
            return false;
        }
        return true;
    }
    #endregion
}
