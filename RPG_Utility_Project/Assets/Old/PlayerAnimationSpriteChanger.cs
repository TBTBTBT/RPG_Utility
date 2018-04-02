using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimationSpriteChanger : MonoBehaviour { // SpriteCreater {
    //private static int idMainTex = Shader.PropertyToID("_MainTex");
    [Header("参照するアニメーションパラメーター\n")]
   //public PlayerAnimationManager _animParam;
   public List<SpriteManager> _sprites;
    void Start()
    {
        foreach (var s in _sprites)
        {

            s.Init();
        }

    }
    void LateUpdate()
    {
        int snum = 0;
        foreach (var s in _sprites)
        {
            
            //s.ChangeSprite(_animParam.GetAnimationNum(snum),_animParam.GetDirection());
            snum++;
        }

//        sr.SetPropertyBlock(block);
    }

  
    

}

[Serializable]
public class SpriteManager
{
    public string partsName;
    [Header("テクスチャのファイルパス(数字は除く)")]
    public string texturePath;
    private List<Sprite> _sprites = new List<Sprite>();
    [Header("反映するRenderer")] public SpriteRenderer sr;

    [Header("1アニメーションあたりの枚数")] public int num = 1;
    protected int nowSpriteNumber = 0;
    protected Sprite GetSprite(int i)
    {
        //Debug.Log(_sprites.Count);
        if (i >= 0 && i < _sprites.Count)
            return _sprites[i];
        return null;
    }
    public void Init()
    {
        LoadSprite();
        //sr = srGetComponent<SpriteRenderer>();
        Debug.Log(_sprites.Count);
        sr.sprite = GetSprite(0);
    }
    void LoadSprite()
    {
        _sprites.AddRange(LoadSpriteFromResource(texturePath,0));
    }

    public Sprite[] LoadSpriteFromResource(string fileName, int num)
    {
        string numString = num < 10 ? "0" + num.ToString() : num.ToString();
        Sprite[] sprites = Resources.LoadAll<Sprite>(fileName+numString);
        return sprites;
    }

    public void ChangeSprite(int i,int d)
    {
        if (i >= 0)
        {
            int sn = i + d * num;
            if (nowSpriteNumber != sn)
            {
                
                sr.sprite = GetSprite(sn);
                nowSpriteNumber = sn;
                
            }
        }

    }
}