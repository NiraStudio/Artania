using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBoss : AlphaScript {
    public GameObject coin;
    public int coinAmount;
    public GameObject[] lines;
    public float time;
    public float speed;
    bool ready=false,move;
    float t;

    Vector2 Pos;
    int LineId=1;
	// Use this for initialization
	IEnumerator Start () {
        coinAmount =(int)( PlayerAttack.instance.damage * 20);
        lines = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            lines[i] = Camera.main.transform.parent.GetChild(i).gameObject;
        }
        transform.SetParent(lines[1].transform);
        transform.localPosition = Vector2.zero;

        transform.SetParent(null);
        yield return new WaitForSeconds(1);
        GamePlayManager.Instance.play = true;
        GamePlayManager.Instance.Run = true;
        chooseLine();
	}
	
	// Update is called once per frame
	void Update () {
        if (!ready)
            return;
        t += Time.deltaTime;
        if(t>=time)
        {
            print("frog Died");
            die();
            return;
        }
        if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, Pos, speed*Time.deltaTime);
        }

	}
    public void Hit()
    {
        if (ready)
        {
            Vector2 a = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            GameObject g = Instantiate(coin, gamePlayUi.Instance.CoinParent.transform);
            g.transform.position = a;
            g.GetComponent<CoinScript>().value = coinAmount;
        }
    }
    void die()
    {
        ready = false;
        GamePlayManager.Instance.FrogDeath();
        GetComponent<Animator>().SetTrigger("Exit");
        Destroy(gameObject, 4);
    }
    void chooseLine(){
        int Choosen = 0;
        do
        {
            Choosen = Random.Range(0, 3);
        } while (Choosen==LineId);
        LineId = Choosen;
        Pos = lines[Choosen].transform.position;
        GetComponent<Animator>().SetTrigger("Jump");
    }
    public void Jump()
    {
        move = true;
    }
    public void DeJump()
    {
        move = false;
        chooseLine();
    }
    public void Allower()
    {
        ready = true;
    }
    
}
