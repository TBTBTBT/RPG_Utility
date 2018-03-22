﻿
// キャラクターがステージに沿って移動するためのコンポーネント
// また、パラメーター管理も行う。

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




/// <summary>
/// キャラクターのパラメーター
/// HP ATKなど
/// </summary>
public enum CharacterParameter
{
    Hp,
    Attack,
    Deffence,
    Magic,
    Speed,

}




/// <summary>
/// ベースクラスとして扱う。
/// ローグライクやARPG向け
/// 
/// 関係クラス
/// CharacterOnField -依存-> FieldManager
/// </summary>
/// 

public class CharacterOnField : CharacterBase
{
    //touchGestureと互換性がある
    public enum MoveDirection
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        None = 99,
    }

    protected bool isArrive = true;

    protected Dictionary<CharacterParameter, int> _parameter = new Dictionary<CharacterParameter, int>();

    protected override void Awake()
    {
        base.Awake();
        InitParameter();
    }
    #region パラメーター操作

    void InitParameter()
    {
        foreach(CharacterParameter p in Enum.GetValues(typeof(CharacterParameter)))
        {
          
            _parameter.Add(p,0);
           
        }
    }
    void SetParameter(CharacterParameter param,int num)
    {
        if (_parameter.ContainsKey(param))
        {
            _parameter[param] = num;
        }
    }

    int GetParameter(CharacterParameter param)
    {
        if (_parameter.ContainsKey(param))
        {
            return _parameter[param];
        }

        return 0;
    }
    #endregion
    #region 移動
    //向きを変える
    protected void ChangeDirection(MoveDirection dir)
    {
        //
        int state = 0;
        switch (dir)
        {
            case MoveDirection.Left: state = 1; break;
            case MoveDirection.Up: state = 2; break;
            case MoveDirection.Right: state = 3; break;
            case MoveDirection.Down: state = 4; break;
            case MoveDirection.None: state = 0; break;
        }
        //ステート変更
        if (_nowState != state)
        {
            ChangeState(state);
        }
    }


    protected Vector2Int DirectionToVector2Int(MoveDirection dir)
    {
        Vector2Int moveDir = new Vector2Int(0, 0);
        switch (dir)
        {
            case MoveDirection.Left:
                moveDir.x = -1;
                break;
            case MoveDirection.Up:
                moveDir.y = 1;
                break;
            case MoveDirection.Right:
                moveDir.x = 1;
                break;
            case MoveDirection.Down:
                moveDir.y = -1;
                break;
        }

        return moveDir;
    }
    protected bool CanMove(bool left, bool up, bool right, bool down, Vector2Int dir)
    {
        if (!left && dir.x < 0) return false;
        if (!up && dir.y > 0) return false; //upは内部的には↑だが見た目的には↓かも Todo:要確認
        if (!right && dir.x > 0) return false;
        if (!down && dir.y < 0) return false;
        return true;
    }
    /// <summary>
    /// 実質壁とのあたり判定 周囲が壁だったら今いるマスの中心に引き寄せられる
    /// </summary>
    protected void Gravitation(bool left, bool up, bool right, bool down, Vector2Int ind, float buffer = 0)
    {
        FieldManager field = FieldManager.Instance;
        Vector2 pos = field.IndexToPosition(ind);
        float lerpTime = 0.3f;

        if (!left && transform.position.x < pos.x - buffer)
        {

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x - buffer, lerpTime), transform.position.y, transform.position.z);
        }

        if (!up && transform.position.y > pos.y + buffer)
        {
            //Debug.Log(pos.y);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, pos.y + buffer, lerpTime), transform.position.z);
        }

        if (!right && transform.position.x > pos.x + buffer)
        {

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x + buffer, lerpTime), transform.position.y, transform.position.z);
        }

        if (!down && transform.position.y < pos.y - buffer)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, pos.y - buffer, lerpTime), transform.position.z);
        }


    }

    #endregion

}
/*
    public MoveDirection IntToDirection(int dir)
    {
        return (MoveDirection) dir;
    }*/
