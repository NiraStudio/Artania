using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemConvertorPanel : MonoBehaviour {
    public static GemConvertorPanel Instance;
    public GemConvertor currentRate;
    public Animator quiestionPanel;
    public GemConvertor[] convetors;
    Animator anim;
	// Use this for initialization
	void Start () {
        Instance = this;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Enter()
    {
        anim.SetTrigger("Enter");
        foreach (var item in convetors)
        {
            item.RenewTitle();
        }
    }
    public void Exit()
    {
        anim.SetTrigger("Idle");

    }
    public void buy()
    {
        currentRate.Convert();
    }
    
    public void QuestionPanelExit()
    {
        quiestionPanel.SetTrigger("Idle");
        currentRate = null;
    }
    
    
}
