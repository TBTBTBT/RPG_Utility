using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDPad : MonoBehaviour
{
    private Vector3 pos;
	// Use this for initialization
	void Start ()
	{
	    pos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    TouchManager touch = TouchManager.Instance;

	    Vector2 mod = touch.GetTouchDistance();
        transform.localPosition = pos + (Vector3)mod;
	}
}
