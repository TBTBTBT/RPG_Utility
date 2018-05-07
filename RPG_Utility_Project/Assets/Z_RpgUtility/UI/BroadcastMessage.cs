using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastMessage : MonoBehaviour
{
    public string messageDown;
    public string messageUp;
    public void BroadcastAllMessage(string msg)
    {
        BroadcastAll(msg);
    }
    public static void BroadcastAll(string fun)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                go.gameObject.BroadcastMessage(fun, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
