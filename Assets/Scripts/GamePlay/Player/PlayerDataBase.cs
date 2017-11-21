using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerDataBase : ScriptableObject {

    [SerializeField]
    List<Player> DB = new List<Player>();


    public Player GetPlayerById(int ID)
    {
        Player t = new Player();
        for (int i = 0; i < DB.Count; i++)
        {
            if(DB[i].Id==ID)
            {
                t = DB[i];
                break;
            }
        }
        return t;
    }
    public int lastId()
    {
        int t = 0;
        for (int i = 0; i < DB.Count; i++)
        {
            if (DB[i].Id > t)
                t = DB[i].Id;
        }
        return t;
    }
    public Player GetByIndex(int index)
    {
        return DB[index];
    }
    public int DBLength
    {
        get { return DB.Count; }
    }

}

[System.Serializable]
public class Player
{
    public string Name,PersianName;
    public int Id;
    public Color color;
    public GameObject Shape;
    public Sprite Image;
    public Price Price;
    public int LvlNeed;
    public int Attack, FillRate, MaxMana;
}
[System.Serializable]
public class Level
{
    public int id;
    public ObscuredInt DmgLevel;
    public ObscuredInt FillRateLVL;
    public ObscuredInt MaxEnergyLVL;
    public Level(int ID)
    {
        id = ID;
    }
}
[System.Serializable]
public class Price
{
    public enum Type
    {
        Gem,
        Coin
    }
    public Type type;
    public int amount;
}




















/*PlayerDataBase database;
const string DATABASE_FILE_NAME = @"PlayerDataBase.asset";
const string DATABASE_FILE = @"DataBase";
const string FULL_PATH = "Assets/" + DATABASE_FILE + "/" + DATABASE_FILE_NAME;
// Use this for initialization
void Start()
{
    database = AssetDatabase.LoadAssetAtPath(FULL_PATH, typeof(PlayerDataBase)) as PlayerDataBase;
    if (database == null)
    {
        if (!AssetDatabase.IsValidFolder("Assets/" + DATABASE_FILE_NAME))
            AssetDatabase.CreateFolder("Assets", DATABASE_FILE);

        database = ScriptableObject.CreateInstance<PlayerDataBase>();
        AssetDatabase.CreateAsset(database, FULL_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
*/