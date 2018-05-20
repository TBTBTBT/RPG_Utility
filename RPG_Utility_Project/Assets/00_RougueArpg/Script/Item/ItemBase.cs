using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
    public ItemType _type;
    protected int _num = 0;
    protected int _id = 0;
    private void OnTriggerStay2D(Collider2D c)
    {
        if(c.tag == "Player"){
            c.transform.root.GetInterface<ICanPickItem>((picker) =>
            {
                picker.GetItem(_type, _id,_num);
                Destroy(this.gameObject);
            });
        }
    }
    public abstract void Spawn(ItemType type, int id, int num);
}
