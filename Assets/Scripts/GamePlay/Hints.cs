using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPersian.Components;

public class Hints : MonoBehaviour {
    [System.Serializable]
    public class hint
    {
        public string English, Persian;
        public string GiveText()
        {
            string a = "";
            if (PlayerPrefs.GetString("Language") == "Persian")
                a = Persian;
            else
                a = English;
            return a;
        }
    }

    public hint[] hints;

    RtlText t;
	// Use this for initialization
	void Start () {
        t = GetComponent<RtlText>();
        t.text = hints[Random.Range(0, hints.Length)].GiveText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
