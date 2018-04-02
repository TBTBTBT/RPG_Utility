using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Animator))]

public class PlayerTransformAnimationManager : MonoBehaviour
{
   // private Animator anim;
	[Header("Walkアニメーションスクリプトと紐付け")]
	public PlayerSpriteAnimationManager leg;
//    [Header("武器所持時アタックアニメーション")]
    public CharacterBase character;
    void LateUpdate()
	{
	   // int num =
	    float y = Mathf.Abs(Mathf.Sin(leg.GetAnimationTime()*2*Mathf.PI)) * 0.08f;
        transform.localPosition = new Vector3(
            transform.localPosition.x, 
            y, 
            transform.localPosition.z);

	}



}
