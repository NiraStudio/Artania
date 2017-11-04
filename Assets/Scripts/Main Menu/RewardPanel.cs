using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RewardPanel : MonoBehaviour {
    public Reward[] rewards;

    public Button Btn;
    public Text AmountText;
    public Text time;
    public Image imag;

    public Sprite Coin, Gem, Meat;

    float current, Main;
    Reward R;
    Animator anim;
    DateTime d;
    int remainSeconds;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        checkForTime();
	}
	
	// Update is called once per frame
	void Update () {
        AmountText.text = current.ToString("##");
        if (current != Main)
            current = Mathf.MoveTowards(current, Main, (Main /2f) * Time.deltaTime);
        if (time.gameObject.activeInHierarchy)
        {
            remainSeconds = (int)(d - DateTime.Now).TotalSeconds;
            string t = "";
            t += (remainSeconds / 3600).ToString("00") + ":";
            remainSeconds -= (remainSeconds / 3600) * 3600;
            t += (remainSeconds / 60).ToString("00") + ":";
            t += (remainSeconds % 60).ToString("00");
            time.text = GameManager.NumberPersian(t,time);
            if (remainSeconds <= 0)
            {
                checkForTime();
            }
        }
    }
    public void ChooseReward()
    {
        R = rewards[UnityEngine.Random.Range(0, rewards.Length)];
        current = 0;
        Main = R.amount;
        switch (R.type)
        {
            case Reward.Type.Gem:
                imag.sprite = Gem;
                break;
            case Reward.Type.Coin:
                imag.sprite = Coin;
                break;
            
                break;
        }
        anim.SetTrigger("Enter");

    }
    void checkForTime()
    {
        DateTime a = GameManager.Instance.stateData.RewardTime;
        print(a);
        DateTime b = new DateTime(a.Year, a.Month, a.Day, 0, 0, 0);
        b = b.AddDays(1);
       // b=b.AddSeconds(1);
        if (DateTime.Now > b)
        {
            Btn.interactable = true;
            time.gameObject.SetActive(false);
        }
        else
        {
            d = b;
            Btn.interactable = false;
            time.gameObject.SetActive(true);
        }
    }
    public void Add(int MultiPly)
    {
        switch (R.type)
        {
            case Reward.Type.Gem:
                GameManager.Instance.ChangeGem((int)Main * MultiPly);
                break;
            case Reward.Type.Coin:
                GameManager.Instance.ChangeCoin((int)Main * MultiPly);
                break;
            
                break;
        }
        anim.SetTrigger("Idle");
        GameManager.Instance.stateData.RewardTime = DateTime.Now;
        GameManager.Instance.saveState();
        checkForTime();
    }
}
[System.Serializable]
public class Reward
{
    public enum Type
    {
        Gem,Coin,Exp
    }
    public string RName;
    public Type type;
    public int amount;

    public Reward()
    {
        RName = amount + " " + type;
    }
}
