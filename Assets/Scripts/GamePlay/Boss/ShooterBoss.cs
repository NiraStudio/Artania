﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBoss : Boss {


    [Header("Shooter Boss")]
    public GameObject Bullet;
    [SerializeField]
    Transform ShootPos;

    [SerializeField]
    float waitTime,maxAttack,MinAttack;

     
    GameObject Player;
    float T,maxhp;
    int Times;
    // Use this for initialization
    void Start() {
        Starter();
        Times = (int)Random.Range(MinAttack, maxAttack);
        Player = GameObject.FindWithTag("Player");
        shieldImage.SetActive(true);
        maxhp = Hp;

    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        GoToPos();
        HpChecker();
        if (Shield&&GamePlayManager.Instance.play&&allow)
        {
            T += Time.deltaTime;
            if (T >= waitTime)
            {
                anim.SetTrigger("Attack");
                T = -111111;
            }
        }
	}
    IEnumerator Deshield()
    {
        shieldImage.SetActive(false);
        Shield = false;
        anim.SetTrigger("Tired");
        yield return new WaitForSeconds(5);
        if (allow) 
        anim.SetTrigger("Idle");
        Times = (int)Random.Range(MinAttack, maxAttack);
        Shield = true;
        shieldImage.SetActive(true);

        if(Hp<=maxhp/2&&waitTime>0.3f)
        {
            waitTime = 0.3f;
            maxAttack *= 2;
            MinAttack *= 2;
        }
        
    }
    public void Shoot()
    {
        Instantiate(Bullet, ShootPos.position, Quaternion.identity).GetComponent<Bullet>().ChangeTarget(Player.transform);
        Times--;
        T = 0;
        if (Times == 0)
            StartCoroutine(Deshield());
    }
}
