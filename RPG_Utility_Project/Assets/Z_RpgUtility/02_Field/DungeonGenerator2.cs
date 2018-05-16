using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RoomSettings
{
    [Header("エリアの分割数")]
    [Range(1, 10)]
    public int areaDevisionX;
    [Range(1, 10)]
    public int areaDevisionY;
    [Header("部屋の数")]
    [Range(1, 20)]
    public int roomNum;
    [Header("部屋の大きさ")]
    public Vector2Int minSize;

    public Vector2Int maxSize;
    //[Range(1, 10)]
    //public int maxWallThicknessInArea;

}
public enum RoomType{
    Maze,
    Room
}
public class DungeonGenerator2 : MonoBehaviour
{
    public static FieldInfo[,] Generate(RoomType roomType, RoomSettings room, Vector2Int size, Vector2Int center)
    {
        switch (roomType)
        {
            case RoomType.Maze:
                return GenerateMaze(size, center);

            case RoomType.Room:
                return GenerateRoom(room, size, center);
        }
        return null;
    }
    #region 穴掘り法

    static List<Vector2Int> _direction = new List<Vector2Int>(){
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(0,1)
        };
    static int _mazeSize = 4;

    //穴掘り法による生成
    public static FieldInfo[,] GenerateMaze(Vector2Int size, Vector2Int center)
    {
        FieldInfo[,] map = Init(size);
        //開始地点はcenter
        if (IsExistIndex<FieldInfo>(map, center))
        {
            CreateMaze(map, center);
        }


        //特殊処理


        return map;
    }

    //0 左 1 上 ...
    static void CreateMaze(FieldInfo[,] map, Vector2Int pos)
    {
        //int dir = beforeDir;
        /*
        if (length > 3)
        {
            length = 0;
            dir = UnityEngine.Random.Range(0, 4);
        }
        */
        //方向をランダムに
        Vector2Int[] direction = _direction.OrderBy((cur) => System.Guid.NewGuid()).ToArray().Clone() as Vector2Int[];

        //Vector2Int[] direction = _direction.ToArray();//.OrderBy((cur) => Guid.NewGuid()).ToArray();

        for (int i = 0; i < direction.Length; i++)
        {
            Vector2Int next = pos + direction[i] * _mazeSize;

            if (IsExistIndex<FieldInfo>(map, next))
            {


                if (!map[next.x, next.y].GetFieldState(FieldParam.IsPassable))
                {
                    for (int j = 0; j < _mazeSize; j++)
                    {
                        for (int x = 0; x < _mazeSize / 2; x++)
                        {
                            for (int y = 0; y < _mazeSize / 2; y++)
                            {
                                if (IsExistIndex<FieldInfo>(map, pos + direction[i] * j + new Vector2Int(x, y)))
                                    map[pos.x + direction[i].x * j + x, pos.y + direction[i].y * j + y].SetFieldState(FieldParam.IsPassable, true);
                            }
                        }

                    }

                    CreateMaze(map, next);

                    /// CreateMaze(map, next, UnityEngine.Random.Range(0, 4), length + 1);
                }
            }
        }
    }
    #endregion
    #region 分割法
    //部屋区切りによる生成
    public static FieldInfo[,] GenerateRoom(RoomSettings room, Vector2Int size, Vector2Int center)
    {
        FieldInfo[,] map = Init(size);
        Block[,] b = InitBlock(size);

        List<Area> area = DevideArea(size, room);

        CreateRoom(area,room);
        area.ForEach(a => { Debug.Log(a.start); });
        WriteToMap( map,area);
        CreateRoad(map, area);
        return map;
    }
    static Block[,] InitBlock(Vector2Int size){
        Block[,] area = new Block[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                area[x, y] = new Block();
            }
        }
        return area;
    }
    static void WriteToMap(FieldInfo[,] map,List<Area> area){
        area.ForEach((a) =>
        {
            for (int x = a.start.x; x < a.end.x;x ++){
                for (int y = a.start.y; y < a.end.y; y++)
                {
                    if (IsExistIndex<FieldInfo>(map,new Vector2Int(x, y)))
                    {
                        map[x, y].SetFieldState(FieldParam.IsPassable, true);
                    }
                }
            }

        });
    }
    static List<Area> DevideArea(Vector2Int size,RoomSettings room){
        List<Area> area = new List<Area>();
        int sx = size.x / room.areaDevisionX;
        int sy = size.y / room.areaDevisionY;
        for (int x = 0; x < room.areaDevisionX; x ++){
            for (int y = 0; y < room.areaDevisionY; y++)
            {

                area.Add(new Area() { start = new Vector2Int(sx * x, sy * y), end = new Vector2Int(sx * (x+1), sy * (y+1)) });
            }
        }
        /*
        List<int> x = new List<int>(){size.x};
        List<int> y = new List<int>(){size.y};
        for (int i = 0; i < room.areaDevideNum; i++){
            if(i%2 == 0){
                x.Add(x[0]/2);
            }else{
                
            }
        }*/
        return area;
    }
    static void CreateRoom(List<Area> area,RoomSettings room){
        area.ForEach((a) =>
        {
            Vector2Int size = new Vector2Int(Random.Range(room.minSize.x, room.maxSize.x), Random.Range(room.minSize.y, room.maxSize.y));
            a.start = new Vector2Int(Random.Range(a.start.x+1,a.end.x - size.x-1), Random.Range(a.start.y+1, a.end.y - size.y-1));
            a.end = a.start + size;
            a.center = a.start + new Vector2Int(size.x/2,size.y/2);
        });
    }
    static void CreateRoad(FieldInfo[,] map,List<Area> area){
        for (int i = 0; i < area.Count;i++){
            if (i < area.Count - 1)
            {
                AimTo(area[i].center, area[i + 1].center, (v) => { map[v.x, v.y].SetFieldState(FieldParam.IsPassable, true); });
            }
        }
    }
    //centerとcenterをつなぐ
    static void AimTo(Vector2Int start,Vector2Int end,System.Action<Vector2Int> cb){
        if(start != end){
            Vector2Int dir = end - start;
            dir = new Vector2Int((int)Mathf.Sign(dir.x), (int)Mathf.Sign(dir.y));
            if(start.x == end.x){
                dir.x = 0;
            }
            else if (start.y == end.y)
            {
                dir.y = 0;
            }
            else 
            {
                if (Random.Range(0, 2) < 1) dir.x = 0;
                else dir.y = 0;
            }
            AimTo(start + dir, end, cb);
                   
                    }
                    cb(start);
    }
    //ひとマスの情報
    public class Block{
        //public int area = 0;//エリア番号
        public bool isPassable = false;//通行情報

    }
    //エリア情報
    public class Area{
        public Vector2Int start;
        public Vector2Int center;
        public Vector2Int end;
    }

    #endregion
    #region 汎用
    static bool IsExistIndex<T>(T[,] array, Vector2Int index)
    {
        return array.GetLength(0) > index.x && array.GetLength(1) > index.y && index.x >= 0 && index.y >= 0;
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
    #endregion
}
