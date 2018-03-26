using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteCreater : MonoBehaviour {

    public List<Texture2D> texture;
    protected List<Sprite> _sprites = new List<Sprite>();
    public int _size = 32;

    void Awake()
    {

        CreateSprites();
    }
    /// <summary>
    /// スプライトリストを作成
    /// </summary>
    void CreateSprites()
    {
        foreach (Texture2D t in texture)
        {
            int w = t.width / _size;
            int h = t.height / _size;
            int max = w * h;
            for (int j = 0; j < h; j++)
            {

                for (int i = 0; i < w; i++)
                {
                
                    Color[] pixels = t.GetPixels(_size * i, _size * j, _size, _size);

                    Texture2D nt = new Texture2D(_size, _size, TextureFormat.ARGB32, false, false);
                    nt.filterMode = FilterMode.Point;
                    nt.SetPixels(pixels);
                    _sprites.Add(Sprite.Create(nt, new Rect(0, 0, nt.width, nt.height), new Vector2(0.5f, 0.5f), _size));
                }

            }
        }

        _sprites.Reverse();
    }

    protected Color[] GetPixels(int i)
    {
        if (i >= 0 && i < _sprites.Count)
            return _sprites[i].texture.GetPixels();
        return Enumerable.Repeat(new Color(0, 0, 0, 0), _size * _size).ToArray();
    }
    protected Sprite GetSprite(int i)
    {
        //Debug.Log(_sprites.Count);
        if (i >= 0 && i < _sprites.Count)
            return _sprites[i];
        return null;
    }

}
