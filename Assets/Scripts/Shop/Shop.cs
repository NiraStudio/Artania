using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class Shop : MonoBehaviour {
    public static Shop Instance;
    public Text[] Infos;
    Animator anim;
	// Use this for initialization
	void Start () {
        Instance = this;
        anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Exit()
    {
        anim.SetTrigger("Idle");
    }
    public void Enter()
    {
        for (int i = 0; i < Infos.Length; i++)
        {
            string a = Infos[i].text;
            Infos[i].text =NumberPersian(a, Infos[i]);
        }
        anim.SetTrigger("Enter");
    }
    public string NumberPersian(string number, Text t)
    {
        string a = "";

        char[] b = number.ToString().ToCharArray();
        for (int i = 0; i < b.Length; i++)
        {
            switch (b[i])
            {
                case '1':
                    a += "۱";
                    break;
                case '2':
                    a += "۲";
                    break;
                case '3':
                    a += "۳";
                    break;
                case '4':
                    a += "۴";
                    break;
                case '5':
                    a += "۵";
                    break;
                case '6':
                    a += "۶";
                    break;
                case '7':
                    a += "۷";
                    break;
                case '8':
                    a += "۸";
                    break;
                case '9':
                    a += "۹";
                    break;
                case '0':
                    a += "۰";
                    break;

                default:
                    a += b[i];
                    break;
            }


        }
        return a;
    }
}
