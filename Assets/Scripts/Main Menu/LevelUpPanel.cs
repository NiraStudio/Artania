using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class LevelUpPanel : AlphaScript
{
    public static LevelUpPanel Instance;
    public AudioClip Sound;
    public RtlText LevelTxt, RewardTxt,levelUpText;
    public bool On;

    int rewardAmount;
    AudioSource aud;
    Animator anim;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
    void Start()
    {
        aud = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
	
    public void Repaint()
    {
        
        LevelTxt.text=GameManager.NumberPersian(GameManager.Instance.stateData.lvl.ToString(),LevelTxt);
        levelUpText.text = GameManager.Language("ارتقا", "Level Up", levelUpText);
        On = true;
        //rewardLogic
        anim.SetTrigger("Enter");
        playSound("LevelUp");

    }
    public void GetReward()
    {
        playSound("Button");
        PlayerPrefs.SetInt("LevelUp", 0);
        PlayerPrefs.SetInt("LastLevel", GameManager.Instance.stateData.lvl);
        anim.SetTrigger("Idle");
        On = false;
    }
}
