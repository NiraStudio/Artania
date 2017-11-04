using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingScript : AlphaScript
{
    public MainMenuManger mainMenu;
    public Image BtnImg;
    public Sprite Fa, En;
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        if (PlayerPrefs.GetString("Language") == "English")
        {
            BtnImg.sprite = En;
        }else
            BtnImg.sprite = Fa;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Enter()
    {
        anim.SetTrigger("Enter");
    }
    public void Exit()
    {
        anim.SetTrigger("Idle");
    }
    public void ChangeLanguage()
    {
        print("Change Language");
        playSound("Button");
        if (PlayerPrefs.GetString("Language") != "")
        {
            if (PlayerPrefs.GetString("Language") == "Persian")
            {
                PlayerPrefs.SetString("Language", "English");
                BtnImg.sprite = En;
                MissionController.Instance.LoadMission();

            }
            else if (PlayerPrefs.GetString("Language") == "English")
            {

                PlayerPrefs.SetString("Language", "Persian");
                BtnImg.sprite = Fa;

            }
                mainMenu.loadMissions();
        }
        else
        {
            PlayerPrefs.SetString("Language", "English");
            ChangeLanguage();
        }
    }
}
