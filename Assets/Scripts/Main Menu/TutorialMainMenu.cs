using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMainMenu : AlphaScript
{
    public Sprite[] TutorialByOrder;
    public int GivePrizeTime;
    int index = 0;
    Image img;
    // Use this for initialization
    void Start() {
        img = GetComponent<Image>();
        img.sprite = TutorialByOrder[index];
        gameObject.SetActive(PlayerPrefs.GetInt("FirstTime")==0?true:false);
	}

    // Update is called once per frame
    void Update()
    {
        if (Application.isMobilePlatform)
        {
            Touch[] t = Input.touches;
            if (t.Length > 0 && t.Length <= 1)
            {

                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i].phase == TouchPhase.Ended)
                    {
                        index++;

                        if(index ==GivePrizeTime)
                        {
                            print("Give Prize");
                            PlayerPrefs.SetInt("FirstTime", 1);
                            GameManager.Instance.ChangeCoin(1000);
                            GameManager.Instance.AddEXP(1000);
                        }
                        if (index < TutorialByOrder.Length)
                            img.sprite = TutorialByOrder[index];
                        
                        else
                        {

                            gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else if (Application.isEditor)
            if (Input.GetMouseButtonDown(0))
            {
                index++;
                if (index == GivePrizeTime )
                {

                    PlayerPrefs.SetInt("FirstTime", 1);
                    GameManager.Instance.ChangeCoin(1000);
                    GameManager.Instance.AddEXP(1000);
                }
                if (index < TutorialByOrder.Length)
                    img.sprite = TutorialByOrder[index];
                else
                {

                    gameObject.SetActive(false);
                    PlayerPrefs.SetInt("FirstTime", 1);
                }

            }
    }
}
