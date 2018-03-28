using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 動的生成するスプライトを管理するクラス
/// Animationをしようする && スプライトの差し替えが必要である
/// 場合、動的生成する
/// 今のところ動的生成する必要があるのは
/// プレイヤー
/// </summary>
public class SpriteManager : SingletonMonoBehaviourCanDestroy<SpriteManager> {
    Dictionary<string,Sprite> _sprites = new Dictionary<string, Sprite>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
