using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UPersian.Components;
public class IdlePanel : MonoBehaviour {
    public static IdlePanel Instance;
    public RtlText CPSText, StorageText, CPSBtnText, StorageBtnText, AmountText,TripleAmountText;
    public Button StorageUpgrade, CPSUpgarde,CalmBtn;
    
    public ObscuredInt storageLevel, coinPerSecLevel,storage;
    ObscuredInt amount;
    ObscuredFloat CPS;
    
    Animator anim;
	// Use this for initialization
	void Start () {
        anim=GetComponent<Animator>();
        CalmBtn.onClick.AddListener(calm);
        Instance = this;
        coinPerSecLevel = GameManager.Instance.stateData.CoinPerSecLevel;
        storageLevel = GameManager.Instance.stateData.coinerStorageLevel;
        CPS=calculatePerSec(coinPerSecLevel);
        storage= CalculateStorage(storageLevel);

	}
	
	// Update is called once per frame
    IEnumerator check()
    {
        CurrentAmount();
        if (amount >= 1)
            CalmBtn.interactable = true;
        else
            CalmBtn.interactable = false;

        AmountText.text = GameManager.NumberPersian(amount.ToString(), AmountText);
        TripleAmountText.text = GameManager.NumberPersian((amount*3).ToString(), TripleAmountText);

        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(check());

    }
    public void renewTexts(){


        string persian=GameManager.Instance.stateData.CoinPerSecLevel<7? "\n" + calculatePerSec(coinPerSecLevel+1 ) + " مرحله بعد ":"";
        string english = GameManager.Instance.stateData.CoinPerSecLevel < 7 ? "\n" + " Next Level " + calculatePerSec(coinPerSecLevel + 1) : "";


        CPSText.text = GameManager.Language(CPS + " سکه در ثانیه " +persian  , " Coin Per Second " + CPS + english,CPSText);

        persian = GameManager.Instance.stateData.coinerStorageLevel < 7 ? "\n" + CalculateStorage(storageLevel + 1) + " مرحله بعد " : "";
        english = GameManager.Instance.stateData.coinerStorageLevel < 7 ? "\n" + " Next Level " + CalculateStorage(storageLevel + 1) : "";

        StorageText.text = GameManager.Language(storage + " حداکثر مخزن " + persian, " Max Storage " + storage + english,StorageText);


        if (GameManager.Instance.currencyData.Gem >= CalculateCPSPrice(coinPerSecLevel)&&GameManager.Instance.stateData.CoinPerSecLevel<7)
            CPSUpgarde.interactable = true;
        else
            CPSUpgarde.interactable = false;

        if (GameManager.Instance.currencyData.Gem >= CalculateStoragePrice(coinPerSecLevel)&&GameManager.Instance.stateData.coinerStorageLevel<7)
            StorageUpgrade.interactable = true;
        else
            StorageUpgrade.interactable = false;

        CPSBtnText.text = GameManager.Instance.stateData.CoinPerSecLevel < 7 ? GameManager.NumberPersian(CalculateCPSPrice(coinPerSecLevel).ToString(), CPSBtnText) : GameManager.Language("مرحله آخر","Max Level", CPSBtnText);
        StorageBtnText.text = GameManager.Instance.stateData.coinerStorageLevel < 7 ? GameManager.NumberPersian(CalculateCPSPrice(storageLevel).ToString(), CPSBtnText) : GameManager.Language("مرحله آخر", "Max Level", StorageBtnText);
        AmountText.text = GameManager.NumberPersian(amount.ToString(), AmountText);
    }
    public void Enter()
    {
        renewTexts();
        StartCoroutine(check());
        anim.SetTrigger("Enter");
    }
    void calm()
    {
        GameManager.Instance.ChangeCoin(amount);
        amount = 0;
        GameManager.Instance.stateData.GoldGet = DateTime.Now;
        GameManager.Instance.saveState();
        StartCoroutine(check());
    }
    public void calmThree()
    {
        GameManager.Instance.ChangeCoin(amount*3);
        amount = 0;
        GameManager.Instance.stateData.GoldGet = DateTime.Now;
        GameManager.Instance.saveState();
        StartCoroutine(check());

    }
    public static float calculatePerSec(int a)
    {
        //base 0.1f
        //increase by 0.1f eachLevel
        
        float answer = 0;
        switch (a)
        {
            case 0:
                answer = 0.1f;
                break;
            case 1:
                answer = 0.3f;
                break;
            case 2:
                answer = 0.6f;
                break;
            case 3:
                answer = 1f;
                break;
            case 4:
                answer = 1.5f;
                break;
            case 5:
                answer = 2;
                break;
            case 6:
                answer = 3;
                break;
            case 7:
                answer = 5;
                break;
           
        }
        return answer;
    }
    public static int CalculateStorage(int a)
    {
        int answer = 0;
        switch (a)
        {
            case 0:
                answer = 400;
                break;
            case 1:
                answer = 800;
                break;
            case 2:
                answer = 1200;
                break;
            case 3:
                answer = 2000;
                break;
            case 4:
                answer = 2500;
                break;
            case 5:
                answer = 3500;
                break;
            case 6:
                answer = 5000;
                break;
            case 7:
                answer = 10000;
                break;

        }
        return answer;
    }
    public int CurrentAmount()
    {
        double a = (DateTime.Now - GameManager.Instance.stateData.GoldGet).TotalSeconds;
        amount =(int)( CPS * a);
        if (amount > storage)
            amount = storage;
        return amount;


    }
    public int CalculateStoragePrice(int a)
    {
        int answer = 0;
        switch (a)
        {
            case 0:
                answer = 5;
                break;
            case 1:
                answer = 10;
                break;
            case 2:
                answer = 20;
                break;
            case 3:
                answer = 30;
                break;
            case 4:
                answer = 50;
                break;
            case 5:
                answer = 70;
                break;
            case 6:
                answer = 100;
                break;

        }
        return answer;
    }
    public int CalculateCPSPrice(int a)
    {
        int answer = 0;
        switch (a)
        {
            case 0:
                answer = 5;
                break;
            case 1:
                answer = 10;
                break;
            case 2:
                answer = 20;
                break;
            case 3:
                answer = 30;
                break;
            case 4:
                answer = 50;
                break;
            case 5:
                answer = 70;
                break;
            case 6:
                answer = 100;
                break;

        }
        return answer;
    }

    public void UpgradeStorage()
    {
        GameManager.Instance.ChangeGem(-CalculateStoragePrice(storageLevel));
        GameManager.Instance.stateData.coinerStorageLevel++;
        GameManager.Instance.saveState();
        storageLevel = GameManager.Instance.stateData.coinerStorageLevel;
        renewTexts();
        CPS = calculatePerSec(coinPerSecLevel);
        storage = CalculateStorage(storageLevel);
    }
    public void UpgradeCPS()
    {
        GameManager.Instance.ChangeGem(-CalculateCPSPrice(coinPerSecLevel));
        GameManager.Instance.stateData.CoinPerSecLevel++;
        GameManager.Instance.saveState();
        coinPerSecLevel = GameManager.Instance.stateData.CoinPerSecLevel;
        renewTexts();
        CPS = calculatePerSec(coinPerSecLevel);
        storage = CalculateStorage(storageLevel);
    }
    public void Exit()
    {
        anim.SetTrigger("Idle");
    }
}
