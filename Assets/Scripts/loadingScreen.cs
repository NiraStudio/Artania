using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class loadingScreen : AlphaScript
{
    public static loadingScreen Instance;
    public AudioManager audio;
    public string[] Hints;
    public AudioClip In, Out;
    public Text HintText;
    Animator anim;
    static bool a;

    AudioSource aud;
    string sceneName;
    // Use this for initialization
    void Awake()
    {
        aud = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        anim = GetComponent<Animator>();
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(ShowText());
    }
    // Update is called once per frame
    public void Show(string SceneName)
    {
        audio.play = false;
        anim.SetBool("Show", true);
        sceneName = SceneName;
        aud.PlayOneShot(In);
    }
    IEnumerator ShowText()
    {
        yield return new WaitUntil(() => HintText.gameObject.activeInHierarchy);
        HintText.text=Hints[Random.Range(0,Hints.Length)];
        yield return new WaitForSeconds(4f);
        StartCoroutine(ShowText());

    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Disapear()
    {
        audio.play = true;
        anim.SetBool("Show", false);

        aud.PlayOneShot(Out);
    }
   

}
