using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
    public GameObject[] chunks;
    public float chunkSize;
    public int startChunkNumber;
    List<GameObject> ingameChunks=new List<GameObject>();
    float XPos = -5;

    int order = 0;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < startChunkNumber; i++)
        {
            MakeChunk();
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject g in ingameChunks.ToArray())
        {
            if(Camera.main.transform.position.x-30>g.transform.position.x)
            {
                ingameChunks.Remove(g);
                Destroy(g);
                MakeChunk();
            }
        }
	}
    void MakeChunk()
    {
        GameObject g = Instantiate(chunks[Random.Range(0, chunks.Length)], new Vector2(XPos, 0), Quaternion.identity);
        ingameChunks.Add( g);
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = order;
        if (order == 0)
            order = 1;
        else
            order = 0;
        XPos += chunkSize;
    }
}
