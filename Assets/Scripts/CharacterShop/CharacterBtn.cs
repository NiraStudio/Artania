using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class CharacterBtn : AlphaScript
{
    public Image CharacterImage;
    public Button Btn;
    public Text CharacterName;
    public Sprite Gem, Coin;
    Player p;
	// Use this for initialization
	void Start () {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(Clicked);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Clicked()
    {
        playSound("Button");

        playerShopManager.Instance.SelectBtn.gameObject.SetActive(false);
        playerShopManager.Instance.BuyBtn.gameObject.SetActive(false);
        playerShopManager.Instance.lockPanel.SetActive(false);
        Destroy(playerShopManager.Instance.shapePlace.transform.GetChild(0).gameObject);
        Instantiate(p.Shape.transform.GetChild(0).gameObject, playerShopManager.Instance.shapePlace.transform).transform.localPosition = Vector2.zero;
        playerShopManager.Instance.GetComponent<characterState>().Repaint(p);
        playerShopManager.Instance.SetPlayer(p);

                print("Show BTN");
        if (GameManager.Instance.characterData.charcaterIds.Contains(p.Id))
        {
            Button b = playerShopManager.Instance.SelectBtn;


            if (GameManager.Instance.characterData.charcaterId != p.Id)
            {
                b.gameObject.SetActive(true);
                b.transform.GetChild(0).GetComponent<RtlText>().text = GameManager.Language("انتخاب","Select",b.transform.GetChild(0).GetComponent<RtlText>());
                b.interactable = true;
            }
        }
        else
        {
            Button b = playerShopManager.Instance.BuyBtn;
            b.gameObject.SetActive(true);
            b.transform.GetChild(1).gameObject.SetActive(true);
            if (p.Price.type == Price.Type.Coin)
            {
                if (GameManager.Instance.currencyData.Coin >= p.Price.amount)
                    b.interactable = true;
                else
                    b.interactable = false;
                b.transform.GetChild(1).GetComponent<Image>().sprite = Coin;

            }
            else if (p.Price.type == Price.Type.Gem)
            {
                if (GameManager.Instance.currencyData.Gem >= p.Price.amount)
                    b.interactable = true;
                else
                    b.interactable = false;
                b.transform.GetChild(1).GetComponent<Image>().sprite = Gem;

            }
            b.transform.GetChild(0).GetComponent<Text>().text = GameManager.NumberPersian(p.Price.amount.ToString(), b.transform.GetChild(0).GetComponent<Text>());
            if (GameManager.Instance.stateData.lvl < p.LvlNeed)
            {
                playerShopManager.Instance.lockPanel.transform.GetChild(1).GetComponent<Text>().text = p.LvlNeed.ToString();
                playerShopManager.Instance.lockPanel.SetActive(true);
            }

        }
        
    }
    public void makeButton(Player pl){
        p = pl;
        CharacterImage.sprite = pl.Image;
        CharacterName.text=pl.Name;
    }
}
