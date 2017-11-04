using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCompelete : MonoBehaviour {
    public static MissionCompelete Instance;
    public Text t;
	// Use this for initialization
	void Start () {
        Instance = this;
	}
    public void Repaint(string a)
    {
        t.text = a;
        GetComponent<Animator>().SetTrigger("Enter");

    }
}
