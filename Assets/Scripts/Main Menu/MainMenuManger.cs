using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UPersian.Components;
using System.IO;
using Backtory.Core.Public;

public class MainMenuManger : AlphaScript {

    public static MainMenuManger instance;
    public GameObject[] missions;
    public Transform CharacterPos;
    public Text CoinAmount;
    public Text GemAmount;
    public Animator missionTabAnim;
    public GameObject MissionMark,shopAlert;
    public GameObject MissionFree, MissionGem;
    public RtlText MissionName;
    public RtlText PlayText;
    public Slider XPSlider;
    public Text expText;
    public MeatPanel meatPanel;
    public RewardPanel rewardPanel;
    [Header("Medals")]
    public Image medal;
    public Sprite[] Medals;
    public Sprite gem, coin;
    float CurrentCoin, CurrentGem,currentXP;
    bool play;
    Sprite asprite;
    IEnumerator Start()
    {
        instance = this;
        MissionMark.transform.parent.GetComponent<Button>().onClick.AddListener(MissionEnter);
        Time.timeScale = 1;
        if (PlayerPrefs.GetString("Username") != "")
            StartCoroutine(upload());
        if (GameManager.Instance.stateData.hadBoughtFirstOffer == false)
            shopAlert.SetActive(true);
        loadMissions();
        XPSlider.maxValue = GameManager.Instance.expNeed();
        GameObject g = Instantiate(GameManager.Instance.PlayerDataBase.GetPlayerById(GameManager.Instance.characterData.charcaterId).Shape.transform.GetChild(0), CharacterPos).gameObject; g.transform.localPosition = Vector2.zero;
        g.GetComponent<Animator>().SetTrigger("Idle");
        yield return new WaitForSeconds(0.3f);
        g.GetComponent<PlayerHealth>().TurnOnTheGurdian();

        loadingScreen.Instance.Disapear();
        CheckTimeForMissionButton();

    }
    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("SetScore") == 1 && InternetChecker.Instance.internetConnectBool && PlayerPrefs.GetString("Username")!="")
        {
            MainLeaderBoardEvent Event = new MainLeaderBoardEvent(GameManager.Instance.stateData.HighScore, GameManager.Instance.stateData.lvl);
            Event.SendInBackground(null);
            PlayerPrefs.SetInt("SetScore", 0);
        }

        if (CurrentCoin != GameManager.Instance.currencyData.Coin && GameManager.Instance.currencyData.Coin != 0)
            CurrentCoin = Mathf.MoveTowards(CurrentCoin, GameManager.Instance.currencyData.Coin, (GameManager.Instance.currencyData.Coin / 1.5f) * Time.fixedDeltaTime);
        else if (GameManager.Instance.currencyData.Coin == 0)
            CurrentCoin = 0;

        CurrentGem = GameManager.Instance.currencyData.Gem;

         


        if (currentXP != GameManager.Instance.stateData.EXP && GameManager.Instance.stateData.EXP != 0)
            currentXP = Mathf.MoveTowards(currentXP, GameManager.Instance.stateData.EXP, GameManager.Instance.stateData.EXP * Time.fixedDeltaTime);

        else if (GameManager.Instance.stateData.EXP == 0)
            currentXP = 0;

        CoinAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentCoin).ToString(), CoinAmount).ToString();
        GemAmount.text = "X  " + GameManager.NumberPersian(((int)CurrentGem).ToString(), GemAmount).ToString();
        

        XPSlider.value = currentXP;
        expText.text = GameManager.NumberPersian(((int)currentXP).ToString(), expText) + "/" + GameManager.NumberPersian(GameManager.Instance.expNeed().ToString(), expText);

        if (PlayerPrefs.GetInt("LevelUp") == 1 && !LevelUpPanel.Instance.On)
        {
            LevelUpPanel.Instance.Repaint();
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        //quitPanel

        MissionName.text = GameManager.Language("ماموریت ها", "Missions", MissionName);
        PlayText.text = GameManager.Language("شروع", "Start", PlayText);

    }
    void CheckTimeForMissionButton()
    {
        DateTime a=GameManager.Instance.stateData.MissionTime;
        print("Mission time is " + a);
        DateTime b = new System.DateTime(a.Year, a.Month, a.Day, 0, 0, 0);
        b = b.AddDays(1);
        if (System.DateTime.Now >= b)
        {
            MissionFree.SetActive(true);
            MissionGem.SetActive(false);

        }
        else
        {
            MissionFree.SetActive(false);
            MissionGem.SetActive(true);
        }
    }
    public void GoToScene(string Name)
    {
        playSound("Button");
        loadingScreen.Instance.Show(Name);
    }
    IEnumerator upload()
    {
        yield return new WaitUntil(() => InternetChecker.Instance.internetConnectBool);
        UploadData();
        yield return new WaitForSeconds(10);
        StartCoroutine(upload());
    }
    public void loadMissions()
    {
        MissionMark.SetActive(false);
        for (int i = 0; i < missions.Length; i++)
        {
            missions[i].transform.GetChild(3).gameObject.SetActive(false);
            missions[i].transform.GetChild(0).GetComponent<RtlText>().text = GameManager.Language(MissionController.Instance.missions[i].PersianTitle, MissionController.Instance.missions[i].EnglishTitle, missions[i].transform.GetChild(0).GetComponent<RtlText>());
            missions[i].transform.GetChild(1).GetComponent<RtlText>().text = GameManager.NumberPersian(MissionController.Instance.missions[i].CurrentTimes.ToString(), missions[i].transform.GetChild(1).GetComponent<RtlText>()) + "/" + GameManager.NumberPersian(MissionController.Instance.missions[i].Times.ToString(), missions[i].transform.GetChild(1).GetComponent<RtlText>());
            missions[i].transform.GetChild(2).GetComponent<RtlText>().text = GameManager.NumberPersian(MissionController.Instance.missions[i].Reward.amount.ToString(), missions[i].transform.GetChild(2).GetComponent<RtlText>());
            
            switch (MissionController.Instance.missions[i].Reward.type)
            {
                case Alpha.MissionSystem.reward.Type.Gem:
                    asprite = gem;
                    break;
                case Alpha.MissionSystem.reward.Type.Coin:
                    asprite = coin;
                    break;
                case Alpha.MissionSystem.reward.Type.exp:
                    break;
            }
            missions[i].transform.GetChild(4).GetComponent<Image>().sprite = asprite;
            print("Sprite is " + asprite.name);
            if (MissionController.Instance.missions[i].GainReward)
                missions[i].transform.GetChild(3).gameObject.SetActive(true);
            if (MissionController.Instance.missions[i].isDone && !MissionController.Instance.missions[i].GainReward)
                MissionMark.SetActive(true);

        }
    }
    public void Play()
    {
        if (!play)
        {
            CharacterPos.transform.GetChild(0).GetComponent<Animator>().SetBool("Run", true);
            CharacterPos.GetComponent<Animator>().SetTrigger("Play");
            play = true;
            PlayText.gameObject.SetActive(false);
        }
    }
    
    public void GetReward(int i)
    {
        if (MissionController.Instance.missions[i].isDone && !MissionController.Instance.missions[i].GainReward)
        {
            Alpha.MissionSystem.reward r = MissionController.Instance.missions[i].Reward;
            switch (r.type)
            {
                case Alpha.MissionSystem.reward.Type.Gem:
                    GameManager.Instance.ChangeGem(r.amount);
                    GameManager.Instance.saveCurrency();
                    break;
                case Alpha.MissionSystem.reward.Type.Coin:
                    GameManager.Instance.ChangeCoin(r.amount);
                    GameManager.Instance.saveCurrency();
                    break;
                case Alpha.MissionSystem.reward.Type.exp:
                    GameManager.Instance.AddEXP(r.amount);
                    GameManager.Instance.saveState();
                    break;
            }
            MissionController.Instance.missions[i].GainReward = true;
            MissionController.Instance.SendMissions();
            GameManager.Instance.saveState();
            missions[i].transform.GetChild(3).gameObject.SetActive(true);
            loadMissions();
        }
    }
    public void makeNewMissions()
    {
        playSound("Button");

        MissionController.Instance.MakeMission();
        GameManager.Instance.saveState();
        loadMissions();
        GameManager.Instance.stateData.MissionTime = DateTime.Now;
        CheckTimeForMissionButton();
    }
    public void makeNewMissionsGem()
    {
        if (GameManager.Instance.currencyData.Gem >= 5)
        {
            playSound("Button");
            GameManager.Instance.ChangeGem(-1);    
            MissionController.Instance.MakeMission();
            GameManager.Instance.saveState();
            loadMissions();
            GameManager.Instance.stateData.MissionTime = DateTime.Now;

        }
    }
    public void UploadData()
    {
        ///upload St.alpha
        if (File.Exists(Application.persistentDataPath + "/Data/ST.alpha"))
        {
            BacktoryFile backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/ST.alpha");
            backtoryFile.UploadInBackground("/playersdata/" + PlayerPrefs.GetString("Username") + "/", true, (response) =>
            {
                if (response.Successful)
                {


                    ///Upload CH.alpha

                    if (File.Exists(Application.persistentDataPath + "/Data/CH.alpha"))
                    {
                        backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/CH.alpha");
                        backtoryFile.UploadInBackground("/playersdata/" + PlayerPrefs.GetString("Username") + "/", true, (res) =>
                        {
                            if (res.Successful)
                            {

                                ///Upload CR.alpha
                                ///

                                if (File.Exists(Application.persistentDataPath + "/Data/CR.alpha"))
                                {
                                    backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/CR.alpha");
                                    backtoryFile.UploadInBackground("/playersdata/" + PlayerPrefs.GetString("Username") + "/", true, (r) =>
                                    {
                                        if (r.Successful)
                                        {

                                            string filePathOnServer = response.Body;
                                            Debug.Log("Upload was successful. File path on server (url) is " + filePathOnServer);
                                            print("All Of Them Was SucssesFull");
                                        }
                                        else
                                        {
                                            Debug.Log("failed; " + response.Message + " " + response.Code);

                                        }
                                    });
                                }


                            }
                            else
                            {
                                Debug.Log("failed; " + response.Message + " " + response.Code);

                            }
                        });
                    }






                }
                else
                {
                    Debug.Log("failed; " + response.Message + " " + response.Code);
                }
            });
        }
    }
    public void MissionEnter()
    {
        playSound("Button");

        if(missionTabAnim.GetCurrentAnimatorStateInfo(0).IsName("Enter"))
        {
            missionTabAnim.SetTrigger("Exit");

        }
        else
        {
            missionTabAnim.SetTrigger("Enter");

        }
    }
    public void OpenShop()
    {
        GameManager.Instance.ChangeGem(1000);
        Shop.Instance.Enter();
    }
    
    

}
