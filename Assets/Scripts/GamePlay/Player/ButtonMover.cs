using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMover : MonoBehaviour {
    public GameObject player;
    public float MaxPos;
    public float SwipeSpeed;
    Vector3 Pos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckForTouch();
        if (player.transform.localPosition != Pos)
            player.transform.localPosition = Vector2.MoveTowards(player.transform.localPosition, Pos, SwipeSpeed * Time.deltaTime);
	}
    public void moveUp()
    {
        if (!GamePlayManager.Instance.Run)
            GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().PlayFootStep();

        if (GamePlayManager.Instance.play)
        {
            if (Pos.y < MaxPos)
                Pos.y += MaxPos;
        }
    }
    public void moveDown()
    {
        if(!GamePlayManager.Instance.Run)
            GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().PlayFootStep();

        if (GamePlayManager.Instance.play)
        {
            if (Pos.y > -MaxPos)
                Pos.y -= MaxPos;
        }
    }
    public void CheckForTouch()
    {
        Touch[] t = Input.touches;
        if (t.Length > 0 && t.Length <= 2)
        {

            for (int i = 0; i < t.Length; i++)
            {
                if (!EventSystem.current.IsPointerOverGameObject(t[i].fingerId) && GamePlayManager.Instance.play)
                {
                    switch (t[i].phase)
                    {
                        case TouchPhase.Began:
                            Vector2 p = Camera.main.ScreenToWorldPoint(t[i].position);
                            if(Mathf.Abs (player.transform.localPosition.y-p.y)>=1)
                            {
                                if (player.transform.localPosition.y > p.y)
                                    moveDown();
                                else if (player.transform.localPosition.y < p.y)
                                    moveUp();
                            }
                            break;
                        
                    }
                }
            }
        }
    }
}
