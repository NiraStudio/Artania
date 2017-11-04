using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalBehavior : MonoBehaviour {
    public static MedalBehavior Instance;
    public Sprite[] medals;
    public Image medal;
    public Text level;
	// Use this for initialization
	void Start () {
        Instance = this;
        StartCoroutine( RenewMedalIcon());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     IEnumerator RenewMedalIcon()
    {
        medal.sprite = medals[GameManager.Instance.stateData.lvl / 8];
        level.text =GameManager.NumberPersian( GameManager.Instance.stateData.lvl.ToString(),level);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RenewMedalIcon());

    }
}
