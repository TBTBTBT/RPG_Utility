using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : SingletonMonoBehaviourCanDestroy<MessageManager>
{
    private Queue<string> _message = new Queue<string>();
    private string msgLog = "";
    [Header("Msgボックス")]
    public Text _msgBox;

    [Header("Msgボックスのアニメーター")]
    public Animator _animator;

    [Header("表示時間")]
    public float dispTime = 5;
    public static void AddMsg(string msg)
    {
       Instance.AddMessage(msg);
        Instance.UpdateMsgBox();
    }

    void AddMessage(string msg)
    {
        _message.Enqueue(msg);
        if (_message.Count > 3)
        {
            msgLog += _message.Dequeue() + "\n";
        }
    }
	void Start () {
	    _msgBox.text = "";
        //AddMsg("test");
    }

    void UpdateMsgBox()
    {
        _animator.SetTrigger("Open");
        _msgBox.text = "";
        foreach (var str in _message)
        {
            _msgBox.text += str;
            _msgBox.text += "\n";
        }
        if(singleCoroutine != null)StopCoroutine(singleCoroutine);
        singleCoroutine = StartCoroutine(Disp());
    }

    private Coroutine singleCoroutine = null;
    IEnumerator Disp()
    {
        yield return new WaitForSeconds(dispTime);
        while (_message.Count > 0)
        {
            msgLog += _message.Dequeue() + "\n";
        }
        _animator.SetTrigger("Close");
    }
}
