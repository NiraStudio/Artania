using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLineBoss: Boss {
    [Header("TwoLine Boss")]
    public GameObject Bullet;
    public float waitTime;

    int AttackTimes;
    
    float T;
    float maxHp;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Starter();
        maxHp = Hp;
        AttackTimes = Random.Range(4, 7);
        shieldImage.SetActive(true);

	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        GoToPos();
        HpChecker();
        if (GamePlayManager.Instance.play&&allow)
        {
            T += Time.deltaTime;
            if (T >= waitTime)
            {
                ChooseLines();

                anim.SetTrigger("Attack");
                T = -111111;
            }
        }
	}
    public void RestartTimer()
    {
        T = 0;
    }
    public void Throw()
    {
        Instantiate(Bullet, Lines[FirstLine].position, Quaternion.identity);
        Instantiate(Bullet, Lines[secondLine].position, Quaternion.identity);
        AttackTimes--;
        RestartTimer();
        if (AttackTimes == 0)
            StartCoroutine(deShield());
    }
    IEnumerator deShield()
    {
        allow = false;
        Shield = false;
        shieldImage.SetActive(false);

        anim.SetTrigger("Tired");
        yield return new WaitForSeconds(5.0f);

        if (Hp <= maxHp / 2)
            waitTime = 0.5f;
        Shield = true;
        shieldImage.SetActive(true);

        anim.SetTrigger("Idle");
        if(Hp>0.5f)
        allow = true;
        AttackTimes = Random.Range(4, 7);

        T = 0;
    }
    
}
