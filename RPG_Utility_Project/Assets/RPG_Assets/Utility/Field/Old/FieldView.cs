using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;


enum FieldAroundState
{
    PassableLeft,
    PassableUp,
    PassableDown,
    PassableRight,
}

public class FieldView : MonoBehaviour//SpriteCreater
{

    public SpriteRenderer _floorrenderer;
    public SpriteRenderer _wallrenderer;
    public GameObject _fieldPrefab;
    
    public Texture2D texture;
    protected List<Texture2D> _tex = new List<Texture2D>();
    // Use this for initialization
    public int _size = 32;
    void Start()
    {
        Init();
        //FieldSpriteInit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        FieldManager field = FieldManager.Instance;
        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                GameObject go = GameObject.Instantiate(_fieldPrefab,transform);
                Vector2 pos = field.IndexToPosition(new Vector2Int(i, j));
                go.transform.position = new Vector3(pos.x, pos.y, 10);


            }
        }
        
    }
    int ThreeValueToIndex(int x, int y, int w)
    {
        return x + y * w;
    }
    void MergePixel(Texture2D tx, Color[] arr, int sx, int sy, int size, bool ignoreAlpha = false)
    {

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (!ignoreAlpha || arr[ThreeValueToIndex(i, size - 1 - j, size)].a > 0)
                    tx.SetPixel(sx + i, sy + j, arr[ThreeValueToIndex(i, size - 1 - j, size)]);
            }
        }
    }
    protected Color[] GetPixels(int i)
    {
        if (i >= 0 && i < _tex.Count)
            return _tex[i].GetPixels();
        return Enumerable.Repeat(new Color(0, 0, 0, 0), _size * _size).ToArray();
    }
    void CreateTex()
    {
        Debug.Log("重い処理");

        int w = texture.width / _size;
        int h = texture.height / _size;

        for (int j = 0; j < h; j++)
            {

                for (int i = 0; i < w; i++)
                {

                    Color[] pixels = texture.GetPixels(_size * i, _size * (h - 1 - j), _size, _size);

                    Texture2D nt = new Texture2D(_size, _size, TextureFormat.ARGB32, false, false);
                    nt.filterMode = FilterMode.Point;
                    nt.SetPixels(pixels);
                    //Outliner(nt);
                     nt.Apply();
                    _tex.Add(nt);
                }
            }

        Debug.Log(_tex.Count);
        _tex.Reverse();
    }
    void FieldSpriteInit()
    {
        CreateTex();


        FieldManager field = FieldManager.Instance;
        int num = field._width * _size * field._height * _size;

        Texture2D ft = new Texture2D(field._width * _size, field._height * _size, TextureFormat.ARGB32, false, false);
        ft.filterMode = FilterMode.Point;
        Texture2D wt = new Texture2D(field._width * _size, field._height * _size, TextureFormat.ARGB32, false, false);
        wt.filterMode = FilterMode.Point;
        Color[] fc = GetPixels(11);
        //Color[] wall = GetPixels(1);

        Color[] wall = GetPixels(10);
        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                //床を作成
                 MergePixel(ft, fc, i* _size, j* _size, _size);

                //カベを作成

                int wallIndex = field.FieldSurround(i, j);
                
                if (!field.GetFieldState(i, j, FieldParam.IsPassable))
                {
                    MergePixel(wt, wall, i * _size, j * _size, _size);
                }
                else
                {
                    MergePixel(wt, GetPixels(-1), i * _size, j * _size, _size);
                }
            }
        }

        ft.Apply();
        wt.Apply();
        _floorrenderer.sprite = Sprite.Create(ft, new Rect(0, 0, ft.width, ft.height), new Vector2(0.5f, 0.5f), _size);
        _wallrenderer.sprite = Sprite.Create(wt, new Rect(0, 0, wt.width, wt.height), new Vector2(0.5f, 0.5f), _size);

    }
    /*
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

    void Outliner(Texture2D wt, FieldManager field)
    {
        Color[] wtp = wt.GetPixels();
        Color[] wtpo = wt.GetPixels();
        for (int c = 0; c < wtp.Length; c++)
        {
            bool ol = false;
            if (wtp[c].a <= 0)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i != 0 || j != 0)
                        {
                            int ind = ThreeValueToIndex(c + i, j, field._width * _size);
                            if (ind >= 0 && ind < wtp.Length)
                            {
                                if (wtp[ind].a > 0)
                                {
                                    ol = true;
                                }
                            }
                        }
                    }
                }
            }

            if (ol) wtpo[c] = new Color(0, 0, 0, 1);
        }
        wt.SetPixels(wtpo);
    }
    public void Init()
    {
		
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

    }
    */
}
