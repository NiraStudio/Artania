using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuardBoss : Boss {
    [Header("Enemy Gurad Boss")]

    public Wave[] Waves;

    Wave CurrentWave;
    [SerializeField]
    float  maxAttack, MinAttack;

    int Times;
	// Use this for initialization
	void Start () {
        Times = (int)Random.Range(MinAttack, maxAttack);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Spawn(Wave w)
    {
        for (int i = 0; i < w.enemies.Length; i++)
        {
         //   GameObject e = Instantiate(enemies[i], c);
         //   e.transform.localPosition = e.GetComponent<Enemy>().SPoint;
        }
    }
}
public class Wave
{
    public GameObject[] enemies;
    public bool Finished()
    {
        bool a=true;
        foreach (GameObject item in enemies)
        {
            if (item)
                a= false;
        }
        return a;
    }
}
