using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AlphaScript
{

    public enum Type
    {
        Straight,
        Aimer,
        Ultimate,
    }
    public Type type;

    public enum attackerType
    {
        player,Enemy,Boss,Strong,expDrainer
    }
    public attackerType AttackerType;
    public TrailRenderer trail;
    [Range(-1, 1)]
    public int dir;
    public float speed;
    public float dmg;
    public string TargetTag;

    bool ultimatefinished;
    Transform target;
    Vector2 Dir;
	// Use this for initialization
	void Start () {
        Invoke("Die", 4);

        if (trail != null)
            trail.sortingLayerName = "Bullet";
	}

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case Type.Straight:
                Vector2 t = transform.position;
                t.x += speed * Time.deltaTime * dir;
                transform.position = t;
                break;


            case Type.Aimer:
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (dir * Dir), speed * Time.deltaTime);
               
                break;

            case Type.Ultimate:
                Vector2 pos = target.transform.position;
                if (Vector2.Distance(transform.position, pos) > 1 && !ultimatefinished)
                {
                    Dir = (transform.position - target.position);
                    Dir = Dir.normalized;
                    var angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
                    if (dir == -1)
                        angle += 180;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                    ultimatefinished = true;
                
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (dir * Dir), speed * Time.deltaTime);
                break;

        }

    }
    void OnTriggerEnter2D(Collider2D target)
    {
        switch (AttackerType)
        {
            case attackerType.player:
                float d = dmg * (powerUpManager.Instance.superPower ? 5 : 1);
                if(target.tag=="Enemy")
                {
                    target.GetComponent<Enemy>().getDamage(d);
                    Die();
                }
                if (target.tag == "Boss")
                {
                    target.GetComponent<Boss>().ReciveDamage(d);
                    Die();
                }
                if (target.tag == "EnemyBullet" )
                {
                    target.GetComponent<Bullet>().Die();
                    Die();
                }
                if (target.tag == "Frog")
                {
                    target.GetComponent<FrogBoss>().Hit();
                    Die();
                }
                break;
            case attackerType.Enemy:
                if (target.tag == "Player")
                {
                    if (GamePlayManager.Instance.play)
                        target.GetComponent<PlayerHealth>().die(this, false);


                }
                break;
            case attackerType.Boss:
                if (target.tag == "Player")
                {
                    if (GamePlayManager.Instance.play)
                        target.GetComponent<PlayerHealth>().die(this, true);
                    Die();
                }
                break;
            case attackerType.expDrainer:
                if (target.tag == "Player")
                {
                    if (GamePlayManager.Instance.play&&!powerUpManager.Instance.superPower)
                    {
                        if (GamePlayManager.Instance.Exp >= dmg)
                            GamePlayManager.Instance.Exp -= (int)dmg;
                        else
                        target.GetComponent<PlayerHealth>().die(this, true);

                    }
                    Die();
                }
                break;
            case attackerType.Strong:
                float b = dmg * (powerUpManager.Instance.superPower ? 5 : 1);
                if(target.tag=="Enemy")
                {
                    target.GetComponent<Enemy>().getDamage(b);
                    Die();
                }
                if (target.tag == "Boss")
                {
                    target.GetComponent<Boss>().ReciveDamage(b);
                    Die();
                }
                if (target.tag == "EnemyBullet")
                {
                    target.GetComponent<Bullet>().Die();
                }
                if (target.tag == "Frog")
                {
                    target.GetComponent<FrogBoss>().Hit();
                    Die();
                }
                break;
            default:
                break;
        }
        
    }

    public void ChangeTarget(Transform Target)
    {
        target = Target;
        Dir = (transform.position - target.position);
        Dir = Dir.normalized;
        var angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Die()
    {
        if (trail != null)
            trail.gameObject.SetActive(false);
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetTrigger("Die");
            if (!GamePlayManager.Instance.Run)
                dir = 0;
            else
                speed /= 3.5f;
        }
        else
        {
            Destroy(gameObject);
        }
        this.enabled = false;
    }
    public void Des()
    {
        Destroy(gameObject);
    }
}
