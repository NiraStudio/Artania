using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemenyPosesMvement : MonoBehaviour {
    Vector3 OrgPos,GoToPos;
    float timer;
    bool boss;
	// Use this for initialization
	void Start () {
        OrgPos = transform.localPosition;
        timer = Random.Range(2, 4);
        StartCoroutine(chooseDes());
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.GetChild(0).GetComponent<Boss>())
            boss = true;
        if (transform.localPosition != GoToPos&&!boss)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, GoToPos, 2 * Time.deltaTime);
	}
    IEnumerator chooseDes()
    {
        yield return new WaitForSeconds(timer);
       // yield return new WaitUntil(() => !transform.GetChild(0).GetComponent<Boss>());
        Vector3 a = Random.insideUnitCircle *.5f;

        GoToPos = OrgPos + a;
        GoToPos.z = 10;
        timer=Random.Range(2,4);
        StartCoroutine(chooseDes());

    }
}
