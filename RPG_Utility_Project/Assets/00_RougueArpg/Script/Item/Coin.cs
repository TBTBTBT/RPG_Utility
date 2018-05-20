using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ItemBase {
	// Use this for initialization
	void Start () {
        _type = ItemType.Coin;
	}
    public override void Spawn(ItemType type,int id,int num){
        _num = num;

    }

}
