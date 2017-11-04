using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : AlphaScript
{

    public Text CoinAmount, GemAmount, expText;
    public GameObject CharacterPos;
    public Slider XPSlider;
    AudioSource aud;
    float CurrentGem, CurrentCoin, currentXP;
    // Use this for initialization
    void Start()
    {
        playSound("Chaching");
        loadingScreen.Instance.Disapear();
        GameObject g = Instantiate(GameManager.Instance.PlayerDataBase.GetPlayerById(GameManager.Instance.characterData.charcaterId).Shape.transform.GetChild(0), CharacterPos.transform).gameObject; g.transform.localPosition = Vector2.zero;
        g.GetComponent<Animator>().SetTrigger("Idle");
        XPSlider.maxValue = GameManager.Instance.expNeed();

    }
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
        GemAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentGem).ToString(), GemAmount).ToString();


        XPSlider.value = currentXP;
        expText.text = GameManager.NumberPersian(((int)currentXP).ToString(), expText) + "/" + GameManager.NumberPersian(GameManager.Instance.expNeed().ToString(), expText);
        if (Input.GetKey(KeyCode.Escape))
            loadingScreen.Instance.Show("MainMenu");
    }
    public void GoToMenu()
    {
        loadingScreen.Instance.Show("MainMenu");
    }
    public void goToGemToCoin()
    {

    }

}
