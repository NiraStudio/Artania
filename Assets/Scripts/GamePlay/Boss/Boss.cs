using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : AlphaScript {

    public enum Type
    {
        Common,
        Rare,
        Legendary
    }

    public static Boss Instance;

    public Sprite icon;
    public string BossName;
    public Type BossType;
    public bool Shield;
    public bool Tutorial;
    public GameObject[] enemies;
    public Slider HpBar;
    public Vector3 startPos;
    public Color lightColor;
    public float Hp,ArmHp;
    public float exp,ArmyEXP;
    public int coinAmount,ArmyCoinAmount; 
    protected Transform[] Lines;
    protected int FirstLine, secondLine;
    protected bool allow = true;
    protected Animator anim;
    protected GameObject shieldImage;
    GameObject coinP, EXPP;
    int notChoosenLine=0;

    bool doubleCoin, DoubleEXP;
    float t;
	// Use this for initialization
    public void Starter()
    {
        Instance = this;
        doubleCoin = GameManager.Instance.currencyData.IsDoubleCoin;
        DoubleEXP = GameManager.Instance.currencyData.IsDoubleExp;
        anim = GetComponent<Animator>();
        coinP = (GameObject)Resources.Load("Prefabs/Coin", typeof(GameObject));
        EXPP = (GameObject)Resources.Load("Prefabs/EXPText", typeof(GameObject));
        Lines = new Transform[3];
        for (int i = 0; i < 3; i++)
            Lines[i] = Camera.main.transform.parent.GetChild(i);
        HpBar.maxValue = Hp;
        shieldImage = gamePlayUi.Instance.shieldImage;
        
    }
    public void AlartOff()
    {
        AlartSystem.Instance.SetOff();
    }
    public void ChooseLines()
    {
        bool a = true;
        while (a)
        {
            FirstLine = Random.Range(0, 3);

            do
            {
                secondLine = Random.Range(0, 3);
            } while (secondLine == FirstLine);
            if (secondLine == notChoosenLine || FirstLine == notChoosenLine)
                a = false;
        }


        for (int i = 0; i < Lines.Length; i++)
            if (FirstLine != i && secondLine != i)
                notChoosenLine = i;

        AlartSystem.Instance.SetOn(FirstLine, true);

        AlartSystem.Instance.SetOn(secondLine, true);

    }
    public void GoToPos()
    {
        if (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, Vector2.zero, 100 * Time.deltaTime);
        }
    }
    public void HpChecker()
    {
        HpBar.value=Hp;
        if (Hp <= 0)
            die();
    }
    public void GoldBrust()
    {
        allow = false;
        if (!Tutorial)
        {
            float value = coinAmount * (PlayerHealth.Instance.coinMulti + (powerUpManager.Instance.coinMulti ? 1 : 0+(doubleCoin?1:0)));
            print("gold = "+value);
            value = value / 5;
            for (int i = 0; i < 5; i++)
            {
                Vector2 a = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                a += Random.insideUnitCircle * 20;
                GameObject g = Instantiate(coinP, a, Quaternion.identity);
                g.transform.SetParent(gamePlayUi.Instance.gameObject.transform);
                g.GetComponent<RectTransform>().localScale = Vector3.one * 0.15f;
                g.GetComponent<CoinScript>().value = value;
                g.transform.SetParent(gamePlayUi.Instance.CoinParent.transform);
            }
            
        }
    }
    public void ReciveDamage(float amount)
    {
        if (!Shield)
        {
            Hp -= amount;
            anim.SetTrigger("Hit");
        }
    }
    public void die()
    {
        allow = false;
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("LookAtBoss");
        GetComponent<BoxCollider2D>().enabled = false;
        Hp = 0.01f;
        GamePlayManager.Instance.BossDie();
        MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.bossKill, 1);
        GamePlayManager.Instance.addEXP((int)(exp *( PlayerHealth.Instance.EXPMulti + (powerUpManager.Instance.EXPMulti ? 1 : 0 + (DoubleEXP ? 1 : 0)))));
        anim.SetTrigger("Die");
    }
    public void Destroy()
    {
        shieldImage.SetActive(false);
        
        Destroy(gameObject);
    }
    public void RenewState()
    {
        Hp = Hp + ((Hp * 0.5f) * GamePlayManager.Instance.bossKilledNumber);
        float a =(coinAmount + (coinAmount * (0.1f * GamePlayManager.Instance.bossKilledNumber))) + (coinAmount * (0.25f * (int)(GameManager.Instance.stateData.lvl / 8)));
        coinAmount = (int)a;
    }

    public void Shake()
    {
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("EarthShake");
    }
    public void playSound(string soundName){
        AudioManager.Instance.PlaySound(soundName);
    }
}
