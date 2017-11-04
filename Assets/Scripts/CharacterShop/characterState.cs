using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterState : MonoBehaviour {
    public Text CharacterName,Attack,FillRate,MaxMana;
    public Color baseColor;
	// Use this for initialization
	
    public void Repaint(Player a)
    {
        for (int i = 0; i < 3; i++)
        {
            Attack.transform.GetChild(i).GetComponent<Image>().color = baseColor;
            FillRate.transform.GetChild(i).GetComponent<Image>().color = baseColor;
            MaxMana.transform.GetChild(i).GetComponent<Image>().color = baseColor;
            
        }
        CharacterName.text = a.Name;
        for (int i = 0; i < a.Attack; i++)
        {
            Attack.transform.GetChild(i).GetComponent<Image>().color = a.color;
        }
        for (int i = 0; i < a.FillRate; i++)
        {
            FillRate.transform.GetChild(i).GetComponent<Image>().color = a.color;
        }
        for (int i = 0; i < a.MaxMana; i++)
        {
            MaxMana.transform.GetChild(i).GetComponent<Image>().color = a.color;
        }
    }
}
