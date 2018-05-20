using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour,ITalkable, ITargetable
{
    public TargetType GetTargetType()
    {
        return TargetType.Talk;
    }
    public string talk = "";
    public string Talk()
    {
        MessageManager.Talk(talk);
        return talk;
    }
}
