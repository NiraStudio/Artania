using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeatPanel : AlphaScript
{
    public Text CoinT;
    public Text MeatT;

    float Current, main;
    [SerializeField]
    Animator anim;

    // Use this for initialization

    void Start()
    {
        // CheckForMeat();
    }
    // Update is called once per frame
    void Update() {
        CoinT.text = Current.ToString();
        if (Current != main)
            Current = (int)Mathf.MoveTowards(Current, main, (main / 1.5f) * Time.deltaTime);
    }
    public void Repaint(float amount, int Meat)
    {
        anim.SetTrigger("Enter");
        Current = 0;
        main = amount;
        MeatT.text = "X " + Meat + " Meats Used";
    }
    public void Add(int MultiPly)
    {
        playSound("Button");

        GameManager.Instance.ChangeCoin((int)main * MultiPly);
        PlayerPrefs.SetInt("Meat", 0);
        anim.SetTrigger("Idle");
    }
}
/*
  public void CheckForMeat()
    {
        int meats = GameManager.Instance.currencyData.Meat;
        if (meats > 0&&PlayerPrefs.GetInt("Meat")==1)
        {
            DateTime t = GameManager.Instance.stateData.QuitTime;
            int sec = (int)(DateTime.Now - t).TotalSeconds;
            int Hours = sec / 3600;
            if (Hours > 0)
            {
                if (Hours == meats)
                {
                    GameManager.Instance.ChangeMeat(-meats);
                    Repaint(meats * MPH(), meats);
                }
                else if (Hours > meats)
                {
                    GameManager.Instance.ChangeMeat(-meats);
                    Repaint(meats * MPH(), meats);
                }
                else if (Hours < meats)
                {
                    GameManager.Instance.ChangeMeat(-Hours);
                    Repaint(Hours * MPH(), Hours);
                }
            }
            else
            {
                PlayerPrefs.SetInt("Meat", 0);

            }
        }
    }

    public int MPH()
    {
        return (int)(100 * (Mathf.Pow(1.5f, GameManager.Instance.stateData.CoinPerHourLVL)));
    }*/

