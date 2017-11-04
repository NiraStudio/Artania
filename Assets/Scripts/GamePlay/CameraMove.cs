using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Range(-1,1)]
    public int Dir;
    public PlayerMoveForward Player;

    float speed;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        speed = Player.s;
        if (GamePlayManager.Instance.play&&GamePlayManager.Instance.Run)
        {

            Vector3 t = transform.position;
            t.x += speed * Time.deltaTime * Dir;
            transform.position = t;
        }
    }
     
}