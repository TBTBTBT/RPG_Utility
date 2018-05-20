using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface用getcomponent
/// </summary>
public static class ComponentExtension{
    public static T GetInterface<T>(this Component go) where T : class
    {
        return go.GetComponent(typeof(T)) as T;

    }
    public static T GetInterface<T>(this GameObject go) where T : class
    {
        return go.GetComponent(typeof(T)) as T;

    }
    public static void GetInterface<T>(this Component go,System.Action<T> callback) where T : class
    {
        T cp = go.GetComponent(typeof(T)) as T;
        if(cp != null){
            callback(cp);
        }

    }
    public static void GetInterface<T>(this GameObject go, System.Action<T> callback) where T : class
    {
        T cp = go.GetComponent(typeof(T)) as T;
        if (cp != null)
        {
            callback(cp);
        }

    }
}
    