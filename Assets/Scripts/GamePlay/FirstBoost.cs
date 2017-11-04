using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoost : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeBossKiled()
    {
        
    }
    IEnumerator change()
    {
        ///splash
        yield return new WaitForSeconds(3);
        int a=GameManager.Instance.stateData.LastKilledBoss-Random.Range(2,4);
        GamePlayManager.Instance.bossKilledNumber = a;
        GameManager.Instance.ChangeCoin(-(a * 400));
        //desplash
    }
}
