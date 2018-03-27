using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


enum FieldAroundState
{
    PassableLeft,
    PassableUp,
    PassableDown,
    PassableRight,
}

public class FieldView : SpriteCreater
{

    public SpriteRenderer _floorrenderer;

	public GameObject _fieldPrefab;
    // Use this for initialization

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    int ThreeValueToIndex(int x,int y,int w)
    {
        return x + y * w;
    }
	void MergePixel(Texture2D tx, Color[] arr ,int sx,int sy,int size,bool ignoreAlpha = false)
    {

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
				if(!ignoreAlpha || arr[ThreeValueToIndex(i, size - 1 - j, size)].a > 0)
                tx.SetPixel(sx + i, sy + j, arr[ThreeValueToIndex(i, size - 1 - j, size)]);
            }
        }
    }

    public void Init()
    {
		/*
        FieldManager field = FieldManager.Instance;
        int num = field._width * _size * field._height * _size;

        Texture2D ft = new Texture2D(field._width * _size, field._height * _size, TextureFormat.ARGB32, false, false);
        ft.filterMode = FilterMode.Point;
        Texture2D wt = new Texture2D(field._width * _size, field._height * _size, TextureFormat.ARGB32, false, false);
        wt.filterMode = FilterMode.Point;
        Color[] fc = GetPixels(0);
        //Color[] wall = GetPixels(1);

        Color[] wall = GetPixels(4);
        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                //床を作成
               // MergePixel(ft, fc, i* _size, j* _size, _size);

                //カベを作成

				int wallIndex = field.FieldSurround(i,j);
				MergePixel(wt,GetPixels(-1), i * _size, j * _size, _size);
                if (!field.GetFieldState(i, j, FieldParam.IsPassable))
                {
                    MergePixel(wt, wall, i * _size, j * _size, _size);
                }
            }
        }
        ft.Apply();
        wt.Apply();
        _floorrenderer.sprite = Sprite.Create(ft,new Rect(0,0,ft.width,ft.height),new Vector2(0.5f,0.5f),_size);
        _wallrenderer.sprite = Sprite.Create(wt, new Rect(0, 0, wt.width, wt.height), new Vector2(0.5f, 0.5f), _size);
        //field.OnChangeField.AddListener(ChangeField);
*/
    }

}
