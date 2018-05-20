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
    //[Header("Msgボックスのアニメーター")]
    public Animator _msgAnimator;
    [Header("表示時間")]
    public float dispTime = 3;


    [Header("Talkボックス")]

    public Text _talkBox;
    public Animator _talkAnimator;
    [Header("表示時間")]
    public float talkDispTime = 3;


    public static void Talk(string msg){
        if (Instance)
        {
            Instance._talkAnimator.SetTrigger("Open");
            if (Instance.singleCoroutineTalk != null) Instance.StopCoroutine(Instance.singleCoroutineTalk);
            Instance.singleCoroutineTalk  = Instance.StartCoroutine(Instance.DispTalk());
        }
    }
    private Coroutine singleCoroutineTalk = null;
    IEnumerator DispTalk()
    {
        yield return new WaitForSeconds(talkDispTime);
        _talkAnimator.SetTrigger("Close");
    }
    public static void AddMsg(string msg)
    {
        if (Instance)
        {
            Instance.AddMessage(msg);
            Instance.UpdateMsgBox();
        }
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
        _msgAnimator.SetTrigger("Open");
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
        _msgAnimator.SetTrigger("Close");
    }
}
