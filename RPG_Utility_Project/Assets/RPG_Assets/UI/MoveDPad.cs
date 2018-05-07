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

        //mod.x = Limit(mod.x, 60);
        //mod.y = Limit(mod.y, 60);

        transform.localPosition = pos + (Vector3)mod;
	}
    float Limit(float var, float lim)
    {
        float l = 0;
        l = var;
        if (Mathf.Abs(var) > lim)
        {
            l = lim * Mathf.Sign(var);
        }
        return l;
    }
}
