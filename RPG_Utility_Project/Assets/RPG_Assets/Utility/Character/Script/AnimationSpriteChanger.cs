using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationSpriteChanger : MonoBehaviour {
    private static int idMainTex = Shader.PropertyToID("_MainTex");
    [SerializeField]
    private Texture texture = null;
    private MaterialPropertyBlock block;
    SpriteRenderer sr;
    public Texture overrideTexture
    {
        get { return texture; }
        set
        {
            texture = value;
            if (block == null)
            {
                Init();
            }
            block.SetTexture(idMainTex, texture);
        }
    }

    void Awake()
    {
        Init();
        overrideTexture = texture;
    }
    void LateUpdate()
    {
        sr.SetPropertyBlock(block);
        Debug.Log(idMainTex);
    }

    void OnValidate()
    {
        //overrideTexture = texture;
    }
    void Init()
    {
        block = new MaterialPropertyBlock();
        sr = GetComponent<SpriteRenderer>();
        sr.GetPropertyBlock(block);
    }
}
