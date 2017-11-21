using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerShopManager : AlphaScript
{
    public static playerShopManager Instance;
    public GameObject shapePlace;
    public Transform BtnParent;
    public GameObject PlayerBTN,lockPanel;
    public Text CoinAmount, GemAmount, expText;
    public Slider XPSlider;

    [Header("GameButtons")]
    public Button BuyBtn;
    public Button SelectBtn;

    AudioSource aud;
    float CurrentCoin, CurrentGem, currentXP;
    Player pl;
    PlayerDataBase DB;
	// Use this for initialization
	void Start () {
        loadingScreen.Instance.Disapear();
        aud=GetComponent<AudioSource>();
        playSound("Chaching");
        Instance = this;
        DB = GameManager.Instance.PlayerDataBase;
        BTNCreator();
        XPSlider.maxValue = GameManager.Instance.expNeed();

	}
	
	// Update is called once per frame
	void Update () {
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
        GemAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentGem).ToString(), GemAmount).ToString();


        XPSlider.value = currentXP;
        expText.text = GameManager.NumberPersian(((int)currentXP).ToString(), expText) + "/" + GameManager.NumberPersian(GameManager.Instance.expNeed().ToString(), expText);
        if (Input.GetKey(KeyCode.Escape))
            loadingScreen.Instance.Show("MainMenu");
		
	}
    public void BTNCreator()
    {
        for (int i = 0; i < DB.DBLength; i++)
        {
            Player p = DB.GetByIndex(i);
            GameObject g= Instantiate(PlayerBTN, BtnParent);g.GetComponent<CharacterBtn>().makeButton(p);
            g.transform.localScale = Vector3.one;
            if (GameManager.Instance.characterData.charcaterId == p.Id)
            {

                Instantiate(p.Shape.transform.GetChild(0).gameObject, shapePlace.transform).transform.localPosition = Vector2.zero;
                GetComponent<characterState>().Repaint(p);
                g.transform.GetChild(2).gameObject.SetActive(true);
                
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
            Player p = DB.GetByIndex(i);
            GameObject g = Instantiate(PlayerBTN, BtnParent); g.GetComponent<CharacterBtn>().makeButton(p);
            g.transform.localScale = Vector3.one;
            if (GameManager.Instance.characterData.charcaterId == p.Id)
                g.transform.GetChild(2).gameObject.SetActive(true);

        }
    }

    public void BuyCharacter()
    {
        playSound("Button");
        if (pl.Price.type == Price.Type.Coin)
        {
            GameManager.Instance.ChangeCoin(-pl.Price.amount);
            playSound("Chaching");


        }
        else if (pl.Price.type == Price.Type.Coin)
        {
            GameManager.Instance.ChangeGem(-pl.Price.amount);
            playSound("Chaching");


        }

        GameManager.Instance.characterData.charcaterId = pl.Id;
        GameManager.Instance.characterData.charcaterIds.Add(pl.Id);
        BuyBtn.gameObject.SetActive(false);
        SelectBtn.gameObject.SetActive(true);
        SelectBtn.transform.GetChild(0).GetComponent<Text>().text = GameManager.Language("انتخاب", "Select",  SelectBtn.transform.GetChild(0).GetComponent<Text>());
        
        SelectBtn.interactable = false;

        //////
        GameManager.Instance.characterData.levels.Add(new Level(pl.Id));
        GameManager.Instance.SetCharacterLevel(pl.Id);
        //////
        GameManager.Instance.saveCharacter();
        print("Bought");
    }
    public void SelectCharacter()
    {
        

        GameManager.Instance.characterData.charcaterId = pl.Id;
        GameManager.Instance.SetCharacterLevel(pl.Id);
        GameManager.Instance.saveCharacter();
        SelectBtn.gameObject.SetActive(false);
        changeTick();
        playSound("Coin");

    }
    public void SetPlayer(Player p)
    {
        pl = p;
    }
    public void MainMenu()
    {
        playSound("Button");
        loadingScreen.Instance.Show("MainMenu");

    }
    public void Test()
    {
    }
    public void ChangeLevel()
    {

    }
}
