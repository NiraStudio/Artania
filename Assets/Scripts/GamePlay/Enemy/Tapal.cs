using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapal : Enemy {

	// Use this for initialization
	void Start () {
        start();
        transform.SetParent(null);
        ready = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x - Camera.main.transform.position.x < -12)
            chargeDeath();
        CheckPerFrame();
	}
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.GetComponent<PlayerHealth>().die(null, true);
            Die();

        }
    }
    void chargeDeath()
    {
        EnemiesController.Instance.Remove(gameObject);

        Destroy(gameObject);
    }
}
