using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlartSystem : MonoBehaviour {
    public static AlartSystem Instance;
    public GameObject[] Lines;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetOn(int lineNumber,bool active)
    {
        Lines[lineNumber].SetActive(active);
    }
    public void SetOff()
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            Lines[i].SetActive(false);
        }
    }
    
}
