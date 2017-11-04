using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using System;
public class UpgradePanel : AlphaScript
{
    public enum UpgradeKind
    {
        damage,MaxFill,FillRate,CoinPerHour,CoinIncrease
    }
    public ParticleSystem LevelUp;
    public UpgradeKind Type;
    public float Multi,baseCost;
    public RtlText Level, Cost,Title;
    public Image Currency;
    public Sprite Gem, Coin;
    public GameObject Lock;
    public Action OnClick;
    bool canBuy;
    Button btn;
    public UpgradePanel[] panels;
    public int cost;
	// Use this for initialization
	void Start () {
        btn= GetComponent<Button>();
        Lock.transform.GetChild(0).GetComponent<RtlText>().text = "Low Level";
        Repaint();
	}
	
	// Update is called once per frame
    public void Repaint()
    {
        string a = "";
        string b = "";
        switch (Type)
        {
            case UpgradeKind.damage:
                a = GameManager.Instance.upgradeLevel.DmgLevel.ToString();
                b = CostPerLvl((int)baseCost, Multi, GameManager.Instance.upgradeLevel.DmgLevel).ToString();
                if (GameManager.Instance.upgradeLevel.DmgLevel < GameManager.Instance.stateData.lvl)
                {
                    canBuy = true;
                    Lock.SetActive(false);
                }
                else
                {
                    canBuy = false;
                    Lock.SetActive(true);
                }

                Currency.sprite=Coin;

                cost = Int32.Parse(b);
                if (cost > GameManager.Instance.currencyData.Coin)
                    Cost.color = Color.red;
                else
                    Cost.color = Color.white;
                Title.text = GameManager.Language("قدرت حمله", "Attack", Title);
                break;

            case UpgradeKind.MaxFill:
                a = GameManager.Instance.upgradeLevel.MaxEnergyLVL.ToString();
                b = CostPerLvl((int)baseCost, Multi, GameManager.Instance.upgradeLevel.MaxEnergyLVL).ToString();
                if (GameManager.Instance.upgradeLevel.MaxEnergyLVL < GameManager.Instance.stateData.lvl)
                {
                    canBuy = true;
                    Lock.SetActive(false);
                }
                else
                {
                    canBuy = false;
                    Lock.SetActive(true);
                }

                Currency.sprite=Coin;

                cost = Int32.Parse(b);
                if (cost > GameManager.Instance.currencyData.Coin)
                    Cost.color = Color.red;
                else
                    Cost.color = Color.white;
                Title.text = GameManager.Language("مقدار مانا", "Max Mana", Title);

                break;

            case UpgradeKind.FillRate:
                a = GameManager.Instance.upgradeLevel.FillRateLVL.ToString();
                b = CostPerLvl((int)baseCost, Multi, GameManager.Instance.upgradeLevel.FillRateLVL).ToString();
                if (GameManager.Instance.upgradeLevel.FillRateLVL < GameManager.Instance.stateData.lvl)
                {
                    canBuy = true;
                    Lock.SetActive(false);
                }
                else
                {
                    canBuy = false;
                    Lock.SetActive(true);
                }

                Currency.sprite=Coin;

                cost = Int32.Parse(b);
                if (cost >GameManager.Instance.currencyData.Coin)
                    Cost.color = Color.red;
                else
                    Cost.color = Color.white;
                Title.text = GameManager.Language("سرعت مانا", "Mana Speed", Title);

                break;

           

           
        }
        a = GameManager.NumberPersian(a, Level);
        a = GameManager.Language(a + " " + "مرحله", "Level " + a, Level);
        Level.text = a;
        b = GameManager.NumberPersian(b, Cost);
        Cost.text = b;
    }
    public void Buy()
    {
        if (canBuy)
        {
            print("Buy");

            switch (Type)
            {
                case UpgradeKind.damage:
                    if (GameManager.Instance.currencyData.Coin >= cost)
                    {
                        GameManager.Instance.ChangeCoin(-cost);
                        GameManager.Instance.upgradeLevel.DmgLevel++;
                        GameManager.Instance.saveCharacter();
                        LevelUp.gameObject.SetActive(true);
                        LevelUp.Stop();
                        LevelUp.Play();
                    }
                    break;
                case UpgradeKind.MaxFill:
                    if (GameManager.Instance.currencyData.Coin >= cost)
                    {
                        GameManager.Instance.ChangeCoin(-cost);
                        GameManager.Instance.upgradeLevel.MaxEnergyLVL++;
                        GameManager.Instance.saveCharacter();
                        LevelUp.gameObject.SetActive(true);
                        LevelUp.Stop();
                        LevelUp.Play();
                    }
                    break;
                case UpgradeKind.FillRate:
                    if (GameManager.Instance.currencyData.Coin >= cost)
                    {
                        GameManager.Instance.ChangeCoin(-cost);
                        GameManager.Instance.upgradeLevel.FillRateLVL++;
                        GameManager.Instance.saveCharacter();
                        LevelUp.gameObject.SetActive(true);
                        LevelUp.Stop();
                        LevelUp.Play();
                    }
                    break;
            
                
            }
            playSound("Button");
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Repaint();
            }
        }
    }
    public int CostPerLvl(int B, float Multi, int lvl)
    {
        int r = (int)(B * (Mathf.Pow(Multi, lvl)));
        return r;
    }
    
}
