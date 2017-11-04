using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    public bool play;
    public sound[] sounds;
	// Use this for initialization
	void Start () {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            item.source.volume = item.volume;
            item.source.loop = item.loop;
        }
	}
    public void PlaySound(string Name)
    {
        if (play)
        {
            sound s = System.Array.Find(sounds, sound => sound.ClipName == Name);
            if (s == null)
                return;
            s.source.volume = s.volume;
            s.source.Play();
        }
    }
    public void PlaySound(string Name, float volume)
    {
        if (play)
        {
            sound s = System.Array.Find(sounds, sound => sound.ClipName == Name);
            if (s == null)
                return;
            float a = s.source.volume;
            s.source.volume = volume;
            s.source.Play();
            s.source.volume = a;
        }
    }
    public void StopPlayAll()
    {
        foreach (var item in sounds)
        {
            item.source.Stop();
        }
    }
    public void stopOne(string name)
    {
        sound s = System.Array.Find(sounds, sound => sound.ClipName == name);
        if (s == null)
            return;
        s.source.Stop();
    }
    public void ChangePitch(string name, float pitch)
    {
        sound s = System.Array.Find(sounds, sound => sound.ClipName == name);
        if (s == null)
            return;
        s.source.pitch = pitch;
    }
}

[System.Serializable]
public class sound
{
    public string ClipName;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
    [Range(-3,3)]
    public float Pitch = 1;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
