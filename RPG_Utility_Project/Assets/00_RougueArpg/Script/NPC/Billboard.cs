using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour,ITalkable
{
    public string talk = "";
    public string Talk()
    {
        return talk;
    }
}
