using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastMessage : MonoBehaviour
{
    public string message;

    public void BroadcastAllMessage()
    {
        BroadcastAll(message);
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
