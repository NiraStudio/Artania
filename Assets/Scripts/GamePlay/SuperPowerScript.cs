using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerScript : AlphaScript
{
    public float speed;
    public float radius;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.play)
        {
            Vector2 a = transform.position;
            a.x -= speed * Time.deltaTime;
            a.y = Mathf.Cos(a.x)+radius;
            transform.position = a;
            if (Application.isMobilePlatform)
            {
                Touch[] t = Input.touches;
                if (t.Length > 0 && t.Length <= 2)
                {

                    for (int i = 0; i < t.Length; i++)
                    {
                        if ( GamePlayManager.Instance.play)
                        {
                            switch (t[i].phase)
                            {
                                case TouchPhase.Began:
                                    Vector2 p = Camera.main.ScreenToWorldPoint(t[i].position);
                                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                                    if (hit != null && hit.collider == gameObject.GetComponent<Collider2D>())
                                    {
                                        OnClick();
                                    }
                                    break;

                            }
                        }
                    }
                }
            }
            else if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                    if (hit != null && hit.collider == gameObject.GetComponent<Collider2D>())
                    {
                        OnClick();
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnClick()
    {
        if (GamePlayManager.Instance.play  && Time.timeScale == 1)
        {
            powerUpManager.Instance.SuperPowerGet();
            PlayerAttack.instance.ChargeTheEnergy();
            MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.superPower, 1);

            Destroy(gameObject);
        }
    }
    
}
