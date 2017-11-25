using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Alpha.MissionSystem
{
    [System.Serializable]
    public class Mission:ICloneable
    {
        public enum Level
        {
            Common,
            UnCommon,
            Rare,
            Legendary
        }
        public enum Type
        {
            game,
            collectGurdian,
            superPower,
            WatchVideo,
            buyNewGurdian,
            SpendMoney,
            enemykill,
            bossKill,
            Exprience,
            coin
        }
        public string EnglishTitle = "";
        public string PersianTitle = "";
        public int Id;
        public Type type;
        public Level level;
        public int Times;
        public reward Reward;
        public bool InMatch;
        public bool isDone;
        public bool GainReward;
        public bool Increaseable;
        public int CurrentTimes;
        public Mission(string persian,string English,Type type, int times, reward.Type t, int rewardAmount, bool InMatch,int ID)
        {

            this.type = type;
            this.Times = times;
            CurrentTimes = 0;

            this.EnglishTitle = English;
            this.PersianTitle = persian;


            this.Reward = new reward(t, rewardAmount);
            this.InMatch = InMatch;
            this.Id = ID;
        }
        public Mission()
        {
            Reward = new reward(reward.Type.Coin, 0);
        }

        public void CheckForProgress()
        {
            if (CurrentTimes >= Times)
            {
                CurrentTimes = Times;
                isDone = true;
                Debug.Log(EnglishTitle + " Finished");
                MissionCompelete.Instance.Repaint(GameManager.Language(PersianTitle, EnglishTitle));
            }

        }
        public void AddTime(int amount)
        {
            if (!isDone)
            {
                CurrentTimes+=amount;
                Debug.Log(type + " Added");
                CheckForProgress();
            }
        }
        public void RemakeTheTitle()
        {
            if (PersianTitle.ToLower().Contains("تعداد"))
            {
               PersianTitle= PersianTitle.Replace("تعداد", Times.ToString());
            }
            if (EnglishTitle.ToLower().Contains("number"))
            {
                EnglishTitle=EnglishTitle.Replace("number", Times.ToString());

            }
        }
        public void SetTime(int amount)
        {
            if (InMatch&&!isDone)
            {
                CurrentTimes = amount;
                CheckForProgress();
                if (!isDone)
                    CurrentTimes = 0;
            }
        }
            
        public void Restart()
        {
            CurrentTimes = 0;
            GainReward = false;
            isDone = false;
        }

        public object Clone()
        {
            Mission a = new Mission(this.PersianTitle, this.EnglishTitle, this.type, this.Times, this.Reward.type, this.Reward.amount, this.InMatch, this.Id);
            a.Increaseable = this.Increaseable;
            return a;
        }
    }


    [System.Serializable]
    public class reward
    {
        public enum Type
        {
            Gem,
            Coin,
            exp
        }
        public Type type;
        public int amount;

        public reward(Type type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
