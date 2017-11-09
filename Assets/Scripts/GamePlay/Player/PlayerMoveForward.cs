using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveForward : MonoBehaviour
{
    public static PlayerMoveForward Instance;
    [Header("Speed in 15 Sec")]
    public float speed;
    public float maxSpeed;
    float a;
   // [HideInInspector]
    public float s;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        a = speed / 15;
        s = speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (GamePlayManager.Instance.play && GamePlayManager.Instance.Run)
        {
            if (s < maxSpeed)
                s += a * Time.deltaTime;

            Vector3 t = transform.position;
            t.x += s * Time.deltaTime;
            transform.position = t;
        }
    }
    public void RestartSpeed()
    {
        s = speed;
    }
    
   
}
