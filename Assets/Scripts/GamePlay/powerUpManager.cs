using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerUpManager : AlphaScript
{
    public static powerUpManager Instance;
    public GameObject Gurdian;
    public GameObject ManaPoint;
    public GameObject doubleCoin;
    public GameObject doubleEXP;

    public Slider CoinSlider,EXPSlider,DragonPotionSlider;

    public bool coinMulti,EXPMulti,superPower;
    public float MaxCoinTime, MaxExpTime, MaxsuperPowerTime;

    float Expt, CoinT, superPowerT;

    [Header("Chances to make PowerUp (By Order)")]



    [Header("Chance to Make PowerUp")]
    [Range(0, 100)]
    public int Chance;

    [Header("Chance to Make DoubleEXP")]
    [Range(0, 100)]
    public int DoubleEXPChance;


    [Header("Chance to Make DoubleCoin")]
    [Range(0, 100)]
    public int DoubleCoinChance;


    [Header("Chance to Make Gurdian")]
    [Range(0,100)]
    public int GurdianChance;


    [Header("SuperPower")]
    public float SuperChance,time; public GameObject SuperPower;

    float t;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.play)
        {
            t += Time.deltaTime;
            if (t >= time)
            {
                int a = Random.Range(0, 100);
                if (a <= SuperChance)
                    SpawnSuperPower();
                t = 0;
            }
            if (Expt > 0)
            {
                EXPMulti = true;
                Expt -= Time.fixedDeltaTime;
                EXPSlider.value = Expt;
            }
            else
            {
                EXPMulti = false;
                EXPSlider.gameObject.SetActive(false);
            }


            if (CoinT > 0)
            {
                coinMulti = true;
                CoinT -= Time.fixedDeltaTime;
                CoinSlider.value = CoinT;
            }
            
            else
            {
                coinMulti = false;
                CoinSlider.gameObject.SetActive(false);
            }
            if (superPowerT > 0)
            {
                superPower = true;
                superPowerT -= Time.fixedDeltaTime;
                // CoinSlider.value = CoinT;
                DragonPotionSlider.value = superPowerT;
            }
            else if (superPowerT <= 0 && superPower)
            {
                superPower = false;
                DragonPotionSlider.gameObject.SetActive(false);
            }
            else
            {
                PlayerController.Instance.EnergyBar.fillRect.GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void SpawnSuperPower()
    {
        if (!GamePlayManager.Instance.Tutorial && GamePlayManager.Instance.play)
            Instantiate(SuperPower, gamePlayUi.Instance.SuperPowerParent.transform).GetComponent<SuperPowerScript>().radius = Random.Range(-3, 3);
    }
    public void SuperPowerGet()
    {
        superPowerT = MaxsuperPowerTime;
        DragonPotionSlider.maxValue = MaxsuperPowerTime;
        DragonPotionSlider.gameObject.SetActive(true);
        PlayerController.Instance.EnergyBar.fillRect.GetComponent<Image>().color = new Color(93 / 255, 0, 255, 255);
        //slider

    }
    public void DoubleCoin()
    {
        CoinT = MaxCoinTime;
        CoinSlider.maxValue = MaxCoinTime;
        CoinSlider.value = CoinT;
        CoinSlider.gameObject.SetActive(true);

    }
    public void DoubleExp()
    {
        Expt = MaxExpTime;
        EXPSlider.maxValue = MaxExpTime;
        EXPSlider.value = Expt;
        EXPSlider.gameObject.SetActive(true);

    }
	
	// Update is called once per frame
    public void MakePowerUp(Vector2 Pos)
    {
        int a = Random.Range(0, 100);
        if (a <= Chance)
        {
            a=Random.Range(0, 100);
            if (a <= DoubleEXPChance)
                Instantiate(doubleEXP, Pos, Quaternion.identity);
            
            else if(a<=DoubleCoinChance)
                Instantiate(doubleCoin, Pos, Quaternion.identity);
            else if (a <= GurdianChance)
                Instantiate(Gurdian, Pos, Quaternion.identity);

        }
        Instantiate(ManaPoint, Pos, Quaternion.identity);
    }
}
