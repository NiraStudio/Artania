using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : AlphaScript
{
    public string EnemyName;
    public int Count;
    public float Hp;
    public int EXP;
    [Header("Sounds")]
    public Slider HpBar;
    public Vector3 SPoint;
    public GameObject bullet;
    public Transform ShoortPos;
    GameObject coinP,EXPP;
    public bool ready,Tutorial;
    public float MonsterValue ;
    int coin;
    float speed=20;
    protected Animator anim;
    Vector2 pos;
    float t;
    int bossKilled;



    bool doubleCoin,DoubleEXP;
	public void start () {
        doubleCoin = GameManager.Instance.currencyData.IsDoubleCoin;
        DoubleEXP= GameManager.Instance.currencyData.IsDoubleExp;
        coinP = (GameObject)Resources.Load("Prefabs/Coin", typeof(GameObject));
        EXPP = (GameObject)Resources.Load("Prefabs/EXPText", typeof(GameObject));
        anim = GetComponent<Animator>();
        HpBar.maxValue = Hp;
        bossKilled = GamePlayManager.Instance.bossKilledNumber;
        coin = 5;
        setHpBar(false);
	}
	public void CheckPerFrame () {
        HpBar.value = Hp;
		if(Hp<=0&&ready)
        {
            ready = false;
            Hp = 0;
            Die();
        }

	}
    
    public void Die()
    {
        //animation
        anim.SetTrigger("Die");
    }
    public void getDamage(float Dmg)
    {
        if (ready)
            Hp -= Dmg;
    }
    public void Move()
    {
        if (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, Vector2.zero, speed * Time.deltaTime);
        }
        else if(Vector3.Distance(transform.localPosition,Vector3.zero)<2&&Hp>1)
        {
            setHpBar(true);
            ready = true;
            GetComponent<BoxCollider2D>().enabled = true;

        }
    }
    public Vector3 StartPoint()
    {
        return SPoint;
    }
    public void destroy()
    {
        
        MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.enemykill, 1);
        EnemiesController.Instance.Remove(gameObject);
        Destroy(gameObject);

    }
    public void FirstDeathStep()
    {
        playSound("Enemy Explotion");
        Hp = 0.01f;
        GetComponent<BoxCollider2D>().enabled = false;
        setHpBar(false);
        ready = false;
        speed = 0;
        transform.SetParent(null);
        transform.GetChild(0).gameObject.SetActive(false);
        powerUpManager.Instance.MakePowerUp(transform.position);

    }
   /* public void RenewState()
    {

        
        MonsterValue = (int)Random.Range(MinCoin, MaxCoin);
        MonsterValue = (MonsterValue + (MonsterValue * (0.25f * bossKilled))) + (MonsterValue * (0.25f * (int)(GameManager.Instance.stateData.lvl / 8)));
    }*/
    public void reNewState(float coin,float exp , float hp)
    {
        Hp = Count * hp;
        EXP = (int)(exp * Count);
        MonsterValue = coin * Count;
    }
    public void GoldBrust()
    {
        if (!Tutorial)
        {
            float value = MonsterValue * (PlayerHealth.Instance.coinMulti + (powerUpManager.Instance.coinMulti ? 1 : 0 + (doubleCoin ? 1 : 0)));

            Vector2 a = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            GameObject g = Instantiate(coinP, gamePlayUi.Instance.CoinParent.transform);
            g.transform.position = a;
            g.GetComponent<CoinScript>().value = value;

            Vector2 b = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            GameObject f = Instantiate(EXPP, b, Quaternion.identity); f.transform.SetParent(gamePlayUi.Instance.gameObject.transform);
            int amount = (EXP * (PlayerHealth.Instance.EXPMulti + (powerUpManager.Instance.EXPMulti ? 1 : 0+ (DoubleEXP ? 1 : 0))));
            f.GetComponent<Text>().text = amount + " EXP";
            GamePlayManager.Instance.addEXP(amount);

        }
    }
    public void Shoot()
    {
        Instantiate(bullet, ShoortPos.position, Quaternion.identity);

    }
    public void setHpBar(bool active){
        HpBar.gameObject.SetActive(active);
    }
    public void attack()
    {
        if ( Hp >= 0.02f&&ready)
            anim.SetTrigger("Attack");
    }
    
}
