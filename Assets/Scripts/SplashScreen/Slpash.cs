using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slpash : AlphaScript
{

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(0);
        loadingScreen.Instance.Show(GameManager.Instance.PageName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
