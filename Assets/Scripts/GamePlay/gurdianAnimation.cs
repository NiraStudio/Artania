using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gurdianAnimation : AlphaScript
{
    public float Multi;
    public float speed;
    Vector3 pos;
    float t;
    float NextT=1;
    Vector2 orginal;
	// Use this for initialization
	void Start () {
        pos = transform.localPosition;
        orginal = pos;
        NextT = Random.Range(1, 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition != pos)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, pos, speed * Time.deltaTime);
        }
        t += Time.deltaTime;
        if (t >= NextT)
        {
            t = 0;
            NextT = Random.Range(1, 2.5f);

           // do
           // {
                pos = orginal ;

                pos += (Vector3)(Random.insideUnitCircle * Multi);
                pos.z = 0;
          //  } while (pos.y < orginal.y || pos.y > orginal.y+0.2f);

        }
	}
}
