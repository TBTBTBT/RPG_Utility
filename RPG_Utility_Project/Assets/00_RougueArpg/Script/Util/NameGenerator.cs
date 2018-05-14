using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static string randomstrings = 
        "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワンー";

    public static string Generate(int length)
    {
        string name = "";
        for (int i = 0; i < length; i++)
        {
            int r = Random.Range(0, randomstrings.Length);
            name += randomstrings[r];
        }

        return name;
    }

    public static string Generate()
    {
        return Generate(Random.Range(2, 6));
    }
}
