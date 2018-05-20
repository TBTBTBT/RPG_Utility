using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator3 : MonoBehaviour {

    static public FieldInfo[,] Generate(Vector2Int size){
        
        return Glassfield(size);
    }
    static FieldInfo[,] Glassfield(Vector2Int size){
        FieldInfo[,] map = Init(size);
        CreateRoad(map, size,Random.Range(0,100f),3);
        CreateRoad(map, size, Random.Range(0, 100f), 2);
        return map;
    }
    static void CreateRoad(FieldInfo[,] map,Vector2Int size,float seed,int width){
        for (int x = 1; x < size.x-1; x++)
        {
            for (int y = 1; y < size.y-1; y++)
            {
                float f = Mathf.PerlinNoise((y+seed) / 25f,y / 25f) ;
                float w = Mathf.PerlinNoise((y + seed) / 10f, y / 10f)+0.5f;
                int road = (int)(f * size.x);
                bool isRoad = x > road - width*w && x < road + width*w;
                if(isRoad)
                map[x, y].SetFieldState(FieldParam.IsPassable, isRoad);

            }
        }
        //List<Vector2Int> mountainCenter = new List<Vector2Int>();
        //mountainCenter.Add(new Vector2Int(Random.Range(0,size.x),Random.Range(0, size.y)));
    }

    static FieldInfo[,] AllPassable(Vector2Int size){
        FieldInfo[,] map = new FieldInfo[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map[x, y] = new FieldInfo();

            }
        }
        for (int x = 1; x < size.x-1; x++)
        {
            for (int y = 1; y < size.y-1; y++)
            {
                map[x, y].SetFieldState(FieldParam.IsPassable, true);
            }
        }
        return map;
    }
    static FieldInfo[,] Init(Vector2Int size)
    {
        FieldInfo[,] map = new FieldInfo[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map[x, y] = new FieldInfo();
            }
        }
        return map;
    }
}
