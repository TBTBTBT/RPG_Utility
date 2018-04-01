using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationManager : MonoBehaviour
{
   // private Animator anim;
	[Header("ステートを持つスクリプトと紐付け")]
	public CharacterBase character;
	[Header("テクスチャのファイルパス(数字は除く)")]
	public string texturePath;
	private SpriteRenderer render;
	int _animTime = 0;
	int _animTimeMax = 24;
	int _changeFrame = 6;
	[Header("1アニメーションあたりの枚数")]
	public int _animationFrameNum = 3;
	[Header("アニメーション番号設定")]
	public List<AnimationNode> nodes = new List<AnimationNode> ();
	private List<Sprite> _sprites = new List<Sprite>();
	int nowSpriteNumber = -1;
	int nowPlayAnimationNumber = 0;
	int direction = 0;
    // Use this for initialization
    void Start ()
	{
	   // anim = GetComponent<Animator>();
		character.OnChangeBehaviourState.AddListener(ChangeState);
		character.OnChangeDirectionState.AddListener(ChangeDirection);
		render = GetComponent<SpriteRenderer>();
		LoadSprite();
	}

	/// <summary>
	/// 状態が変化した
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="flag">If set to <c>true</c> flag.</param>

	void ChangeState(string state,bool flag){
		int i = 0;
		Debug.Log (state);
		foreach (var n in nodes) {
			if (n.nodeName == state && n.toggle == flag) {
				nowPlayAnimationNumber = i;
			}
			i++;
		}
		
	}

	/// <summary>
	/// 向きが変化した
	/// </summary>
	void ChangeDirection(int d){
		direction = d;
	}
	// Update is called once per frame
	int ChangeAnimationTime(){
		_animTime++;
		if (_animTime >= _animTimeMax) {
			_animTime = 0;
		}
		int ret = _animTime / _changeFrame;
		return ret;
	}
	void Update () {
		


	}
	void LateUpdate(){
		//render.sprite = 
		int spriteNum = ChangeAnimationTime ();
		ChangeSprite(nodes[nowPlayAnimationNumber].GetSpriteNum(spriteNum),direction);
	}
	void LoadSprite()
	{
		_sprites.AddRange(LoadSpriteFromResource(texturePath,0));

	}

	public Sprite[] LoadSpriteFromResource(string fileName, int num)
	{
		string numString = num < 10 ? "0" + num.ToString() : num.ToString();
		Sprite[] sprites = Resources.LoadAll<Sprite>(fileName+numString);
		return sprites;
	}
	protected Sprite GetSprite(int i)
	{
		//Debug.Log(_sprites.Count);
		if (i >= 0 && i < _sprites.Count)
			return _sprites[i];
		return null;
	}
	public void ChangeSprite(int i,int d)
	{
		if (i >= 0)
		{
			int sn = i + d * _animationFrameNum;
			if (nowSpriteNumber != sn)
			{

				render.sprite = GetSprite(sn);
				nowSpriteNumber = sn;

			}
		}

	}


}
[System.Serializable]
public class AnimationNode{
	public string nodeName;
	public bool toggle;
	public List<int> SpriteNum = new List<int>();
	public int GetSpriteNum(int i){
		if (i >= 0 && i < SpriteNum.Count) {
			return SpriteNum[i];
		}
		return 0;
	}
}
/*
    public int GetAnimationNum(int i)
    {
        switch (i)
        {
            case 0:
                return (int)Mathf.Floor(hairnum);
                break;
            case 1:
                return (int)Mathf.Floor(headnum);
                break;
            case 2:
                return (int)Mathf.Floor(bodynum);
                break;
            case 3:
                return (int)Mathf.Floor(legnum);
                break;
        }

        return 0;
    }
*/