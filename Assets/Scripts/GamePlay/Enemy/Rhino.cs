using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Enemy {
    public float Speed;
    bool charge;
	// Use this for initialization
	void Start () {

        start();
        StartCoroutine(Charge());
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        CheckPerFrame();
        if(charge)
        {
            Vector2 t = transform.position;
            t.x -= Speed * Time.fixedDeltaTime;
            transform.position = t;

        }
       else if(!charge)
            Move();
       else if (ready && !charge)
            StartCoroutine(Charge());
	}
    IEnumerator Charge()
    {
        yield return new WaitForSeconds(3);
        if (GamePlayManager.Instance.play)
        {
            anim.SetTrigger("Charge");
            charge = true;
            Invoke("chargeDeath", 1.5f);
        }
        else
            StartCoroutine(Charge());
    }
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.GetComponent<PlayerHealth>().die(null,true);
            Die();
            Speed = 0;

        }
    }
    void chargeDeath()
    {
        EnemiesController.Instance.Remove(gameObject);

        Destroy(gameObject);
    }
}
