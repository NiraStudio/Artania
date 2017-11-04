using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPPrefabScript : AlphaScript
{
    public float speed;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 1);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, transform.position+transform.up, speed * Time.deltaTime);
	}
}
