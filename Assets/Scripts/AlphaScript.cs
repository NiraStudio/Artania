using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaScript : MonoBehaviour {

    public void playSound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName);
    }
    public void playSoundVolume(string soundName,float volume)
    {
        AudioManager.Instance.PlaySound(soundName,volume);
    }
    public void StopPlayAll()
    {
        AudioManager.Instance.StopPlayAll();
    }
    public void stopOne(string name)
    {
        AudioManager.Instance.stopOne(name);
    }
    public void Shake()
    {
        Handheld.Vibrate();
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("EarthShake");
    }
    public void OpenGemPanel()
    {
        GemConvertorPanel.Instance.Enter();
    }
}
