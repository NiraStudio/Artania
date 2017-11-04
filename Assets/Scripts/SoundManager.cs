using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;
    public AudioClip Button, Panel, Coin, Gem;
    AudioSource Audio;
	// Use this for initialization
	void Start () {
        Instance = this;
        Audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
    public void ButtonSound()
    {
        Audio.PlayOneShot(Button);
    }
    public void PanelSound()
    {
        Audio.PlayOneShot(Panel);
    }
    public void CoinSound()
    {
        Audio.PlayOneShot(Coin);
    }
    public void GemSound()
    {
        Audio.PlayOneShot(Gem);
    }
}
