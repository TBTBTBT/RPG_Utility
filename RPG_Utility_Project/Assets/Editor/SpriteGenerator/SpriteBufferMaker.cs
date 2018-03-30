using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteBufferMaker : EditorWindow
{
    private static SpriteBufferMaker _window;
    public Texture2D texture;
    private Texture2D outTex;
    private int size = 32;
    private int padding = 1;
    private int width = 2;

    private int height = 2;

    [MenuItem("Window/PNG Padding Maker %l")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        _window = (SpriteBufferMaker) EditorWindow.GetWindow(typeof(SpriteBufferMaker));
        _window.titleContent.text = "PNG Padding Maker";
        _window.autoRepaintOnSceneChange = false;

    }

    Color[] SeparateSprite()
    {
        int w = texture.width;
        Color[] c = texture.GetPixels();
        List<Color> oc = new List<Color>();
        for (int i = 0; i < c.Length; i++)
        {
            
            if (i % (w * size) == 0 )
            {
                for (int j = 0; j < w; j++)
                {
                    if (j % size == 0)
                    {
                        oc.Add(c[i + j]);

                    }
                    oc.Add(c[i + j]);
                    if (j % size == size - 1)
                    {
                        oc.Add(c[i + j]);

                    }
                }
            }
            if (i % size == 0)
            {
                //oc.Add(new Color(0, 0, 0, 1));
                //if (i % w != 0 && i % w != w - 1)
                oc.Add(c[i]);

            }
            oc.Add(c[i]);
            if (i % size == size - 1)
            {
                oc.Add(c[i]);
            }
            if (i % (w * size) == (w * size) - 1)
            {
                for (int j = 0; j < w ; j++)
                {
                    if (j % size == 0)
                    {
                        oc.Add(c[i - (w-1) + j]);

                    }
                    oc.Add(c[i- (w - 1) + j]);
                    if (j % size == size - 1)
                    {
                        oc.Add(c[i - (w - 1) + j]);

                    }
                }
            }
        }
        
        return oc.ToArray();
    }
    void MergeSprite()
    {

    }
    public void OnGUI()
    {
        //var spriteRect = new Rect(0,0,10,10);
        texture = (Texture2D)EditorGUILayout.ObjectField( texture, typeof(Texture2D),true);
        size =  EditorGUILayout.IntField("sprite size", size);
        //padding = EditorGUILayout.IntField("sprite padding(px)", padding);
        width = EditorGUILayout.IntField("sprite width", width);
        height = EditorGUILayout.IntField("sprite height", height);
        if (GUILayout.Button("Create Padding"))
        {
            Color[] s = SeparateSprite();
            Debug.Log((size + padding * 2) * width);
            Debug.Log(s.Length /( (size + padding * 2) * width ));
            
            int h = s.Length / ((size + padding * 2) * width);
            Debug.Log(h);
            outTex = new Texture2D((size + padding*2) * width, h,TextureFormat.RGBA32,false);
            Debug.Log(outTex.width * outTex.height);
            Debug.Log(s.Length);
            outTex.SetPixels(s);
            outTex.Apply();
            if (s.Length == outTex.width * outTex.height)
            {
                byte[] pngData = outTex.EncodeToPNG();
                string filePath = EditorUtility.SaveFilePanel("Save Texture", "", texture.name + ".png", "png");
                File.WriteAllBytes(filePath, pngData);
                Debug.Log("Save at " + filePath);
            }
        };
    }
}