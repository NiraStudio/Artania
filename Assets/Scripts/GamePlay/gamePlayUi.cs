using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamePlayUi : MonoBehaviour {
    public static gamePlayUi Instance;
    public GamePlayManager gamePlayeManager;
    public Transform ControllerParent;
    public UPersian.Components.RtlText EnemiesText;
    public GameObject Coin;
    public GameObject CoinParent,SuperPowerParent;
    public GameObject shieldImage;
    int enemiesNumber;
	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gamePlayeManager.enemiesNumber == 0)
            EnemiesText.text = GameManager.Language(EnemiesController.Instance.boss.transform.GetChild(0).GetComponent<Boss>().persianName, EnemiesController.Instance.boss.transform.GetChild(0).GetComponent<Boss>().BossName, EnemiesText);
        else
            EnemiesText.text = GameManager.NumberPersian(gamePlayeManager.enemiesNumber.ToString(), EnemiesText);
	}
    
    public void EnemyTextActive(bool active)
    {
        EnemiesText.gameObject.SetActive(active);
    }
}
