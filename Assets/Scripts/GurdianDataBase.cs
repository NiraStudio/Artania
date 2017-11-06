using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurdianDataBase : ScriptableObject
{

    [SerializeField]
    List<GurdianClass> DB = new List<GurdianClass>();


    public GurdianClass GetPlayerById(int ID)
    {
        GurdianClass t = new GurdianClass();
        for (int i = 0; i < DB.Count; i++)
        {
            if (DB[i].Id == ID)
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
    public GurdianClass GetByIndex(int index)
    {
        return DB[index];
    }
    public int DBLength
    {
        get { return DB.Count; }
    }

}

[System.Serializable]
public class GurdianClass
{
    public string ENName,PRName;
    public int Id;
    public Gurdian.GurdianModel Kind;
    public GameObject Prefab;
    public Sprite Image;
    public Price Price;
    public int lvlNeed;
    public string EnglishDes,PersianDes;
}
