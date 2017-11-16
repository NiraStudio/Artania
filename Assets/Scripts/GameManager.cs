using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using Alpha.MissionSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TapsellSDK;
using Assets.SimpleAndroidNotifications;
public class GameManager : MonoBehaviour
{
    public bool restart;

    public static GameManager Instance;
    public PlayerDataBase PlayerDataBase;
    public GurdianDataBase gurdianDataBase;
    public Level upgradeLevel;
    public Font English, Persian;
    
    public static Font persian,english;
    public bool seeAd;
    public string PageName;
    public CurrencyData currencyData;
    public StateData stateData;
    public CharacterData characterData;
    MissionController missionController;

   Text aaaa;
   public int coinAmount
   {
       get {return currencyData.Coin; }
   }
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Tapsell.initialize("hitinfajfertkdptqjtkqrrfoqrqpignnaaqgitqcdpbcqgskbjdsladrsbddgecniiotk");
        persian = Persian;
        english = English;
        if (restart)
            PlayerPrefs.DeleteAll();
        else
        {
            missionController = GetComponent<MissionController>();
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.GetInt("FirstTime") == 0)
            {

                stateData = new StateData();
                currencyData = new CurrencyData();
                characterData = new CharacterData();
                characterData.charcaterIds = new List<int>();
                characterData.GurdianIds = new List<int>();
                characterData.levels = new List<Level>();
                characterData.charcaterIds.Add(1);
                characterData.GurdianIds.Add(1);
                characterData.charcaterId = 1;
                characterData.GurdianId = 1;
                characterData.levels.Add(new Level(1));
                SetCharacterLevel(1);
                stateData.hadBoughtFirstOffer = false;
                stateData.lvl = 0;
                stateData.EXP = 0;
                stateData.GoldGet = DateTime.Now;
                missionController.MakeMission();

                saveCharacter();
                saveCurrency();
                saveState();
                PlayerPrefs.SetInt("Meat", 0);
                PageName = "Entro";
                PlayerPrefs.SetString("Language", "Persian");
            }
            else
            {
                PlayerPrefs.SetInt("Meat", 1);
                LoadCharacter();
                LoadCurrency();
                LoadState();
                SetCharacterLevel(characterData.charcaterId);

                missionController.LoadMission();

                PageName = "MainMenu";
                
            }
        }
    }

    void FixedUpdate()
    {
        if (stateData.EXP >= expNeed())
        {
            stateData.EXP -= expNeed();
            stateData.lvl++;
            PlayerPrefs.SetInt("LevelUp", 1);
            saveState();
        }
    }



    public void AddEXP(int amount)
    {
        if(stateData.lvl<32)
        stateData.EXP += amount;
        saveState();
    }
    public int expNeed()
    {
        int a = (int)(1000 * (Mathf.Pow(1.27f, stateData.lvl)));
        return a;
    }
    // Update is called once per frame
    public void ChangeCoin(int amount)
    {
        currencyData.Coin += amount;
        saveCurrency();
    }
    
    public void ChangeGem(int amount)
    {
        currencyData.Gem += amount;
        saveCurrency();
    }
    
    public void SetCharacterLevel(int id)
    {
        foreach (var item in characterData.levels.ToArray())
        {
            if (item.id == id)
                upgradeLevel = item;
        }
    }

    void OnApplicationQuit()
    {

        notification();
        saveState();
        if (restart)
        {
            if (File.Exists(Application.persistentDataPath + "/Data/CR.alpha"))
            {
                File.Delete(Application.persistentDataPath + "/Data/CR.alpha");
            }
            if (File.Exists(Application.persistentDataPath + "/Data/CH.alpha"))
            {
                File.Delete(Application.persistentDataPath + "/Data/CH.alpha");
            }
            if (File.Exists(Application.persistentDataPath + "/Data/ST.alpha"))
            {
                File.Delete(Application.persistentDataPath + "/Data/ST.alpha");
            }
        }

    }
    void OnApplicationFocus(bool a)
    {
        if (a)
        {
            NotificationManager.CancelAll();
        }
    }
    void OnApplicationPause(bool a)
    {
        if (a)
        {
            

                notification();
                saveState();
            
        }
    }


    public void saveCurrency()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Data"));
        }
        FileStream File = new FileStream(Application.persistentDataPath + "/Data/CR.alpha", FileMode.Create);
        bf.Serialize(File, currencyData);

        File.Close();
    }
    public void LoadCurrency()
    {
        if (File.Exists(Application.persistentDataPath + "/Data/CR.alpha"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream File = new FileStream(Application.persistentDataPath + "/Data/CR.alpha", FileMode.Open);

            currencyData = bf.Deserialize(File) as CurrencyData;
            File.Close();
        }
        else
        {
            print("File not Exist");
            saveCurrency();
        }
    }

    public void saveCharacter()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Data"));
        }
        FileStream File = new FileStream(Application.persistentDataPath + "/Data/CH.alpha", FileMode.Create);
        for (int i = 0; i < characterData.levels.Count; i++)
        {
            if (characterData.levels[i].id == characterData.charcaterId)
                characterData.levels[i] = upgradeLevel;
        }
        bf.Serialize(File, characterData);


        File.Close();
    }
    public void LoadCharacter()
    {
        if (File.Exists(Application.persistentDataPath + "/Data/CH.alpha"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream File = new FileStream(Application.persistentDataPath + "/Data/CH.alpha", FileMode.Open);

            characterData = bf.Deserialize(File) as CharacterData;
            File.Close();
        }
        else
        {
            print("File not Exist");
            saveCharacter();
        }
    }

    public void saveState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Data"));
        }
        FileStream File = new FileStream(Application.persistentDataPath + "/Data/ST.alpha", FileMode.Create);
        bf.Serialize(File, stateData);
        File.Close();
    }
    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/Data/ST.alpha"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream File = new FileStream(Application.persistentDataPath + "/Data/ST.alpha", FileMode.Open);

            stateData = bf.Deserialize(File) as StateData;
            File.Close();

        }
        else
        {
            print("File not Exist");
            saveState();
        }
    }

    public static string NumberPersian(string number,Text t)
    {
        string a = "";
        if (PlayerPrefs.GetString("Language") == "Persian")
        {
            char[] b = number.ToString().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                switch (b[i])
                {
                    case '1':
                        a += "۱";
                        break;
                    case '2':
                        a += "۲";
                        break;
                    case '3':
                        a += "۳";
                        break;
                    case '4':
                        a += "۴";
                        break;
                    case '5':
                        a += "۵";
                        break;
                    case '6':
                        a += "۶";
                        break;
                    case '7':
                        a += "۷";
                        break;
                    case '8':
                        a += "۸";
                        break;
                    case '9':
                        a += "۹";
                        break;
                    case '0':
                        a += "۰";
                        break;
                    
                    default:
                        a += b[i];
                        break;
                }

            }
            try
            {

                t.font = persian;
            }
            catch (Exception)
            {
                print(t.text + " the problrm text");
                
            }
            
        }
        else
        {
            char[] b = number.ToString().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                switch (b[i])
                {
                    case '۱':
                        a += "1";
                        break;
                    case '۲':
                        a += "2";
                        break;
                    case '۳':
                        a += "3";
                        break;
                    case '۴':
                        a += "4";
                        break;
                    case '۵':
                        a += "5";
                        break;
                    case '۶':
                        a += "6";
                        break;
                    case '۷':
                        a += "7";
                        break;
                    case '۸':
                        a += "8";
                        break;
                    case '۹':
                        a += "9";
                        break;
                    case '۰':
                        a += "0";
                        break;
                    
                    default:
                        a += b[i];
                        break;
                }
            }
            t.font = english;
        }
        return a;
    }

    public static string NumberPersian(string number)
    {
        string a = "";
        if (PlayerPrefs.GetString("Language") == "Persian")
        {
            char[] b = number.ToString().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                switch (b[i])
                {
                    case '1':
                        a += "۱";
                        break;
                    case '2':
                        a += "۲";
                        break;
                    case '3':
                        a += "۳";
                        break;
                    case '4':
                        a += "۴";
                        break;
                    case '5':
                        a += "۵";
                        break;
                    case '6':
                        a += "۶";
                        break;
                    case '7':
                        a += "۷";
                        break;
                    case '8':
                        a += "۸";
                        break;
                    case '9':
                        a += "۹";
                        break;
                    case '0':
                        a += "۰";
                        break;
                   
                    default:
                        a += b[i];
                        break;
                }
            }
        }
        else
        {
            char[] b = number.ToString().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                switch (b[i])
                {
                    case '۱':
                        a += "1";
                        break;
                    case '۲':
                        a += "2";
                        break;
                    case '۳':
                        a += "3";
                        break;
                    case '۴':
                        a += "4";
                        break;
                    case '۵':
                        a += "5";
                        break;
                    case '۶':
                        a += "6";
                        break;
                    case '۷':
                        a += "7";
                        break;
                    case '۸':
                        a += "8";
                        break;
                    case '۹':
                        a += "9";
                        break;
                    case '۰':
                        a += "0";
                        break;
                    
                    default:
                        a += b[i];
                        break;
                }
            }
        }
        return a;
    }





    public static string Language(string Persian,string English,Text t)
    {
        string a = "";
        a = PlayerPrefs.GetString("Language") == "Persian" ? Persian : English;
        if(t!=null)
        t.font = PlayerPrefs.GetString("Language") == "Persian" ? persian : english;
        return a;
    }
    public static string Language(string Persian, string English)
    {
        string a = "";
        a = PlayerPrefs.GetString("Language") == "Persian" ? Persian : English;
        return a;
    }
    void notification()
    {
    
       
        NotificationManager.SendCustom(new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(86400*7),
            Title = Language("کجایی؟؟","Where are you??"),
            Message = Language("دلمون برات تنگ شده نمیخوایی به ما سر بزنی؟؟","We miss you ,Dont you want to see us again?"),
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        });
        if (CurrentAmount() < IdlePanel.CalculateStorage(stateData.coinerStorageLevel))
        {
            int timeToGo =(int)( (IdlePanel.CalculateStorage(stateData.coinerStorageLevel) - CurrentAmount()) / IdlePanel.calculatePerSec(stateData.CoinPerSecLevel));
            NotificationManager.SendCustom(new NotificationParams
            {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = TimeSpan.FromSeconds(timeToGo),
                Title = Language("مخزن پر شده", "Storage is full "),
                Message = Language("مخزن سکه هات پر شده زود بیا خالی کن تا دوباره پرش کنیم", "You storage is full quick make it empty so we can make coin again"),
                Ticker = "Ticker",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Clock,
                SmallIconColor = new Color(0, 0.5f, 0),
                LargeIcon = "app_icon"
            });
        }
        /*if (currencyData.Meat > 1)
        {
            NotificationManager.SendCustom(new NotificationParams
            {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = TimeSpan.FromSeconds(currencyData.Meat*3600),
                Title = Language("گوشتمون تموم شده", "We are out of meat"),
                Message = Language("گوشتمون تموم شده دیگه نمیتونیم کار کنیم", "We are out of meat we cant work anymore"),
                Ticker = "Ticker",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Clock,
                SmallIconColor = new Color(0, 0.5f, 0),
                LargeIcon = "app_icon"
            });
        }*/
        DateTime a = DateTime.Now;
        DateTime b = new DateTime(a.Year, a.Month, a.Day, 0, 0, 0);
        b = b.AddDays(1);
        double aa = (b - DateTime.Now).TotalSeconds;
        NotificationManager.SendCustom(new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(aa),
            Title = Language("جایزه و ماموریت جدید", "Prize & new Missions"),
            Message = Language("زود باش بیا تو آرتانیا ", "Quick !! Come to Artania"),
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Event,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        });

    
    }



    public int CurrentAmount()
    {
        double a = (DateTime.Now - stateData.GoldGet).TotalSeconds;
        int amount = (int)(IdlePanel.calculatePerSec(stateData.CoinPerSecLevel) * a);
        if (amount > IdlePanel.CalculateStorage(stateData.coinerStorageLevel))
            amount = IdlePanel.CalculateStorage(stateData.coinerStorageLevel);
        return amount;


    }



















    [Serializable]
    public class CurrencyData
    {
        public ObscuredInt Coin;
        public ObscuredInt Gem;
        public ObscuredBool IsDoubleCoin;
        public ObscuredBool IsDoubleExp;
    }

    [Serializable]
    public class StateData
    {
        public ObscuredInt lvl;
        public ObscuredInt EXP;
        public ObscuredInt LastKilledBoss;
        public ObscuredInt CoinPerSecLevel;
        public ObscuredInt coinerStorageLevel;
        public DateTime RewardTime;
        public DateTime GoldGet;
        public DateTime MissionTime;
        public Mission[] misssions;
        public bool hadBoughtFirstOffer;
        public int HighScore;
    }

    [Serializable]
    public class CharacterData
    {
        public int charcaterId;
        public int GurdianId;
        public List<int> charcaterIds;
        public List<int> GurdianIds;
        public List<Level> levels;
    }

    
}
