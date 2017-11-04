using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Backtory.Core.Public;
using UPersian.Components;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderBoard : AlphaScript {
    public Animator signUpPanel, LogInPanel, LeadrBoardPanel;
    public GameObject LoadingPanel;
    
    [Header("SignUp Texts")]
    public RtlText SignUpLogText,UserNameText,PassWordText,EmailText,SignUpName;
    [Header("SignUp Texts")]
    public InputField username, email, password;
    [Header("LogIn Parameters")]
    public InputField usernameLogIn, passwordLogIn;
    [Header("LogIn Parameters")]
    public RtlText LogInLogText, UserNameLogInText, PassWordLogInText,LogInName;

    public Button LeaderBoardBtn;
    [Header("Leaderboard")]
    public RtlText Level, UserName, Score;public GameObject UserPrefab;public Profile UserProfile;public GameObject ProfileParent;

    [Header("medal images")]

    public Sprite[] medals;
    public Sprite firstRank, SecondRank, ThirdRank,normalRank;


    public enum SvaeKind{
        state,currency,character
    }
	void Start () {

        LeaderBoardBtn.onClick.AddListener(clicked);
	}
	void Update () {

        LeaderBoardBtn.interactable = InternetChecker.Instance.internetConnectBool;
	}
    void clicked(){
        playSound("Button");

        if (PlayerPrefs.GetString("Username") != "")
        {
            LoadingPanel.SetActive(true);
            BacktoryUser.LoginInBackground(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("PassWord"), loginResponse =>
            {

                // Login operation done (fail or success), handling it:
                if (loginResponse.Successful)
                {
                    // Login successful
                    ShowLeaderBoard();
                }
                else if (loginResponse.Code == (int)BacktoryHttpStatusCode.Unauthorized)
                {
                    // Username 'mohx' with password '123456' is wrong
                    Debug.Log("Either username or password is wrong.");
                    LoadingPanel.SetActive(false);

                }
                else
                {
                    // Operation generally failed, maybe internet connection issue
                    Debug.Log("Login failed for other reasons like network issues.");
                    LoadingPanel.SetActive(false);

                }
            });
        }
        else
        {
            signUpPanel.SetTrigger("Enter");
            SignUpName.text = GameManager.Language("ثبت نام", "Sign Up", SignUpName);

            if (PlayerPrefs.GetString("Language") == "Persian")
            {
                UserNameText.text = "نام کاربری";
                PassWordText.text = "رمز ورود";
                EmailText.text = "ایمیل";
            }
            else
            {
                UserNameText.text = "Username";
                PassWordText.text = "Password";
                EmailText.text = "E-mail";
            }
        }
    }
    public void ShowLeaderBoard()
    {

        for (int i = 0; i < ProfileParent.transform.childCount; i++)
        {
            Destroy(ProfileParent.transform.GetChild(i).gameObject);
        }
        MainLeaderBoard m = new MainLeaderBoard();
        m.GetTopPlayersInBackground(100, leaderboardResponse =>
        {

            // Checking if response was fetched successfully
            if (leaderboardResponse.Successful)
            {
                for (int i = 0; i < leaderboardResponse.Body.UsersProfile.Count; i++)
                {
                    BacktoryLeaderBoard.UserProfile topPlayer =
                        leaderboardResponse.Body.UsersProfile[i];
                    string username = topPlayer.UserBriefProfile.UserName;
                    IList<int> scores = topPlayer.Scores;
                    Sprite a = new Sprite();
                    switch (i+1)
                    {
                        case 1:
                            a = firstRank;
                            break;
                        case 2:
                            a = SecondRank;
                            break;
                        case 3:
                            a = ThirdRank;
                            break;
                        default:
                            a = normalRank;
                            break;
                    }
                    Instantiate(UserPrefab,ProfileParent.transform).GetComponent<Profile>().RePaint(username, scores[0], scores[1], i+1, medals[scores[1] / 8], a);

                    // Logging best player
                    Debug.Log("best player is: " + username
                            + " Exp: " + scores[0] + " Lvl: " + scores[1]);
                }
            }
            else
            {
                print("SomeThingWentWrong");
                LoadingPanel.SetActive(false);

            }
        });
        m.GetPlayerRankInBackground(rankResponse =>
        {

            // Check if backtory returned result successfully
            if (rankResponse.Successful)
            {
                IList<int> scores = rankResponse.Body.Scores;
                Sprite a=new Sprite();
                switch (rankResponse.Body.Rank)
                {
                    case 1:
                        a = firstRank;
                        break;
                    case 2:
                        a = SecondRank;
                        break;
                    case 3:
                        a = ThirdRank;
                        break;
                    default:
                        a = normalRank;
                        break;
                }
                UserProfile.RePaint(PlayerPrefs.GetString("Username"), scores[0], scores[1], rankResponse.Body.Rank, medals[scores[1] / 8],a );
                LoadingPanel.SetActive(false);
                LeadrBoardPanel.SetTrigger("Enter");
            }
            else
            {
                // do something based on error code
                LoadingPanel.SetActive(false);

            }
        });
    }
    public void SignUp()
    {
        playSound("Button");

        if (InputValidCheck(username.text, 4, false, false))
        {
            if (InputValidCheck(email.text, 8, true, false))
            {
                if (InputValidCheck(password.text, 8, false, false))
                {

                    //openpanel
                    LoadingPanel.gameObject.SetActive(true);

                    BacktoryUser newUser = new BacktoryUser
                    {
                        Username = username.text,
                        Email = email.text,
                        Password = password.text,
                    };
                    // Registring user to backtory (in background)
                    newUser.RegisterInBackground(response =>
                    {
                        // Checking result of operation
                        if (response.Successful)
                        {
                            LoadingPanel.gameObject.SetActive(true);
                            Debug.Log("Register Success; new username is " + response.Body.Username);
                            PlayerPrefs.SetString("Username", username.text);
                            PlayerPrefs.SetString("PassWord", password.text);
                            BacktoryUser.LoginInBackground(username.text, password.text, loginResponse =>
                            {

                                // Login operation done (fail or success), handling it:
                                if (loginResponse.Successful)
                                {
                                    // Login successful
                                    Debug.Log("Welcome " + username);
                                    signUpPanel.SetTrigger("Idle");
                                    MainLeaderBoardEvent Event = new MainLeaderBoardEvent(GameManager.Instance.stateData.HighScore, GameManager.Instance.stateData.lvl);
                                    Event.SendInBackground(null);
                                    PlayerPrefs.SetInt("SetScore", 0);
                                    UploadData();
                                    ShowLeaderBoard();
                                }
                                else if (loginResponse.Code == (int)BacktoryHttpStatusCode.Unauthorized)
                                {
                                    // Username 'mohx' with password '123456' is wrong
                                    Debug.Log("Either username or password is wrong.");
                                }
                                else
                                {
                                    // Operation generally failed, maybe internet connection issue
                                    Debug.Log("Login failed for other reasons like network issues.");
                                }
                            });
                        }
                        else if (response.Code == (int)BacktoryHttpStatusCode.Conflict)
                        {
                            // Username is invalid
                            Debug.Log("Bad username; a user with this username already exists.");

                            SignUpLogText.text = PlayerPrefs.GetString("Language") == "Persian" ? "این نام کاربری وجود دارد" : "username already exists.";
                            LoadingPanel.gameObject.SetActive(false);

                        }
                        else
                        {
                            // General failure
                            Debug.Log("Registration failed; for network or some other reasons.");
                            SignUpLogText.text = PlayerPrefs.GetString("Language") == "Persian" ? "ثبت نام با خطلا مواجه شد" : "Registration failed";
                            LoadingPanel.gameObject.SetActive(false);

                        }
                    });
                    //closepanel


                }
                else
                {
                    SignUpLogText.text = PlayerPrefs.GetString("Language") == "Persian" ? "رمزعبور اشتباه است، حداقل 8 حرف" : "Password Is invalid";
                    //password
                }
            }
            else
            {
                SignUpLogText.text = PlayerPrefs.GetString("Language") == "Persian" ? "ایمیل اشتباه است" : "Email Is invalid";

                //email
            }
        }
        else
        {
            SignUpLogText.text = PlayerPrefs.GetString("Language") == "Persian" ? "نام کاربری اشتباه است، حداقل 8 حرف" : "UserName Is invalid";
            //username
            SignUpLogText.text = "UserName Is invalid";
        }
    }
    public void LogIn()
    {
        playSound("Button");

        LoadingPanel.SetActive(true);
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
                ShowLeaderBoard();
            }
            else if (loginResponse.Code == (int)BacktoryHttpStatusCode.Unauthorized)
            {

                // Username 'mohx' with password '123456' is wrong
                Debug.Log("Either username or password is wrong.");
                LogInLogText.text = GameManager.Language("نام کاربری یا رزم عبور اشتباه است", "Either username or password is wrong.", LogInLogText);
                LoadingPanel.SetActive(false);

            }
            else
            {
                // Operation generally failed, maybe internet connection issue
                Debug.Log("Login failed for other reasons like network issues.");
                LogInLogText.text = GameManager.Language("خطلا در ورود", "Login failed", LogInLogText);
                LoadingPanel.SetActive(false);
            }
        });
    }
    public void OpenLogInPanel()
    {
        signUpPanel.SetTrigger("Idle");
        LogInName.text = GameManager.Language("ورود", "Log In", LogInName);
        if (PlayerPrefs.GetString("Language") == "Persian")
        {
            UserNameLogInText.text = "نام کاربری";
            PassWordLogInText.text = "رمز ورود";
        }
        else
        {
            UserNameLogInText.text = "Username";
            PassWordLogInText.text = "Password";
        }
        LogInPanel.SetTrigger("Enter");

    }
    public static bool InputValidCheck(string Input, int length, bool email, bool phoneNumber)
    {
        Input = Input.ToLower();
        char[] a = Input.ToCharArray();

        bool valid = false;
        if (a.Length < length)
            return false;
        if (email)
        {
            if (a.Length >= 14 && Input.Contains("@") && Input.Contains(".com"))
                valid = true;
        }
        else if (phoneNumber)
        {
            if (a[0] == '0' && a.Length == 11)
                valid = true;
        }
        else
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null && a[i] != ' ')
                    valid = true;
            }
        }

        return valid;
    }
    public void exitLeaderBoard()
    {
        playSound("Button");

        LeadrBoardPanel.SetTrigger("Idle");
    }
    public void exitSignUp()
    {
        playSound("Button");

        print("Exit");
        signUpPanel.SetTrigger("Idle");
    }
    public void exitLogIn()
    {
        playSound("Button");

        LogInPanel.SetTrigger("Idle");
    }
    public void UploadData()
    {
        LoadingPanel.SetActive(true);
        ///upload St.alpha
        if (File.Exists(Application.persistentDataPath + "/Data/ST.alpha"))
        {
            BacktoryFile backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/ST.alpha");
            backtoryFile.UploadInBackground("/" + PlayerPrefs.GetString("Username") + "/", true, (response) =>
            {
                if (response.Successful)
                {
                    

                    ///Upload CH.alpha

                    if (File.Exists(Application.persistentDataPath + "/Data/CH.alpha"))
                    {
                        backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/CH.alpha");
                        backtoryFile.UploadInBackground("/" + PlayerPrefs.GetString("Username") + "/", true, (res) =>
                        {
                            if (res.Successful)
                            {

                                ///Upload CR.alpha
                                ///

                                if (File.Exists(Application.persistentDataPath + "/Data/CR.alpha"))
                                {
                                    backtoryFile = new BacktoryFile(Application.persistentDataPath + "/Data/CR.alpha");
                                    backtoryFile.UploadInBackground("/" + PlayerPrefs.GetString("Username") + "/", true, (r) =>
                                    {
                                        if (r.Successful)
                                        {
                                            LoadingPanel.SetActive(false);

                                            string filePathOnServer = response.Body;
                                            Debug.Log("Upload was successful. File path on server (url) is " + filePathOnServer);
                                        }
                                        else
                                        {
                                            Debug.Log("failed; " + response.Message + " " + response.Code);
                                            LoadingPanel.SetActive(false);

                                        }
                                    });
                                }


                            }
                            else
                            {
                                Debug.Log("failed; " + response.Message + " " + response.Code);
                                LoadingPanel.SetActive(false);

                            }
                        });
                    }






                }
                else
                {
                    Debug.Log("failed; " + response.Message + " " + response.Code);
                    LoadingPanel.SetActive(false);
                }
            });
        }
    }
    public void DownloadData()
    {
        LoadingPanel.SetActive(true);
        StartCoroutine(downloadData("https://storage.backtory.com/playerdata/" + PlayerPrefs.GetString("Username") + "/CR.alpha", Application.persistentDataPath + "/Data/CR.alpha", SvaeKind.currency));
        StartCoroutine(downloadData("https://storage.backtory.com/playerdata/" + PlayerPrefs.GetString("Username") + "/CH.alpha", Application.persistentDataPath + "/Data/CH.alpha", SvaeKind.character));
        StartCoroutine(downloadData("https://storage.backtory.com/playerdata/" + PlayerPrefs.GetString("Username") + "/ST.alpha", Application.persistentDataPath + "/Data/ST.alpha", SvaeKind.state));
        LoadingPanel.SetActive(false);
    }
    IEnumerator downloadData(string Download, string path,SvaeKind kind)
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
                case SvaeKind.state:
                    GameManager.Instance.LoadState();

                    break;
                case SvaeKind.currency:
                    GameManager.Instance.LoadCurrency();

                    break;
                case SvaeKind.character:
                    GameManager.Instance.LoadCharacter();
                    break;
                default:
                    break;
            }
        }
    }
}
public class MainLeaderBoardEvent : BacktoryGameEvent
{

    [EventName]
    public const string EventName = "MainLeaderBoard";

    [FieldName("EXP")]
    public int ExpValue { get; set; }

    [FieldName("Level")]
    public int LevelValue { get; set; }

    public MainLeaderBoardEvent(int EXP, int Level)
    {
        ExpValue = EXP;
        LevelValue = Level;
    }
}
public class MainLeaderBoard : BacktoryLeaderBoard
{

    [LeaderboardId]
    public const string Id = "59c6df2be4b05b0592cc26c9";
}
