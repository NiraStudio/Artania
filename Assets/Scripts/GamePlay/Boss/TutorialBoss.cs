using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoss : Boss {

	// Use this for initialization
    public GameObject Bullet;
    [SerializeField]
    Transform ShootPos;
    bool shootAllow = false;


    GameObject Player;
    float T;
    int ExpDrain;

    float wt, MaxHp;

    // Use this for initialization
    void Start()
    {
        MaxHp = Hp;
        wt = (Hp / MaxHp);
        Starter();
        Player = GameObject.FindWithTag("Player");
        ExpDrain = GamePlayManager.Instance.Exp / 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GoToPos();
        HpChecker();
        T += Time.deltaTime;
        if (T >= wt && shootAllow&&GamePlayManager.Instance.play)
        {
            //animation
            anim.SetTrigger("Attack");
            shootAllow = false;
        }
    }
    public void shoot()
    {
        GameObject g = Instantiate(Bullet, ShootPos.position, Quaternion.identity); g.GetComponent<Bullet>().ChangeTarget(Player.transform); ;
        wt = (Hp / MaxHp)*1.5f;
        T = 0;

    }
    public void Allower()
    {
        shootAllow = true;
    }
}
