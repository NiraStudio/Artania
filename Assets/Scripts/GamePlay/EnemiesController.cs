using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public static EnemiesController Instance;
    public GameObject TutorialBoss;
    public float MinTime, maxTime,TapalTime;
    public Transform[] Positions;
    public GameObject[] bosses;
    public GameObject[] enemies;
    public bool allow = true;
    public Light[] Lights;
    List<Transform> freePlaces = new List<Transform>();
    List<GameObject> Enemies = new List<GameObject>();
    public GameObject boss,tapal;
    int index = 0;
    float aveCoin, aveExp, aveHp,tapalT;

    int NotChoosen, number1, number2;
    float time, t;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        time = Random.Range(MinTime, maxTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.play)
        {
            t += Time.fixedDeltaTime;
            if (t >= time)
            {
                ChooseAndShoot();
            }
        }
    }
    public void Remove(GameObject enemy)
    {
        Enemies.Remove(enemy);
        GamePlayManager.Instance.ReduceEnemyNumber();
    }
    public void Add(GameObject enemy)
    {
        Enemies.Add(enemy);
    }
    public void ChooseAndShoot()
    {
        bool a = true;
        bool b = true;
        if (Enemies.Count > 2)
        {
            {
                while (a)
                {
                    number1 = Random.Range(0, 3);
                    do
                    {
                        number2 = Random.Range(0, 3);
                    } while (number2 == number1);
                    if (number1 == NotChoosen || number2 == NotChoosen)
                        a = false;

                    for (int i = 0; i < 3; i++)
                        if (number1 != i && number2 != i)
                            NotChoosen = i;


                }
                if (Enemies[number1]!=null  && Enemies[number2]!=null)
                {
                    b = false;
                    if (Enemies[number1].GetComponent<Enemy>() && Enemies[number1].GetComponent<Enemy>().ready && !Enemies[number1].GetComponent<Rhino>() && !Enemies[number1].GetComponent<Tapal>())
                        Enemies[number1].GetComponent<Enemy>().attack();
                    if (Enemies[number2].GetComponent<Enemy>() && Enemies[number2].GetComponent<Enemy>().ready && !Enemies[number2].GetComponent<Rhino>() && !Enemies[number2].GetComponent<Tapal>())
                        Enemies[number2].GetComponent<Enemy>().attack();
                    t = 0;
                }
            } while (b) ;
        }else if (Enemies.Count == 2)
        {
            if (Enemies[0].GetComponent<Enemy>() && Enemies[0].GetComponent<Enemy>().ready && !Enemies[0].GetComponent<Rhino>() && !Enemies[0].GetComponent<Tapal>())
                Enemies[0].GetComponent<Enemy>().attack();
            if (Enemies[1].GetComponent<Enemy>() && Enemies[1].GetComponent<Enemy>().ready && !Enemies[1].GetComponent<Rhino>() && !Enemies[1].GetComponent<Tapal>())
                Enemies[1].GetComponent<Enemy>().attack();
            t = 0;
        }
        else if(Enemies.Count==1)
        {
            if (Enemies[0].GetComponent<Enemy>().ready && !Enemies[0].GetComponent<Rhino>())
            {
                Enemies[0].GetComponent<Enemy>().attack();
                t = 0;
            }

        }
    }
    public string BossName()
    {
        if (boss != null)
            return boss.transform.GetChild(0).GetComponent<Boss>().BossName;
        else
            return "";
    }
    public void chooseBoss()
    {

        if (!GetComponent<GamePlayManager>().Tutorial)
        {
            boss = bosses[GetComponent<GamePlayManager>().bossKilledNumber];
            enemies = boss.transform.GetChild(0).GetComponent<Boss>().enemies;
            GamePlayManager.Instance.ChangeEnemieNumer(enemies.Length);
            index = 0;
            allow = true;
            StartCoroutine(Spawn());
           // StartCoroutine(spawnTapal());
            tapalT = TapalTime;
            print("Boss Choosed =" + BossName());
        }
        else
        {
            boss = TutorialBoss;
            enemies = TutorialBoss.transform.GetChild(0).GetComponent<Boss>().enemies;
            GamePlayManager.Instance.ChangeEnemieNumer(enemies.Length);
            index = 0;
            allow = true;
            StartCoroutine(Spawn());
           
            print("Boss Choosed =" + BossName());
        }
        for (int i = 0; i < Lights.Length; i++)
        {
            Lights[i].color = boss.transform.GetChild(0).GetComponent<Boss>().lightColor;
        }
        int count = 0;
        for (int i = 0; i < enemies.Length; i++)
            if (enemies[i].GetComponent<Enemy>())
                count += enemies[i].GetComponent<Enemy>().Count;
        Boss a = boss.transform.GetChild(0).GetComponent<Boss>();
        aveCoin = a.ArmyCoinAmount / count;
        aveExp = a.ArmyEXP / count;
        aveHp = a.ArmHp / count;
    }

    public IEnumerator Spawn()
    {
        yield return new WaitUntil(() => GamePlayManager.Instance.play);
        if (allow&&index<enemies.Length)
        {
            yield return new WaitForSeconds(1);
            freePlaces = new List<Transform>();
            foreach (Transform t in Positions)
                if (t.childCount < 1)
                    freePlaces.Add(t);
            if (freePlaces.Count >= 1)
            {

                Transform c = freePlaces[Random.Range(0, freePlaces.Count)];
                GameObject e = Instantiate(enemies[index], c);
                if (e.GetComponent<Enemy>())
                e.GetComponent<Enemy>().reNewState(aveCoin,aveExp,aveHp);
                if (e.GetComponent<Enemy>())
                    e.transform.localPosition = e.GetComponent<Enemy>().SPoint;
                else
                    e.transform.localPosition = Vector2.zero;
                Add(e);
                index++;
            }
            StartCoroutine(Spawn());
        }
    }
    public IEnumerator spawnTapal()
    {
        if(tapalT>=2)
        tapalT -= 1;
        yield return new WaitForSeconds(tapalT);
        Transform t = Positions[Random.Range(0, Positions.Length)];
        GameObject e = Instantiate(tapal,t);
        e.transform.localPosition = e.GetComponent<Enemy>().SPoint;
        e.GetComponent<Enemy>().reNewState(0, 0, aveHp);
        StartCoroutine(spawnTapal());
    }
    public void respawnBoss()
    {
        allow = false;
        GameObject g = Instantiate(boss.transform.GetChild(0).gameObject, Vector2.zero, Quaternion.identity);
        g.transform.SetParent(Positions[1]);
        g.transform.localPosition = boss.transform.GetChild(0).GetComponent<Boss>().startPos;
        
        GamePlayManager.Instance.runningState(false);
        GamePlayManager.Instance.SetIcon(g.GetComponent<Boss>().icon);
    }
}
