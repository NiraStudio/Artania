using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FastScript : PlayerAttack {
    bool s;
	// Use this for initialization
    void Start()
    {
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
            entry.callback.AddListener((data) => { Shoot(true); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { Shoot(false); });
            trigger.triggers.Add(entry);
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckPerFrame();
        if (!mainmenu)
        {
            if (s)
            {
                if (GamePlayManager.Instance.play)
                    if (!recharging && shootAllow)
                    {
                        anim.SetBool("Attack", true);
                    }
                    else
                        anim.SetBool("Attack", false);

            }
            else
                anim.SetBool("Attack", false);
        }
	}
    public void Shoot(bool shoot)
    {
        s = shoot;
        
    }
    public void shoot()
    {
        Instantiate(bullet, ShootPos.transform.position, Quaternion.identity).GetComponent<Bullet>().dmg = damage;
        CostEnergy();

    }
}
