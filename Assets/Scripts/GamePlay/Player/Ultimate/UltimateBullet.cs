using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateBullet : MonoBehaviour {
    public float dmg;
    public float speed;

    public GameObject target;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 pos=target.transform.position;
        pos.x+=1;
        if (Vector2.Distance(transform.position, pos) >1)
            transform.position = Vector2.MoveTowards(transform.position, pos, speed * Time.deltaTime);
       else
            Destroy(gameObject);


	}
    void OnTriggerEnter2D(Collider2D target)
    {

        if (target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().getDamage(dmg);
            Destroy(gameObject);
        }
        if (target.tag == "Boss")
        {
            target.GetComponent<Boss>().ReciveDamage(dmg);
            Destroy(gameObject);
        }
    }

    
}
