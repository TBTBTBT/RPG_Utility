using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviourCanDestroy<ItemManager>
{
    public GameObject _coin;
    public GameObject _item;
    public GameObject _weapon;

    static public void Spawn(Vector2 pos,ItemType type,int id,int num){
        if(Instance){
            ItemBase item = null;
            switch(type){
                case ItemType.Coin:
                    item = Instantiate(Instance._coin, pos, Quaternion.identity).GetComponent<ItemBase>();
                    break;
                case ItemType.Item:
                    item = Instantiate(Instance._item, pos, Quaternion.identity).GetComponent<ItemBase>();
                    break;
                case ItemType.Weapon:
                    item = Instantiate(Instance._weapon, pos, Quaternion.identity).GetComponent<ItemBase>();
                    break;
            }
            if(item!= null)
            item.Spawn(type, id, num);
        }
    }
    static public void Spawn(Vector2 pos, List<ItemTable> table)
    {

        if (table.Count > 0)
        {
            ItemTable s = table[Random.Range(0, table.Count)];
            Spawn(pos, s.type, s.id, Random.Range(s.minnum, s.maxnum));
        }
    }
}
