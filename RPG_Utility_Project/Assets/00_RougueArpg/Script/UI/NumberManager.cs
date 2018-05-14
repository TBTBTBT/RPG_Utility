using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NumberManager : MonoBehaviour {
    public List<TextMesh> text;

    public void ChangeText(string num){
        text.ForEach(tex =>{ tex.text = num; });
    }
}
