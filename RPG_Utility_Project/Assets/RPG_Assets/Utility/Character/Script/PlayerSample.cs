
//PlayerSample.cs

#region UML
/*

@startuml
PlayerSample -|> CharacterOnField:継承
CharacterOnField -|> CharacterBase:継承
CharacterBase -|> MonoBehaviour:継承
CharacterBase ..> AnimationManagerBase:CharacterBaseのステート共有
FieldManager ..> CharacterOnField:フィールド情報共有
@enduml

 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerSample : CharacterOnField {

    /// <summary>
    /// ステート一覧
    /// 要編集
    /// 

    /// <summary>
    /// 動作のステート
    /// </summary>
    public enum BehaviourState
    {
        Wait,
        Walk,
        Attack01,
        Attack02,
        Damage,
        Dead
    }
    /// <summary>
    /// ステート一覧の初期化
    /// </summary>
    protected override void SetStates()
    {
        foreach (BehaviourState p in Enum.GetValues(typeof(BehaviourState)))
        {
            _behaviourStates.Add(p.ToString(),false);
        }
    }


    private float _moveSpeed = 0.05f;
    Vector2 _moveDirection = new Vector2(0,0);
    private bool pushA = false;
    #region 入力

    void InputTouch()
    {
        if (TouchManager.Instance.GetTouchDistance().magnitude > 5f)
            _moveDirection = TouchManager.Instance.GetTouchDistance();
        else
        {
            _moveDirection = Vector2.zero;
        }
    }

    void InputKey()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized*10;

    }

    #endregion

    #region 移動
    void Move()
    {
        FieldManager field = FieldManager.Instance;
        if (field)
        {
            Vector2Int pos = field.PositionToIndex(transform.position);
            bool left = field.IsFieldPassable(pos + new Vector2Int(-1, 0));
            bool up = field.IsFieldPassable(pos + new Vector2Int(0, 1));
            bool right = field.IsFieldPassable(pos + new Vector2Int(1, 0));
            bool down = field.IsFieldPassable(pos + new Vector2Int(0, -1));

            transform.position += (Vector3) (Vector2) _moveDirection.normalized * _moveSpeed;
            Gravitation(left, up, right, down, pos, 0.25f, 0.9f);

            //ステート変更
            if (_moveDirection.magnitude > 0)
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), true);
              //  ChangeBehaviourState(BehaviourState.Wait.ToString(), false);
            }
            else
            {
                ChangeBehaviourState(BehaviourState.Walk.ToString(), false);
              //  ChangeBehaviourState(BehaviourState.Wait.ToString(), true);
            }

        }
    }
    #endregion

    #region ステート変更

    MoveDirection AccToMoveDirection()
    {
        float angle = MathUtil.GetAim(Vector2.zero,_moveDirection.normalized);
        MoveDirection ret = MoveDirection.None;
        float sin = Mathf.Sin(angle * Mathf.PI / 180);
        float cos = Mathf.Cos(angle * Mathf.PI / 180);
        if (_moveDirection.magnitude > 5f)
        {
            if (Mathf.Abs(sin) > Mathf.Abs(cos))
            {
                if (sin >= 0) ret = MoveDirection.Up;
                if (sin < 0) ret = MoveDirection.Down;
            }
            else
            {
                if (cos < 0) ret = MoveDirection.Left;
                if (cos >= 0) ret = MoveDirection.Right;
            }
        }

        //Debug.Log("" + ret);
        return ret;
    }
    void ChangeDirection()
    {
            ChangeDirection(AccToMoveDirection());
    }

    #endregion
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
	{
        InputTouch();
	    //InputKey();
        Move();
	    ChangeDirection();

	    if (pushA)
	    {
	        ChangeBehaviourState(BehaviourState.Attack01.ToString(), true);
        }
        else
	    {
	        ChangeBehaviourState(BehaviourState.Attack01.ToString(), false);
        }

	    pushA = false;
	}

    void PushButtonA()
    {
        pushA = true;
    }

    void PushButtonB()
    {
        Attack02();
    }
    public void Attack01()
    {
		PushButtonA ();
        Debug.Log("Attack01()");
    }

    public void Attack02()
    {
        //ChangeState((int)State.Attack02);
    }
    //デバッグ用
#if false
    void OnGUI()
    {
        string outText = "";
        foreach (CharacterParameter p in Enum.GetValues(typeof(CharacterParameter)))
        {
            //outText += p + " : " + _parameter[p] + System.Environment.NewLine;
        }

        FieldManager field = FieldManager.Instance;
        outText += field.PositionToIndex(transform.position);
        outText += _moveDirection + System.Environment.NewLine;
        outText += u + System.Environment.NewLine;
        outText += l +","+ r + System.Environment.NewLine;
        outText += d;
       // outText += "State : " + _states[_nowState];
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        GUIStyle style = new GUIStyle();
        style.fontSize = 12;
        style.fontStyle = FontStyle.Normal;
        style.normal.textColor = Color.white;
        GUI.Label(rect, outText, style);

    }

#endif
}
