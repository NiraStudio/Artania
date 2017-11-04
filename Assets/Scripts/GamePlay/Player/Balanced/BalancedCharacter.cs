using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalancedCharacter : PlayerAttack {
	// Use this for initialization
    void Awake()
    {
        
    }
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
            entry.callback.AddListener((data) => { Shoot((BaseEventData)data); });
            trigger.triggers.Add(entry);

        }

    }
	// Update is called once per frame
    void FixedUpdate()
    {
        CheckPerFrame();
        if (!mainmenu)
            anim.SetBool("Run", GamePlayManager.Instance.Run);

    }
    public void Shoot(BaseEventData data)
    {
        if (GamePlayManager.Instance.play&& energy>=costPerShoot)
        {

            if (!recharging && shootAllow)
            {
                anim.SetTrigger("Attack");
                shootAllow = false;

            }
        }
    }
    public void shoot()
    {
       // audioSource.clip = ShootSound;
        Instantiate(bullet, ShootPos.transform.position, Quaternion.identity).GetComponent<Bullet>().dmg = damage;
        CostEnergy();

    }
    public void StopPlaying()
    {
        
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
