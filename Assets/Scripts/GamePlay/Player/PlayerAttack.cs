using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerAttack : AlphaScript
{

    public static PlayerAttack instance;

    [Header("Controller")]
    public Transform controllerParent;
    public GameObject controller;


    [Header("GamePlay")]
    public GameObject bullet;
    public Transform ShootPos;
    public Slider EnergyBar;
    public bool recharging;
    public float fireRate;
    public float maxEnergy;
    public float fillRate;
    public float costPerShoot;
    public float damage;
    protected Animator anim;
    float Fillspeed;
    protected float energy;
    Image energyBarSprite;
    public bool shootAllow=true;

   protected AudioSource audioSource;
    protected bool mainmenu;

    public void start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!mainmenu)
        {
            energy = maxEnergy;
            EnergyBar = PlayerController.Instance.EnergyBar;
            EnergyBar.maxValue = energy;
            EnergyBar.value = energy;
            anim = GetComponent<Animator>();
            Fillspeed = maxEnergy / fillRate;
            energyBarSprite = EnergyBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }
    }
    public void ReNewStates()
    {
        instance = this;
        mainmenu = GamePlayManager.Instance ? false : true;
        if (!mainmenu)
        {
            if (!GamePlayManager.Instance.Tutorial )
            {
                fillRate = fillRate - ((fillRate  * 0.02f) * GameManager.Instance.upgradeLevel.FillRateLVL);
                maxEnergy = maxEnergy + ((maxEnergy*0.2f) * GameManager.Instance.upgradeLevel.MaxEnergyLVL);
                damage = damage + ((damage *0.2f) * GameManager.Instance.upgradeLevel.DmgLevel);
            }
            else
            {
                damage = 15;
                fillRate = 0.5f;
            }
        }
    }
	
	// Update is called once per frame
    public void CheckPerFrame()
    {
        if (!mainmenu)
        {
            if (recharging)
            {
                //Particle.SetActive(true);
                if (energy < maxEnergy && GamePlayManager.Instance.play)
                {
                    energy += Fillspeed * Time.deltaTime;

                }
                else if (energy >= maxEnergy)
                {
                    energy = maxEnergy;
                   // Particle.SetActive(false);
                    recharging = false;


                }
            }


            
            EnergyBar.value = energy;
        }
        
    }
    public void CostEnergy()
    {
        if (!powerUpManager.Instance.superPower)
        {
            energy -= costPerShoot;
            if (energy <= costPerShoot)
            {
                energy = 0;
                recharging = true;
            }
        }
    }
    public void AddEnergy(float amount)
    {
        energy += amount;
    }
    public void ChargeTheEnergy()
    {
        energy = maxEnergy;
    }
    public void Allower()
    {
        shootAllow = true;
    }
    
    public void PlayFootStep()
    {
        int a = UnityEngine.Random.Range(0,4);
        playSound("FootStep " + a);
    }
}
