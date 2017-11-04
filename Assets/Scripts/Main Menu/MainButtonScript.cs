using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonScript : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeState()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Enter"))
        {
            anim.SetTrigger("Exit");

        }
        else
        {
            anim.SetTrigger("Enter");

        }
    }
}
