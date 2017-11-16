using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class GurdianShopManager : AlphaScript
{
    public static GurdianShopManager Instance;
    public GameObject shapePlace;
    public Transform BtnParent;
    public GameObject GurdianBTN,lockPanel;
    public Text CoinAmount, gemAmount, expText;
    public Slider XPSlider;
    public RtlText Des,GurdName;
    [Header("GameButtons")]
    public Button BuyBtn;
    public Button SelectBtn;

    AudioSource aud;
    GurdianClass pl;
    GurdianDataBase DB;
    float CurrentCoin, CurrentGem, currentXP;

    // Use this for initialization
    void Start()
    {
        loadingScreen.Instance.Disapear();
        aud=GetComponent<AudioSource>();
        playSound("Chaching");

        Instance = this;
        DB = GameManager.Instance.gurdianDataBase;
        BTNCreator();
        XPSlider.maxValue = GameManager.Instance.expNeed();

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentCoin != GameManager.Instance.currencyData.Coin && GameManager.Instance.currencyData.Coin != 0)
            CurrentCoin = Mathf.MoveTowards(CurrentCoin, GameManager.Instance.currencyData.Coin, (GameManager.Instance.currencyData.Coin / 1.5f) * Time.fixedDeltaTime);
        else if (GameManager.Instance.currencyData.Coin == 0)
            CurrentCoin = 0;

        if (CurrentGem != GameManager.Instance.currencyData.Gem && GameManager.Instance.currencyData.Gem != 0)
            CurrentGem = Mathf.MoveTowards(CurrentGem, GameManager.Instance.currencyData.Gem, GameManager.Instance.currencyData.Gem * Time.fixedDeltaTime);

        else if (GameManager.Instance.currencyData.Gem == 0)
            CurrentGem = 0;


        if (currentXP != GameManager.Instance.stateData.EXP && GameManager.Instance.stateData.EXP != 0)
            currentXP = Mathf.MoveTowards(currentXP, GameManager.Instance.stateData.EXP, GameManager.Instance.stateData.EXP * Time.fixedDeltaTime);

        else if (GameManager.Instance.stateData.EXP == 0)
            currentXP = 0;

        CoinAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentCoin).ToString(), CoinAmount).ToString();
        gemAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentGem).ToString(), gemAmount).ToString();


        XPSlider.value = currentXP;
        expText.text = GameManager.NumberPersian(((int)currentXP).ToString(), expText) + "/" + GameManager.NumberPersian(GameManager.Instance.expNeed().ToString(), expText);
        if (Input.GetKey(KeyCode.Escape))
            loadingScreen.Instance.Show("MainMenu");

    }
    void BTNCreator()
    {
        for (int i = 0; i < DB.DBLength; i++)
        {
            GurdianClass p = DB.GetByIndex(i);
            GameObject g = Instantiate(GurdianBTN, BtnParent); g.GetComponent<GurdianBTN>().makeButton(p);
            g.transform.localScale = Vector3.one;
            if (GameManager.Instance.characterData.GurdianId == p.Id)
            {

                GameObject a= Instantiate(p.Prefab.gameObject, shapePlace.transform);a.transform.localPosition = Vector2.zero;
                a.SetActive(true);
                g.transform.GetChild(2).gameObject.SetActive(true);
                Des.text = GameManager.Language(p.PersianDes, p.EnglishDes, Des);
                GurdName.text = GameManager.Language(p.PRName, p.ENName, GurdName);
            }

        }
    }
    public void changeTick()
    {
        for (int i = 0; i < BtnParent.transform.childCount; i++)
        {
            Destroy(BtnParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < DB.DBLength; i++)
        {
            GurdianClass p = DB.GetByIndex(i);
            GameObject g = Instantiate(GurdianBTN, BtnParent); g.GetComponent<GurdianBTN>().makeButton(p);
            g.transform.localScale = Vector3.one;
            if (GameManager.Instance.characterData.GurdianId == p.Id)
                g.transform.GetChild(2).gameObject.SetActive(true);

        }
    }

    public void BuyGurdian()
    {

        playSound("Button");


        if (pl.Price.type == Price.Type.Coin)
        {
            GameManager.Instance.ChangeCoin(-pl.Price.amount);
            playSound("Coin");


        }
        else if (pl.Price.type == Price.Type.Coin)
        {
            GameManager.Instance.ChangeGem(-pl.Price.amount);
            playSound("Gem");


        }
        GameManager.Instance.characterData.GurdianIds.Add(pl.Id);
        GameManager.Instance.characterData.GurdianId = pl.Id;
        BuyBtn.gameObject.SetActive(false);
        SelectBtn.gameObject.SetActive(true);
        SelectBtn.transform.GetChild(0).GetComponent<Text>().text = "Selected";
        SelectBtn.interactable = false;
        GameManager.Instance.saveCharacter();
       MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.buyNewGurdian, 1);

    }
    public void SelectGurdian()
    {
        playSound("Button");


        GameManager.Instance.characterData.GurdianId = pl.Id;
        SelectBtn.transform.GetChild(0).GetComponent<Text>().text = "Selected";
        SelectBtn.interactable = false;
        GameManager.Instance.saveCharacter();
    }
    public void SetPlayer(GurdianClass p)
    {
        pl = p;
    }
    public void MainMenu()
    {
        loadingScreen.Instance.Show("MainMenu");

    }
}
