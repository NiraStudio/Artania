using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class EntroText : MonoBehaviour {
    public TextEntro[] Texts;
    public RtlText baseText;
    float t;
    int index=0;
	// Use this for initialization
	void Start () {
        t = Texts[0].time;
        StartCoroutine(changeText());		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator changeText()
    {
        yield return new WaitForSeconds(t);
        baseText.text = Texts[index].text;
        index++;
        if (index >= Texts.Length)
            loadingScreen.Instance.Show("Play");
        else
        {
            t = Texts[index].time;
            StartCoroutine(changeText());
        }
    }
}

[System.Serializable]
public struct TextEntro
{
    public string text;
    public float time;
}
