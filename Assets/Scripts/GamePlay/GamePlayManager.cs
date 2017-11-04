using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class GamePlayManager : AlphaScript {
    public static GamePlayManager Instance;

    
    public GameObject PausePanel,boostPanel,frog;
    public Animator splash;
    public bool Tutorial;
    public bool play;
    public bool Run,seeAd;
    public Text CoinText,EXPText;
    public Text CoolDown;
    public tutorialPanel tutorial;
    public float meterPerSec;
    public ObscuredFloat Distance;
    public Image iconOne, iconTwo;
    public Text BossKilledNumber;


    bool bossFight;
    public int CoinAmount
    {
        get { return CoinCode >> 3; }
    }
    int CoinCode;

    public int Exp
    {
        get { return exp; }
        set { exp = value; }
    }
    public float animationSpeed;
    public ObscuredInt bossKilledNumber, enemiesNumber;

    float expCurrent, coinCurrent;
    ObscuredInt exp;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
    void Start()
    {

        if (PlayerPrefs.GetInt("FirstTime") == 0)
            Tutorial = true;
        if (Tutorial)
        {
            CoinText.gameObject.transform.parent.gameObject.SetActive(false);
            EXPText.gameObject.transform.parent.gameObject.SetActive(false);
        }
        loadingScreen.Instance.Disapear();
        StartCoroutine(coolDown(false));
    }
    public IEnumerator coolDown(bool Restart)
    {
        if (Restart)
            StartCoroutine(GetComponent<EnemiesController>().Spawn());
        else
            GetComponent<EnemiesController>().chooseBoss();

        
        #region Boost
        if (GameManager.Instance.stateData.LastKilledBoss > 3 && GameManager.Instance.coinAmount >= (GameManager.Instance.stateData.LastKilledBoss - 2) * 300)
        {
            CoolDown.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            CoolDown.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.NumberPersian(((GameManager.Instance.stateData.LastKilledBoss - 2) * 300).ToString(), CoolDown.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>());
        }
        if ( GameManager.Instance.coinAmount >= 300)
        {
            CoolDown.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            CoolDown.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.NumberPersian("300", CoolDown.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>());
        }

        #endregion


        if (PlayerPrefs.GetInt("FirstTime") == 0 && tutorial.gameObject.activeInHierarchy)
            yield return new WaitUntil(() => tutorial.clicked);
        CoolDown.gameObject.SetActive(true);
        for (int i = 3; i >= 0; i--)
        {
            if (i != 0)
                CoolDown.text = i.ToString();
            else
                CoolDown.text = "Start!";
            yield return new WaitForSeconds(1);
        }
        
        CoolDown.gameObject.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().Allower();
        play = true;
        playSound("GamePlayMusic");
       if(!Tutorial&&!Restart)
            MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.game, 1);
       if (!bossFight)
       {
          Run = true;
           playSound("GamePlayMusic");

       }else
       {
           Run = false;
           playSound("BossMusic");

       }
    }
	// Update is called once per frame
	void FixedUpdate () {
        BossKilledNumber.text = GameManager.NumberPersian(bossKilledNumber.ToString(), BossKilledNumber);

        if ((int)coinCurrent != CoinAmount)
            coinCurrent = Mathf.MoveTowards(coinCurrent, CoinAmount, CoinAmount * Time.fixedDeltaTime);

        if ((int)expCurrent != exp)
            expCurrent = Mathf.MoveTowards(expCurrent, exp, exp * Time.fixedDeltaTime);
        CoinText.text = "X " + GameManager.NumberPersian(((int)coinCurrent).ToString(), CoinText);
        EXPText.text = " EXP " + GameManager.NumberPersian(((int)expCurrent).ToString(), EXPText);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeInHierarchy)
                UnPause();
            else
                Pause();
        }
        if (Run)
        {
            Distance += meterPerSec * Time.fixedDeltaTime;
        }
	}
    public void AddCoin(float AMount)
    {
        int a = CoinCode >> 3;
        a += (int)AMount;
        CoinCode = a << 3;
        a = 0;
    }
    public void addEXP(int amount)
    {
        exp += amount;
        MissionController.Instance.setMission(Alpha.MissionSystem.Mission.Type.Exprience, exp);
    }
    public void ChangeEnemieNumer(int number)
    {
        enemiesNumber = number;
    }
    public void ReduceEnemyNumber()
    {
        enemiesNumber--;
        if (enemiesNumber == 0)
        {
            stopOne("GamePlayMusic");
            playSound("BossMusic");
            GetComponent<EnemiesController>().respawnBoss();
            bossFight = true;
        }
        
    }
    public void BossDie()
    {
        StartCoroutine(BossDeath());
    }
    public IEnumerator BossDeath()
    {
        play = false;
        stopOne("BossMusic");
        //splash
        yield return new WaitForSeconds(1.5f);

        splash.gameObject.SetActive(true);
        splash.SetTrigger("In");
        yield return new WaitForSeconds(1.5f);
        //desplash
        Boss.Instance.Destroy();
        iconOne.gameObject.SetActive(false);
        iconTwo.gameObject.SetActive(false);
        PlayerMoveForward.Instance.RestartSpeed();
        if (!Tutorial)
        {
            bossKilledNumber++;
            if (GameManager.Instance.stateData.LastKilledBoss < bossKilledNumber)
            {
                GameManager.Instance.stateData.LastKilledBoss = bossKilledNumber;
                GameManager.Instance.saveState();
            }
            int a = Random.Range(0, 101);
            if (a<=10)
            {
                Instantiate(frog);
                splash.SetTrigger("Out");
                bossFight = false;
                splash.gameObject.SetActive(false);

            }
            else
            {
                GetComponent<EnemiesController>().chooseBoss();
                splash.SetTrigger("Out");
                yield return new WaitForSeconds(1.5f);
                bossFight = false;
                splash.gameObject.SetActive(false);
                playSound("GamePlayMusic");
                Run = true;
                play = true;
            }
        }
        else
        {
            
            loadingScreen.Instance.Show("MainMenu");
        }
    }
    public void FrogDeath()
    {
        StartCoroutine(FrogDis());
    }
    IEnumerator FrogDis()
    {
        Run = false;
        play = false;
        yield return new WaitForSeconds(1.5f);
        GetComponent<EnemiesController>().chooseBoss();
        Run = true;
        play = true;
    }
    void OnApplicationPause(bool a)
    {
        if (!a)
        {
            if (seeAd)
            {
                Pause();
                seeAd = false;
            }
        }
    }
    public void runningState(bool state)
    {
        Run = state;
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        Run = false;
        play = false;
        GameManager.Instance.ChangeCoin((int)GamePlayManager.Instance.CoinAmount);
        GameManager.Instance.AddEXP(GamePlayManager.Instance.Exp);
        MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.Exprience, GamePlayManager.Instance.Exp);
        MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.coin, GamePlayManager.Instance.CoinAmount);
        MissionController.Instance.restart();
        if (GamePlayManager.Instance.Exp > GameManager.Instance.stateData.HighScore)
        {
            GameManager.Instance.stateData.HighScore = GamePlayManager.Instance.Exp;
            GameManager.Instance.saveState();
            PlayerPrefs.SetInt("SetScore", 1);
        }
        stopOne("GamePlayMusic");
        stopOne("BossMusic");
        loadingScreen.Instance.Show("MainMenu");
    }
    public void Pause()
    {
        Time.timeScale = 0;
        PausePanel.gameObject.SetActive(true);
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        PausePanel.gameObject.SetActive(false);
    }
    public void GoToLastBoss()
    {
        GameManager.Instance.ChangeCoin(-(GameManager.Instance.stateData.LastKilledBoss - 2)*300);
        bossKilledNumber = GameManager.Instance.stateData.LastKilledBoss - 2;
        CoolDown.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void GetGurdian()
    {
        GameManager.Instance.ChangeCoin(-300);
        PlayerHealth.Instance.TurnOnTheGurdian();
        CoolDown.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void SetIcon(Sprite icon)
    {
        iconOne.gameObject.SetActive(true);
        iconTwo.gameObject.SetActive(true);

        iconOne.sprite = iconTwo.sprite = icon;
    }
}
