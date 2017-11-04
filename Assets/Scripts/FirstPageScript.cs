using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Backtory.Core.Public;
using UPersian.Components;
using System.IO;
public class FirstPageScript : MonoBehaviour {
    public Animator LogInPanel;
    public GameObject loadingPanel;
    public InputField usernameLogIn, passwordLogIn;
    public RtlText LogInLogText;
    public Button LogInButton, SkipBtn;

    public enum SaveKind
    {
        state, currency, character
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        LogInButton.interactable = InternetChecker.Instance.internetConnectBool;
	}
    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        GameManager.Instance.ChangeCoin(1000);
        GameManager.Instance.AddEXP(1000);
        loadingScreen.Instance.Show("MainMenu");
    }
    public void LogIn()
    {
        loadingPanel.SetActive(true);
        BacktoryUser.LoginInBackground(usernameLogIn.text, passwordLogIn.text, loginResponse =>
        {

            // Login operation done (fail or success), handling it:
            if (loginResponse.Successful)
            {
                PlayerPrefs.SetString("Username", usernameLogIn.text);
                PlayerPrefs.SetString("PassWord", passwordLogIn.text);
                // Login successful
                LogInPanel.SetTrigger("Idle");
                DownloadData();
                loadingScreen.Instance.Show("MainMenu");                
            }
            else if (loginResponse.Code == (int)BacktoryHttpStatusCode.Unauthorized)
            {

                // Username 'mohx' with password '123456' is wrong
                Debug.Log("Either username or password is wrong.");
                LogInLogText.text = GameManager.Language("نام کاربری یا رزم عبور اشتباه است", "Either username or password is wrong.", LogInLogText);
                loadingPanel.SetActive(false);

            }
            else
            {
                // Operation generally failed, maybe internet connection issue
                Debug.Log("Login failed for other reasons like network issues.");
                LogInLogText.text = GameManager.Language("خطلا در ورود", "Login failed", LogInLogText);
                loadingPanel.SetActive(false);
            }
        });
    }
    public void DownloadData()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(downloadData("https://storage.backtory.com/playersdata/playersdata/" + PlayerPrefs.GetString("Username") + "/CR.alpha", Application.persistentDataPath + "/Data/CR.alpha", SaveKind.currency));
        StartCoroutine(downloadData("https://storage.backtory.com/playersdata/playersdata/" + PlayerPrefs.GetString("Username") + "/CH.alpha", Application.persistentDataPath + "/Data/CH.alpha", SaveKind.character));
        StartCoroutine(downloadData("https://storage.backtory.com/playersdata/playersdata/" + PlayerPrefs.GetString("Username") + "/ST.alpha", Application.persistentDataPath + "/Data/ST.alpha", SaveKind.state));
        print("https://storage.backtory.com/playersdata/playersdata/" + PlayerPrefs.GetString("Username") + "/ST.alpha");
        loadingPanel.SetActive(false);
    }
    IEnumerator downloadData(string Download, string path, SaveKind kind)
    {
        WWW w = new WWW(Download);
        yield return w;
        if (w.error != null)
        {
            Debug.Log("Error .. " + w.error);
            // for example, often 'Error .. 404 Not Found'
        }
        else
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Data"));
            }
            File.WriteAllBytes(path, w.bytes);
            print("save " + path);
            switch (kind)
            {
                case SaveKind.state:
                    GameManager.Instance.LoadState();

                    break;
                case SaveKind.currency:
                    GameManager.Instance.LoadCurrency();

                    break;
                case SaveKind.character:
                    GameManager.Instance.LoadCharacter();
                    break;
                default:
                    break;
            }
        }
    }

}
