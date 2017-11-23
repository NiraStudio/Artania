using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSceneBehavior : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        loadingScreen.Instance.Disapear();
        yield return new WaitForSeconds(6);
        loadingScreen.Instance.Show("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
