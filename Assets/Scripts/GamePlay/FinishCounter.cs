using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishCounter : AlphaScript
{
    public static FinishCounter Instance;
    public float MaxTime;
    public Slider Timer;
    public Button ExtraLifeBtn;
    Animator anim;
    bool start;
    float t;
    int time=1;
    // Use this for initialization
    void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        t = MaxTime;
        Timer.maxValue = t;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (start)
        {
            t -= Time.deltaTime;
            Timer.value = t;
            if (t <= 0)
            {

                GameManager.Instance.ChangeCoin((int)GamePlayManager.Instance.CoinAmount);
                print("Coin Amount = " + (int)GamePlayManager.Instance.CoinAmount);
                GameManager.Instance.AddEXP(GamePlayManager.Instance.Exp);
                //MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.Exprience, GamePlayManager.Instance.Exp);
               // MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.coin, GamePlayManager.Instance.CoinAmount);
                MissionController.Instance.restart();
                if (GamePlayManager.Instance.Exp > GameManager.Instance.stateData.HighScore)
                {
                    GameManager.Instance.stateData.HighScore = GamePlayManager.Instance.Exp;
                    GameManager.Instance.saveState();
                    PlayerPrefs.SetInt("SetScore", 1);
                }
                Idle();
                GamePlayManager.Instance.pauseAllow = true;
                FinishPanel.Instance.startCounting();
                start = false;

            }
        }
    }
    public void StartCoolDown()
    {
       
    }
    public void ReStartGame()
    {
        if (PlayerPrefs.GetInt("FirstTime") != 0)
            GameManager.Instance.ChangeGem(-1);

        Idle();
        start = false;
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("Restart");
        GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().enabled = true;

        StartCoroutine(GamePlayManager.Instance.coolDown(true));
        time--;

    }
    public void Enter()
    {
        stopOne("GamePlayMusic");
        stopOne("BossMusic");
        StartCoolDown();
        start = true;
        t = time == 0 ? 0 : MaxTime;
        if(t!=0)
        anim.SetTrigger("Enter");
        if (GameManager.Instance.currencyData.Gem >= 1)
            ExtraLifeBtn.interactable = true;
        else
            ExtraLifeBtn.interactable = false;
    }
    public void Idle()
    {
        anim.SetTrigger("Idle");
    }
}
