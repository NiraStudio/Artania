using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( AudioSource))]
public class PowerUp : AlphaScript
{
    [Header("Sounds")]
    public AudioClip sound;

    [Header("Conditions")]
    public bool Gurdian;
    public bool DoubleEXP;
    public bool DoubleCoin;
    public bool ManaPoint;

    AudioSource aud;
    GameObject player;
    // Use this for initialization
    void Start()
    {
        aud = GetComponent<AudioSource>();
        player = PlayerHealth.Instance.gameObject;
        Destroy(gameObject, 3f);
    }
    void Update()
    {
        if (!GamePlayManager.Instance.Run || !GamePlayManager.Instance.play||transform.position.x - Camera.main.transform.position.x < -12)
            Destroy(gameObject);
        if (ManaPoint)
            if (transform.position != player.transform.position)
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 4 * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            //aud.PlayOneShot(sound);
            if (Gurdian)
            {
                PlayerHealth.Instance.TurnOnTheGurdian();
                MissionController.Instance.AddToMission(Alpha.MissionSystem.Mission.Type.collectGurdian, 1);
            }
            if (ManaPoint)
            {
                int a = Random.Range(2, 4);
                target.GetComponent<PlayerAttack>().AddEnergy(a);
            }
            if (DoubleCoin)
            {
                powerUpManager.Instance.DoubleCoin();
            }
            if (DoubleEXP)
            {
                powerUpManager.Instance.DoubleExp();
            }
            Destroy(gameObject);
        }
    }
}
