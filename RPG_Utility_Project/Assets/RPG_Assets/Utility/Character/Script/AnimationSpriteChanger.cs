﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationSpriteChanger : SpriteCreater {
    //private static int idMainTex = Shader.PropertyToID("_MainTex");
    [SerializeField]
    SpriteRenderer sr;
    public AnimationParamManager _animParam;

  
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = GetSprite(0);
    }
    void LateUpdate()
    {
        //sr.SetPropertyBlock(block);
    }

    void OnValidate()
    {
        //overrideTexture = texture;
    }


}