using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrongScript : PlayerAttack {
    public float MaxStrength;

    float perSecPowerUp=3f;
    float Multiply=1;
    bool shootCount;
	// Use this for initialization
	void Start () {
        ReNewStates();
        start();
        if (!mainmenu)
        {
            controllerParent = gamePlayUi.Instance.ControllerParent;
            GameObject g = Instantiate(controller, controllerParent);
            RectTransform rect = g.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = Vector2.zero;
            EventTrigger trigger = g.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { PowerUp(); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { Shoot(); });
            trigger.triggers.Add(entry);
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        CheckPerFrame();
        if (!mainmenu)
            anim.SetBool("Run", GamePlayManager.Instance.Run);
        if (shootCount)
        {
            if (Multiply < MaxStrength)
                Multiply += perSecPowerUp * Time.deltaTime;

            else
                Multiply = MaxStrength;
        }
    }
    void PowerUp()
    {
        if (!recharging && GamePlayManager.Instance.play&&shootAllow)
        {
            shootCount = true;
            shootAllow = false;
            anim.SetTrigger("Attack");

            anim.SetBool("Release", false);
        }
    }
    void Shoot()
    {
        shootCount = false;
        anim.SetBool("Release", true);

    }
    public void shoot()
    {
        if (GamePlayManager.Instance.play)
        {
            Bullet b = Instantiate(bullet, ShootPos.position, Quaternion.Euler(0, 0, -86.7f)).GetComponent<Bullet>(); b.dmg = damage * Multiply; if (Multiply >= 2.5f) { b.AttackerType = Bullet.attackerType.Strong; b.GetComponent<SpriteRenderer>().color = Color.red; } 
            Multiply = 1;
            CostEnergy();
        }
    }
   
}
