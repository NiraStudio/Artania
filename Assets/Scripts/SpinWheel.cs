using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpinWheel : AlphaScript
{
    public Sprite Gem, Exp, coin;
	public List<Reward> normalRewad;
    public List<Reward> CoolRewardRewad;
    public List<Reward> AwesomeRewad;
    public Text[] prizesInfo;

	public List<AnimationCurve> animationCurves;
    public List<Reward> prize;
	private bool spinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
    Animator anim;
    public GameObject FreeBtn, VideoBtn;
	void Start(){
        anim =transform.parent. GetComponent<Animator>();
		spinning = false;
		anglePerItem = 360/prize.Count;
        //startSpin();
        //prize algoritm
        PrizeFeeder();
         System. DateTime a = GameManager.Instance.stateData.RewardTime;

        System.DateTime b = new System.DateTime(a.Year, a.Month, a.Day, 0, 0, 0);
        b = b.AddDays(1);
        if (System.DateTime.Now >= b)
        {
            FreeBtn.SetActive(true);
        }
        else
        {
            FreeBtn.SetActive(false); 
        }
	}
    public void PrizeFeeder()
    {
        for (int i = 0; i < 12; i++)
        {
            if (i < 10)
            {
                prize[i] = normalRewad[Random.Range(0, normalRewad.Count)];
                prizesInfo[i].text = prize[i].amount.ToString();
                switch (prize[i].type)
                {
                    case Reward.Type.Gem:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Gem;
                        break;
                    case Reward.Type.Coin:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = coin;
                        break;
                    case Reward.Type.Exp:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Exp;

                        break;

                }

            }
            else if (i < 11)
            {
                prize[i] = CoolRewardRewad[Random.Range(0, CoolRewardRewad.Count)];
                prizesInfo[i].text = prize[i].amount.ToString();
                switch (prize[i].type)
                {
                    case Reward.Type.Gem:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Gem;
                        break;
                    case Reward.Type.Coin:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = coin;
                        break;
                    case Reward.Type.Exp:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Exp;

                        break;

                }
            }
            else
            {
                prize[i] = AwesomeRewad[Random.Range(0, AwesomeRewad.Count)];
                prizesInfo[i].text = prize[i].amount.ToString();
                switch (prize[i].type)
                {
                    case Reward.Type.Gem:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Gem;
                        break;
                    case Reward.Type.Coin:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = coin;
                        break;
                    case Reward.Type.Exp:
                        prizesInfo[i].transform.GetChild(0).GetComponent<Image>().sprite = Exp;
                        break;

                }
            }
        }
    }
    public void idle()
    {
        playSound("Button");

        anim.SetTrigger("Idle");

    }
    public void Enter()
    {
        playSound("Button");

        anim.SetTrigger("Enter");

    }
    public void startSpin(){
        randomTime = Random.Range (1, 4);
			itemNumber = Random.Range (0, prize.Count);
			float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
			
			StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
    }
    public void freeSpin()
    {
        startSpin();
        FreeBtn.SetActive(false);
        VideoBtn.SetActive(true);
        GameManager.Instance.stateData.RewardTime = System.DateTime.Now;
        GameManager.Instance.saveState();
    }
	IEnumerator SpinTheWheel (float time, float maxAngle)
	{
		spinning = true;
		
		float timer = 0.0f;		
		float startAngle = transform.eulerAngles.z;		
		maxAngle = maxAngle - startAngle;
		
		int animationCurveNumber = Random.Range (0, animationCurves.Count);
		Debug.Log ("Animation Curve No. : " + animationCurveNumber);
		
		while (timer < time) {
		//to calculate rotation
			float angle = maxAngle * animationCurves [animationCurveNumber].Evaluate (timer / time) ;
			transform.eulerAngles = new Vector3 (0.0f, 0.0f, angle + startAngle);
			timer += Time.deltaTime;
			yield return 0;
		}
		
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, maxAngle + startAngle);
		spinning = false;

        Debug.Log("Prize: " + prize[itemNumber].type + " " + prize[itemNumber].amount);//use prize[itemNumnber] as per requirement
        switch (prize[itemNumber].type)
        {
            case Reward.Type.Gem:
                GameManager.Instance.ChangeGem(prize[itemNumber].amount);
                break;
            case Reward.Type.Coin:
                GameManager.Instance.ChangeCoin(prize[itemNumber].amount);

                break;
            

                break;
            case Reward.Type.Exp:
                GameManager.Instance.AddEXP(prize[itemNumber].amount);

                break;
            default:
                break;
        }
        GameManager.Instance.saveCurrency();
	}	
}

[System.Serializable]
public class prizeInfo
{
    public Text AmountText;
    public Image img;
}