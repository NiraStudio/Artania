using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : AlphaScript
{
    public float speed;
    public float value;
    public bool EXP;
    Transform p;
	// Use this for initialization
	void Start () {
        p = gamePlayUi.Instance.Coin.transform;
        
        speed = Vector2.Distance(p.position, transform.position) / 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector2.Distance(transform.position,p.position)<3f)
        {
            MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.coin,(int)value);

            GamePlayManager.Instance.AddCoin(value);
            print("CoinAdded");
            Destroy(gameObject);
        }
        else if (transform.position != p.position)
            transform.position = Vector2.MoveTowards(transform.position, p.position, speed * Time.deltaTime);
	}
}
