using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : SingletonMonoBehaviourCanDestroy<UISpawner> {
    public GameObject _personal;

	
	// Update is called once per frame
    static public void Spawn(GameObject anchor,IParamater param) {
        if (Instance)
        {
            PersonalUI personal = Instantiate(Instance._personal, anchor.transform).GetComponent<PersonalUI>();
            personal.Init(param);
        }
	}
	
}
