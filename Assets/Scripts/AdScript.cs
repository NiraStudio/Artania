using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapsellSDK;

public class AdScript : AlphaScript
{
    public enum rewardKind
    {
        gem,
        coin,
        restart,
        spinWheel
    }
    public rewardKind kind;
    public int amount;
    public string ZoneID;
    public bool Animation;
    bool HaveAd,seachring;
    string AdID;
    public SpinWheel spin;
    public IdlePanel script;

	// Use this for initialization
	void Start () {

        findAd();
        GetComponent<Button>().onClick.AddListener(SHowAd);
       
        
	}
	
	// Update is called once per frame
	void Update () {
        if (HaveAd)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;


	}
    
    void findAd()
    {
        seachring = true;

        Tapsell.requestAd(ZoneID, true,
    (TapsellResult result) =>
    {
        // onAdAvailable
        Debug.Log("Action: onAdAvailable");
        AdID = result.adId; // store this to show the ad later
        /*TapsellShowOptions showOptions = new TapsellShowOptions();
        showOptions.backDisabled = false;
        showOptions.immersiveMode = false;
        showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
        showOptions.showDialog = true;
        Tapsell.showAd(ad, showOptions);*/
        HaveAd = true;
        if (Animation)
            GetComponent<Animator>().SetTrigger("Enter");
    },

    (string zoneId) =>
    {
        // onNoAdAvailable
        Debug.Log("No Ad Available");

    },

    (TapsellError error) =>
    {
        // onError
        Debug.Log(error.error);

    },

    (string zoneId) =>
    {
        // onNoNetwork
        Debug.Log("No Network");

    },

    (TapsellResult result) =>
    {
        // onExpiring
        Debug.Log("Expiring");

        // this ad is expired, you must download a new ad for this zone
    }
);
    }
    public void SHowAd()
    {
        Time.timeScale = 0;
        TapsellShowOptions showOptions = new TapsellShowOptions();
        showOptions.backDisabled = true;
        showOptions.immersiveMode = false;
        showOptions.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
        showOptions.showDialog = true;
        if (GamePlayManager.Instance)
            GameManager.Instance.seeAd = true;
        Tapsell.setRewardListener((TapsellAdFinishedResult result) =>
        {
            switch (kind)
            {
                case rewardKind.gem:
                    GameManager.Instance.ChangeGem(amount);
                    break;
                case rewardKind.coin:
                    script.calmThree();
                    break;
               
                case rewardKind.restart:
                    FinishCounter.Instance.ReStartGame();
                    break;

                case rewardKind.spinWheel:
                    spin.startSpin();
                    break;

            }
            Time.timeScale = 1;
            MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.WatchVideo, 1);
            if (MainMenuManger.instance)
                MainMenuManger.instance.loadMissions();
            HaveAd = false;
            findAd();

        }
);
        Tapsell.showAd(AdID, showOptions);

    }
}
