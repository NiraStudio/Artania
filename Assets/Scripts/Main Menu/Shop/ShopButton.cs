using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class ShopButton : AlphaScript
{
    public int Price;
    public Sprite img;

    Image imgComponent;
    RtlText PriceText;

	// Use this for initialization
	void Start () {
       // imgComponent.sprite = img;
       // PriceText.text = GameManager.NumberPersian(Price.ToString() + " T", PriceText);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
