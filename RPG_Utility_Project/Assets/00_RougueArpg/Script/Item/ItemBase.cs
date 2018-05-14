using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {
    enum Type{
        Coin,
        Item,
        Weapon,
        Equip
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D c)
    {
        if(c.tag == "Player"){
            Destroy(this.gameObject);
        }
    }
}
