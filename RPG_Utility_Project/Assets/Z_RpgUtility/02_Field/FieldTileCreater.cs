using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldTileCreater : MonoBehaviour
{
    public Tilemap _tileGrid;

    public TileBase _floortile;
    public TileBase _walltile;
	// Use this for initialization
    void FieldUpdate()
    {
        _tileGrid.RefreshAllTiles();
        FieldManager field = FieldManager.Instance;
        for (int i = 0; i < field._width; i++)
        {
            for (int j = 0; j < field._height; j++)
            {
                if (field.GetFieldState(i, j, FieldParam.IsPassable))
                {
                    _tileGrid.SetTile(new Vector3Int(i, j, 0), _floortile);
                }
                else
                {
                    _tileGrid.SetTile(new Vector3Int(i, j, 0), _walltile);
                }
            }
        }

	}
	
	// Update is called once per frame
	void Awake () {
	    FieldManager field = FieldManager.Instance;
        field.OnInitField.AddListener(FieldUpdate);
    }
}
