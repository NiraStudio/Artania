using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class FinishPanel : AlphaScript
{
    public static FinishPanel Instance;

    public float Coin, Exp;
    public Text CoinText, ExpText;
    public RtlText HighScore;
    float CurrentCoin, CurrentExp;
    public int time = 0;
    Animator anim;
    bool enter;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        HighScore.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enter)
        {
            if (Application.isMobilePlatform)
            {
                Touch[] t = Input.touches;
                if (t.Length > 0 && t.Length <= 1)
                {

                    for (int i = 0; i < t.Length; i++)
                    {
                        if (t[i].phase == TouchPhase.Began)
                        {
                            time++;
                        }
                    }
                }
            }
            else if (Application.isEditor)
                if (Input.GetMouseButtonDown(0))
                {

                    time++;
                }

        }
        if (enter)
        {
            switch (time)
            {
                case 0:
                    if (CurrentCoin < Coin)
                        CurrentCoin = Mathf.MoveTowards(CurrentCoin, Coin, Coin / 3 * Time.deltaTime);
                    else if (CurrentCoin >= Coin)
                    {
                        CurrentCoin = Coin;
                        time++;
                    }

                    break;
                case 1:
                    if (CurrentExp < Exp)
                        CurrentExp = Mathf.MoveTowards(CurrentExp, Exp, Exp / 3 * Time.deltaTime);
                    else if (CurrentExp >= Exp)
                    {
                        CurrentExp = Exp;
                        time++;
                    }
                    break;
                default:
                    break;
            }
        }

        CoinText.text = GameManager.NumberPersian(((int)CurrentCoin).ToString(), CoinText);
        ExpText.text = GameManager.NumberPersian(((int)CurrentExp).ToString(), ExpText);

    }

    IEnumerator startC()
    {
        yield return new WaitUntil(() => time == 1);
        CurrentCoin = Coin;
        yield return new WaitUntil(() => time == 2);
        CurrentExp = Exp;
        if (GamePlayManager.Instance.Exp > GameManager.Instance.stateData.HighScore)
        {
            HighScore.text = GameManager.Language("رکورد جدید", "New Record", HighScore);
            HighScore.gameObject.SetActive(true);
        }
    }

    public void Enter()
    {
        anim.SetTrigger("Enter");
    }
    public void Idle()
    {
        anim.SetTrigger("Idle");
    }
    public void startCounting()
    {
        Coin = GamePlayManager.Instance.CoinAmount;
        Exp = GamePlayManager.Instance.Exp;
        Enter();
        StartCoroutine(startC());
        enter = true;
    }
    public void ReStart()
    {
        stopOne("GamePlayMusic");
        stopOne("BossMusic");
        loadingScreen.Instance.Show("Play");
    }
    public void MainMenu()
    {
        loadingScreen.Instance.Show("MainMenu");
    }
}
