using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBTN : MonoBehaviour {

    public int id;
    public UltimateScript ultimate;
    GameObject pos;
	// Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        switch (id)
        {
            case 1:
                pos = Camera.main.transform.parent.GetChild(0).gameObject;
                break;
            case 2:
                pos = Camera.main.transform.parent.GetChild(1).gameObject;

                break;
            case 3:
                pos = Camera.main.transform.parent.GetChild(2).gameObject;

                break;
        }
        ultimate.Shoot(pos);
    }
}
