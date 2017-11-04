using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShootEnemy : AlphaScript {
    public Transform shootPos;
    public GameObject bullet;
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    public void shoot()
    {
        Instantiate(bullet, shootPos.position, Quaternion.identity);
    }
    public void Die()
    {
        EnemiesController.Instance.Remove(gameObject);
        Destroy(gameObject);
    }
}
