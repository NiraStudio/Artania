using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class entroScript : MonoBehaviour {
    public RawImage MovieImg;
    #if UNITY_ANDROID
#else
    
    public MovieTexture movie;
#endif
	// Use this for initialization
    IEnumerator Start()
    {
        
        loadingScreen.Instance.Disapear();
        yield return new WaitForSeconds(1);
#if UNITY_ANDROID
        Handheld.PlayFullScreenMovie("Intro.mp4",Color.white,FullScreenMovieControlMode.Hidden);
        loadingScreen.Instance.Show("Play");
#else
        MovieImg.texture = movie;
    loadingScreen.Instance.Disapear();
        movie.Play();
#endif

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
