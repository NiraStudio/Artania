using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Backtory.Core.Public;
using UPersian.Components;
public class Profile : MonoBehaviour {
    public Text Name,Score, Level,Rank;
    public Image LevelImage,RankImg;


    public void RePaint(string name,int score,int level,int rank,Sprite levelimage,Sprite RankImage)
    {
        Name.text = name;
        Score.text = score.ToString();
        Level.text = level.ToString();
        Rank.text = rank.ToString();
        LevelImage.sprite= levelimage;
        RankImg.sprite = RankImage;
        //rank img
    }
}
