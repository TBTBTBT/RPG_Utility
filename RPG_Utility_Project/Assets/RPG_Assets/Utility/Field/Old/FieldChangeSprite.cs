using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldChangeSprite : MonoBehaviour
{
    public SpriteRenderer _floorrenderer;
    public List<SpriteRenderer> _walls;
    public List<Sprite> _sprites;
	// Use this for initialization
	void Start ()
	{
	    SetSprites();

        FieldManager field = FieldManager.Instance;
	    Vector2Int pos = field.PositionToIndex(transform.position);
        List<bool> isPassable = new List<bool>();
        // int ind = 0;
	    for (int j = -1; j < 2; j++)
	    {
            for (int i = -1; i < 2; i++)
	        {
	        
                if(j!=0||i!=0)
	            isPassable.Add(field.IsFieldPassable(pos+new Vector2Int(i,j)));
                //ind++;
	        }
	    }

	    bool my = field.IsFieldPassable(pos);
        Debug.Log(my);
        if (my)
	    {
	        WallDisable();
	        for (int i = 0; i < 8; i++)
	        {
	            SetWallActive(i, !isPassable[i]);
	        }
            SetWallDisable(1,0,2);
	        SetWallDisable(3, 0, 5);
	        SetWallDisable(4, 2, 7);
	        SetWallDisable(6, 5, 7);
        }
	    else
	    {
	        WallDisable();
            _floorrenderer.sprite = _sprites[1];
	       
        }
    }

    void SetWallDisable(int ifActive,int d1,int d2)
    {
        if (_walls[ifActive].gameObject.activeSelf)
        {
            SetWallActive(d1,false);
            SetWallActive(d2,false);
        }
    }
    void WallDisable()
    {
        for (int i = 0; i < _walls.Count; i++)
        {
            SetWallActive(i, false);
        }
    }
    void SetWallActive(int ind,bool flag)
    {
        if (ind >= 0 && _walls.Count > ind)
        {
            _walls[ind].gameObject.SetActive(flag);
        }
    }
    void SetSprites()
    {
        if (_sprites.Count > _walls.Count)
        {
            _floorrenderer.sprite = _sprites[0];
            for (int i = 0; i < _walls.Count; i++)
            {
                _walls[i].sprite = _sprites[i + 2];
            }
        }
    }

}
