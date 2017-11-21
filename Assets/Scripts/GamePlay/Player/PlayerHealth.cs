using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : AlphaScript
{
    public static PlayerHealth Instance;
    public Transform GurdianParent;
    public GameObject Gurd;
    public Gurdian gurdian;
    public Sprite gurdSprite;
    public string HitSoundCode;
    public int Counters, Protected,coinMulti=1,EXPMulti=1;
    public float secondsWaitToDeath;
    AudioSource au;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        au = GetComponent<AudioSource>();
    }
    void Start()
    {
        coinMulti = EXPMulti = 1;
        Gurd = GameManager.Instance.gurdianDataBase.GetPlayerById(GameManager.Instance.characterData.GurdianId).Prefab;
        gurdSprite = Gurd.GetComponent<SpriteRenderer>().sprite;
        GameObject b = Instantiate(Gurd, GurdianParent);
        b.transform.localPosition = Vector3.zero;
        gurdian = b.GetComponent<Gurdian>();
        gurdian.Kind = GameManager.Instance.gurdianDataBase.GetPlayerById(GameManager.Instance.characterData.GurdianId).Kind;
        Gurd.SetActive(false);
       // DetecdTheModel();
      // TurnOnTheGurdian();

    }

    // Update is called once per frame
   

    public IEnumerator Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        GamePlayManager.Instance.pauseAllow = false;
        GetComponent<Animator>().SetTrigger("Die");
        GamePlayManager.Instance.play = false;
        GamePlayManager.Instance.Run = false;
        yield return new WaitForSeconds(secondsWaitToDeath);

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            FinishCounter.Instance.ReStartGame();
        }
        else
        {
            FinishCounter.Instance.Enter();
        }
    }
    public void die(Bullet b,bool unCounterable)
    {
        float dmg = GetComponent<PlayerAttack>().damage;

        if (!powerUpManager.Instance.superPower)
        {
            if (Counters == 2)
            {
                if (!unCounterable)
                {
                    b.dir = 1;
                    b.speed = 30;
                    b.AttackerType = Bullet.attackerType.player;
                    b.dmg = dmg;
                    b.tag = "PlayerBullet";
                    print("Countered");
                    playSound("Counter");
                }
                else
                    playSound(HitSoundCode);


                Counters--;

            }
            else if (Counters == 1)
            {
                if (!unCounterable)
                {
                    b.dir = 1;
                    b.speed = 30;
                    b.AttackerType = Bullet.attackerType.player;
                    b.dmg = dmg;
                    b.tag = "PlayerBullet";
                    print("Countered");
                    playSound("Counter");

                }
                else
                    playSound(HitSoundCode);



                gurdian.gameObject.SetActive(false);
                Counters--;

            }

            else if (Protected == 2)
            {
                Protected--;
                if (!unCounterable)
                    b.Die();
                playSound(HitSoundCode);



            }
            else if (Protected == 1)
            {
                gurdian.gameObject.SetActive(false);
                Protected--;
                if (!unCounterable)
                    b.Die();
                playSound(HitSoundCode);



            }
            else
            {
                StartCoroutine(Die());
                stopOne("GamePlayMusic");
                stopOne("BossMusic");
                if (!unCounterable)
                    b.Die();
            }

        }
        else
        {
            if(!unCounterable)
                b.Die();
        }
    }
    public void TurnOnTheGurdian()
    {
        gurdian.gameObject.SetActive(true);
        DetecdTheModel();

    }
    public void DetecdTheModel()
    {

        switch (gurdian.Kind)
        {
            
            case Gurdian.GurdianModel.Normal:
                Protected = 1;
                Counters = 0;
                coinMulti = EXPMulti = 1;
                break;
            case Gurdian.GurdianModel.Double:
                Protected = 2;
                Counters = 0;
                coinMulti = EXPMulti = 1;
                break;
            case Gurdian.GurdianModel.Counter:
                Protected = 0;
                Counters = 1;
                coinMulti = EXPMulti = 1;
                break;
            case Gurdian.GurdianModel.DoubleCounter:
                Protected = 0;
                Counters = 2;
                coinMulti = EXPMulti = 1;
                break;
            case Gurdian.GurdianModel.DoubleCoin:
                Protected = 1;
                Counters = 0;
                EXPMulti = 1;
                coinMulti = 2;
                break;
            case Gurdian.GurdianModel.DoubleEXP:
                Protected = 1;
                Counters = 0;
                EXPMulti = 2;
                coinMulti = 1;
                break;
            case Gurdian.GurdianModel.TripleCoin:
                Protected = 1;
                Counters = 0;
                coinMulti = 3;
                EXPMulti = 1;
                break;
            case Gurdian.GurdianModel.TripleEXP:
               Protected = 1;
                Counters = 0;
                coinMulti = 1;
                EXPMulti = 3;
                break;
            case Gurdian.GurdianModel.GodLike:
                Protected = 0;
                Counters = 2;
                coinMulti =2;
                EXPMulti = 2;

                break;
        }
    }

}
