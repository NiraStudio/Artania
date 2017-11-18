using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class GemConvertor : MonoBehaviour {
    public int gem, coin;
    public Animator quiestionPanel,dontHaveEnough;
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
        if (GameManager.Instance.currencyData.Gem >= gem)
        {
            text.text = GameManager.Language("از این خرید مطمئنی؟", "Are you sure?", text);
            quiestionPanel.gameObject.transform.GetChild(1).GetComponent<Button>().interactable = true;

            quiestionPanel.SetTrigger("Enter");
            GemConvertorPanel.Instance.currentRate = this;
        }
        else
        {
            text.text = GameManager.Language("جم کافی نداری", "You dont have enough gem", text);
            quiestionPanel.gameObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
            quiestionPanel.SetTrigger("Enter");
        }
        
    }
    public void QuestionPanelExit()
    {
        quiestionPanel.SetTrigger("Idle");
        GemConvertorPanel.Instance.currentRate = null;
    }
    public void Convert()
    {
        if (GameManager.Instance.currencyData.Gem >= gem)
        {
            GameManager.Instance.ChangeGem(-gem);
            GameManager.Instance.ChangeCoin(coin);
            QuestionPanelExit();
        }
    }
}
