using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateScript : PlayerAttack {

    GameObject aim;
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
            for (int i = 0; i < g.transform.childCount; i++)
                g.transform.GetChild(i).GetComponent<UltimateBTN>().ultimate = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
        CheckPerFrame();
        if (!mainmenu)
            anim.SetBool("Run", GamePlayManager.Instance.Run);
	}
    public void Shoot(GameObject pos)
    {
        if (!recharging && GamePlayManager.Instance.play&&shootAllow)
        {
            aim = pos;
            anim.SetTrigger("Attack");
            shootAllow = false;

        }
    }
    public void shoot()
    {
        Bullet b = Instantiate(bullet, ShootPos.position, Quaternion.identity).GetComponent<Bullet>(); b.ChangeTarget( aim.transform); b.dmg = damage;
        CostEnergy();
    }
}
