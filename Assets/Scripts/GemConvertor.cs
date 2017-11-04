using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class GemConvertor : MonoBehaviour {
    public int gem, coin;
    public Animator quiestionPanel;
    public RtlText text,coinAmount,GemAmount;
	// Use this for initialization
	void Start () {
       /* if (GameManager.Instance.currencyData.Gem >= gem)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;*/
       
    }
	
	// Update is called once per frame
    public void RenewTitle()
    {
        coinAmount.text = GameManager.NumberPersian(coin.ToString(), coinAmount);
        GemAmount.text = GameManager.NumberPersian(gem.ToString(), GemAmount);
    }
    public void OnClick()
    {
        text.text = GameManager.Language("از این خرید مطمئنی؟", "Are you sure?", text);
        quiestionPanel.SetTrigger("Enter");
        GemConvertorPanel.Instance.currentRate = this;
        
    }
    public void QuestionPanelExit()
    {
        quiestionPanel.SetTrigger("Idle");
        GemConvertorPanel.Instance.currentRate = null;
    }
    public void Convert()
    {
        GameManager.Instance.ChangeGem(-gem);
        GameManager.Instance.ChangeCoin(coin);
        QuestionPanelExit();
    }
}
