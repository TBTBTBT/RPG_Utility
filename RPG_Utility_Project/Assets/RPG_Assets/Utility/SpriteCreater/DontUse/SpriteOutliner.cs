using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// どっと絵に自動でアウトラインを付ける
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutliner : MonoBehaviour
{
    private SpriteRenderer outSprite;

    //public int _size = 32;
	// Use this for initialization
	void Start ()
	{
	    outSprite = GetComponent<SpriteRenderer>();
	}


}
