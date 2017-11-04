using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alpha.MissionSystem;
using System;
public class MissionController : AlphaScript
{
    public static MissionController Instance;
    [SerializeField]
    MissionDataBase DB;
    public Mission[] missions=new Mission[3];
	// Use this for initialization
	void Start () {
        Instance = this;
       // MakeMission();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddToMission(Mission.Type type,int amount)
    {
        foreach (Mission m in missions)
        {
            if(m.type==type)
            {
                m.AddTime(amount);
            }
        }
    }
    public void setMission(Mission.Type type, int amount)
    {
        foreach (Mission m in missions)
        {
            if (m.type == type)
            {
                m.SetTime(amount);
            }
        }
    }
    public void restart()
    {
        foreach (Mission m in missions)
        {
            if (!m.isDone&&m.InMatch)
            {
                m.Restart();
            }
        }
    }
    public void LoadMission()
    {
        missions = GameManager.Instance.stateData.misssions;
    }
    public void MakeMission()
    {
        Mission m = new Mission();
        Mission[] a = new Mission[3];
        missions = new Mission[3];
        do
        {
            for (int i = 0; i < 3; i++)
            {
                a[i] = DB.GetRandomMission();
                a[i].Restart();

                missions[i] = (Mission)a[i].Clone();
            }
        } while (missions[0].Id == missions[1].Id || missions[1].Id == missions[2].Id || missions[0].Id == missions[2].Id);

        int b = GameManager.Instance.stateData.lvl / 3;
        if (b > 0)
        {
            for (int i = 0; i < 3; i++)
            {

                if (missions[i].Increaseable)
                {
                    missions[i].Times =(int)( missions[i].Times * (Mathf.Pow(b,2)));
                    missions[i].Reward.amount = (int)(missions[i].Reward.amount * (Mathf.Pow(b,1.5f)));
                    missions[i].RemakeTheTitle();

                }
            }
        }
        else
            for (int i = 0; i < 3; i++)
                missions[i].RemakeTheTitle();


        GameManager.Instance.stateData.misssions = missions;
    }
    public void SendMissions ()
    {
        GameManager.Instance.stateData.misssions = missions;
    }
}
