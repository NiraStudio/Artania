using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Backtory.Core.Public;
using UPersian.Components;

public class tutorialPanel : MonoBehaviour
{
   
    public bool clicked;
    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("FirstTime") != 0)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Application.isMobilePlatform)
        {

            Touch[] t = Input.touches;
            if (t.Length > 0 && t.Length <= 2)
            {

                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i].phase == TouchPhase.Ended)
                    {
                        
                            clicked = true;
                            GetComponent<Image>().color = Color.clear;
                            gameObject.SetActive(false);
                        
                    }
                }
            }
        }
        else if (Application.isEditor)
            if (Input.GetMouseButtonDown(0))
            {
               
                    clicked = true;
                    GetComponent<Image>().color = Color.clear;
                    gameObject.SetActive(false);
                

            }

    }
  
}
