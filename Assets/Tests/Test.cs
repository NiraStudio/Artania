using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(2f);
        DateTime a = GameManager.Instance.stateData.RewardTime;
        print(a);
        DateTime b = new DateTime(a.Year, a.Month, a.Day , 0, 0, 0);
        b = b.AddDays(1);
        print(b + " changed Date");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Hello()
    {
        print("Hello");
    }
    void Bye(Action t)
    {
        print("Bye");
        t();
    }
}
