using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
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

/// <summary>
/// 見た目の処理
/// </summary>
[Serializable]
public class FieldViewController
{
    [Header("フィールドオブジェクトのプレハブ")]
    public GameObject _fieldPrefab;
    [Header("フィールドオブジェクトの親オブジェクト")]
    public GameObject _fieldRoot;

    public GameObject[,] _fieldObject;
    public void Init()
    {
        /*
        FieldManager field = FieldManager.Instance;

        _fieldObject = new GameObject[field._width,field._height];
        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                _fieldObject[i,j] = GameObject.Instantiate(_fieldPrefab, _fieldRoot.transform);
                Vector2 pos = field.IndexToPosition(new Vector2Int(i, j));
                _fieldObject[i, j].transform.position = new Vector3(pos.x,pos.y,10);

                
            }
        }
        field.OnChangeField.AddListener(ChangeField);
        */
    }

    void ChangeField()
    {
        /*
        FieldManager field = FieldManager.Instance;

        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                if (_fieldObject[i, j].GetComponent<FieldView>())
                {
                    _fieldObject[i, j].GetComponent<FieldView>().ChangeSprite(field.GetFieldInfo(i,j));
                }
            }
        }
        */
    }
}

/// <summary>
/// フィールドに関する処理を行うクラス
/// </summary>
public class FieldManager : SingletonMonoBehaviourCanDestroy<FieldManager>
{
    [System.NonSerialized]
    public int _width = 50;
    [System.NonSerialized]
    public int _height = 50;

    private FieldInfo[,] _field;

    //フィールドの見た目の大きさ倍率
    private float _extend = 1f;

    public FieldViewController _fieldView;

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

        _fieldView.Init();
    }

    void Start () {
		
	}
	

	void Update () {
		
	}

    #region 外部からフィールドを変更する

    public void FieldGenerate(FieldInfo[,] gen)
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _field[i, j] = gen[i, j];
            }
        }
        OnChangeField.Invoke();
    }

    #endregion

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
        Vector2 ret = new Vector2(fx - _width / 2f, -(fy - _height / 2f)) * _extend;
        return ret;
    }
    public Vector2Int PositionToIndex(Vector2 pos)
    {
        int fx =Mathf.RoundToInt((_width / 2f + pos.x / _extend));
        int fy = Mathf.RoundToInt(((_height / 2f - pos.y / _extend)));
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
    /// 情報を受け取る（全部）
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public FieldInfo GetFieldInfo(int x, int y)
    {
        return _field[x, y];
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
	int BoolToInt(bool b){
		return b ? 1 : 0;
	}
	/// <summary>
	/// 壁生成用 8方向の壁情報をintで取得
	/// </summary>
	/// <returns>The surround.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public int FieldSurround(int x,int y){
		int ret = 0;//0b0000
		/*
		int shift = 0;
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				ret += BoolToInt (IsFieldPassable (x + i, y + i)) << shift;
				shift++;
			}
		}
*/
		ret += BoolToInt (IsFieldPassable (x - 1, y )) << 0;
		ret += BoolToInt (IsFieldPassable (x , y - 1)) << 1;
		ret += BoolToInt (IsFieldPassable (x + 1, y )) << 2;
		ret += BoolToInt (IsFieldPassable (x , y + 1)) << 3;
		return ret;

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
