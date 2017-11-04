using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy {

	// Use this for initialization
	void Start () {

        start();
	}
	
	// Update is called once per frame
    void Update()
    {
        CheckPerFrame();
        Move();
        //CheckForShoot();
    }
   
}
