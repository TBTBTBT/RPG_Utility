using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationSpriteChanger : SpriteCreater {
    //private static int idMainTex = Shader.PropertyToID("_MainTex");
    SpriteRenderer sr;
    public AnimationParamManager _animParam;
    protected int nowSpriteNumber = 0;

    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = GetSprite(0);
    }
    void LateUpdate()
    {
        ChangeSprite(_animParam.GetAnimationNum());
//        sr.SetPropertyBlock(block);
    }

    void OnValidate()
    {
        //overrideTexture = texture;
    }

    void ChangeSprite(int i)
    {
		if (i >= 0) {
			if (nowSpriteNumber != i) {
				sr.sprite = GetSprite (i);
				nowSpriteNumber = i;
			}
		} else {
			
		}

    }
    

}
