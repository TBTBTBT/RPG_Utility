using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour {
    Player _player;
    public Animator _anim;
    State _nowState = State.Normal;
    enum State
    {
        Normal,
        Battle
    }
    void ChangeState(State s){
        if(_nowState != s){
            _nowState = s;
            _anim.SetTrigger(s.ToString());
        }
    }
	// Use this for initialization
	void Start () {
        EventManager.OnTouchMove.AddListener(ArrowMove);
        EventManager.OnTouchEnd.AddListener((i)=>ArrowResize(1f));
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Debug.Log(_player);

	}
    void ArrowResize(float extend){
        transform.localScale = new Vector3(extend,extend,1);
    }
    void ArrowMove(int i){
        ArrowResize(3f);
        if(_player){
            if(_player.IsTarget()){
                ChangeState(State.Battle);
                return;
            }
        }
        ChangeState(State.Normal);
        Vector2 touch = (Vector2)TouchManager.Instance.GetTouchWorldPos(0);
        transform.position = touch;

    }
	// Update is called once per frame
	void Update () {
        if(_player){
            if(_player.IsTarget()){
                transform.position = _player.TargetPos() + new Vector2(0,0.5f);
            }
        }
	}
}
