using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class GurdianBTN : AlphaScript
{
    public Image GurdianImage;
    public Button Btn;
    public Text GurdianName;
    public GameObject shape;
    public Sprite Gem, Coin;


    GurdianClass p;
    // Use this for initialization
    void Start()
    {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(Clicked);
    }
    public void Clicked()
    {
        GurdianShopManager.Instance.SelectBtn.gameObject.SetActive(false);
        GurdianShopManager.Instance.BuyBtn.gameObject.SetActive(false);
        GurdianShopManager.Instance.lockPanel.SetActive(false);
        Destroy(GurdianShopManager.Instance.shapePlace.transform.GetChild(0).gameObject);
        print("Hello");
        GameObject a = Instantiate(p.Prefab, GurdianShopManager.Instance.shapePlace.transform); a.transform.localPosition = Vector2.zero;
        a.SetActive(true);
        if (PlayerPrefs.GetString("Language") == "Persian")
        {
            GurdianShopManager.Instance.Des.text = p.PersianDes;
            GurdianShopManager.Instance.GurdName.text = p.PRName;
        }
        else
        {
            GurdianShopManager.Instance.Des.text = p.EnglishDes;
            GurdianShopManager.Instance.GurdName.text = p.ENName;
        }
        GurdianShopManager.Instance.Des.text = GameManager.Language(p.PersianDes, p.EnglishDes, GurdianShopManager.Instance.Des);
        GurdianShopManager.Instance.GurdName.text= GameManager.Language(p.PRName, p.ENName, GurdianShopManager.Instance.GurdName);

        GurdianShopManager.Instance.SetPlayer(p);

        if (GameManager.Instance.characterData.GurdianIds.Contains(p.Id))
        {
            Button b = GurdianShopManager.Instance.SelectBtn;


            if (GameManager.Instance.characterData.GurdianId != p.Id)
            {
                b.gameObject.SetActive(true);
                b.transform.GetChild(0).GetComponent<Text>().text = GameManager.Language("انتخاب", "Select", b.transform.GetChild(0).GetComponent<RtlText>()); 
                b.interactable = true;
            }
        }
        else
        {
            Button b = GurdianShopManager.Instance.BuyBtn;
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
            if (GameManager.Instance.stateData.lvl < p.lvlNeed)
            {
                GurdianShopManager.Instance.lockPanel.transform.GetChild(0).GetComponent<Text>().text = p.lvlNeed.ToString();
                GurdianShopManager.Instance.lockPanel.SetActive(true);
            }

        }
    }
    public void makeButton(GurdianClass pl)
    {
        p = pl;
        GurdianImage.sprite = pl.Image;
        GurdianName.text = GameManager.Language(p.PRName, p.ENName, GurdianName);


    }
}

