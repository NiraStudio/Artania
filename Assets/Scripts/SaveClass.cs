using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;
using Alpha.MissionSystem;



[Serializable]
public class GlobalStat
{
    ///Value want to Save
    public bool FirstTime = true;
    public List<int> CharacterIds, GurdianIDs;
    public int CharacterId,gurdianId;
    public int coins;
    public int Gems;
    public int CoinIncreaseLVL, CoinPerHourLVL, MeatAmount;
    public int meat;
    public DateTime QuitTime,RewardTime;
    public int lvl;
    public int EXP;
    public Mission[] missions;
    public List<Level> levels;

}

[Serializable]
public class SaveClass 
{
    public GlobalStat MyGlobal=new GlobalStat();


    [NonSerialized]
    private readonly AesManaged _aes = new AesManaged();
    [NonSerialized]
    private RandomNumberGenerator _random = RandomNumberGenerator.Create();

    private byte[] _iv = new byte[16];
    public byte[] Key ;




    public void SaveToDisk()
    {
        Debug.Log("Saving...");
        var formatter = new BinaryFormatter();

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Data"));
        }


        byte[] buffer;
        using (var ms = new MemoryStream())
        {
            //Debug.Log("Pkg: "+MyGlobal.Packages1.Count);
            //for (int i = 0; i < MyGlobal.Packages1.Count;i++ )
            //{
            //    Debug.Log((i+1)+"- "+MyGlobal.Packages1[i]);
            //}
            //Debug.Log("Serializing " + this);
            formatter.Serialize(ms, this);
            buffer = ms.ToArray();
            //Debug.Log("Serialized " + this);
        }

        _random.GetBytes(_iv);

        using (var c = _aes.CreateEncryptor(Key, _iv))
        using (var fs = new FileStream(Path.Combine(Application.persistentDataPath, "Data/000.dat"), FileMode.Create, FileAccess.Write))
        using (var cs = new CryptoStream(fs, c, CryptoStreamMode.Write))
        {
            fs.Write(_iv, 0, _iv.Length);
            cs.Write(buffer, 0, buffer.Length);
            cs.FlushFinalBlock();
        }

    }

    public bool LoadFromDisk()
    {
        Debug.Log("Loading...");
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Data"))
            || !File.Exists(Path.Combine(Application.persistentDataPath, "Data/000.dat")))
        {
            Debug.LogWarning("No Save Data Exist Or Corrupted");
           // MyGlobal.Coins = 100;
            SaveToDisk();
            LoadFromDisk();
            return false;
        }


        var formatter = new BinaryFormatter();
        SaveClass instance;
        try
        {
            using (var fs = new FileStream(Path.Combine(Application.persistentDataPath, "Data/000.dat"), FileMode.Open, FileAccess.Read))
            {
                fs.Read(_iv, 0, _iv.Length);
                using (var c = _aes.CreateDecryptor(Key, _iv))
                using (var cs = new CryptoStream(fs, c, CryptoStreamMode.Read))
                {
                    instance = (SaveClass)formatter.Deserialize(cs);
                }
            }

            MyGlobal = instance.MyGlobal;
            //for (int i = 0; i < MyLevels.Count; i++)
            //{
            //    MyLevels[i] = instance.MyLevels[i];
            //}

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogWarning("No Save Data Exist Or Corrupted");
            SaveToDisk();
            return LoadFromDisk();
        }

    }
}
