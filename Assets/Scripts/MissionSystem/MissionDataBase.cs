using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;



#endif
namespace Alpha.MissionSystem
{
    
    public class MissionDataBase : ScriptableObject
    {

        public List<Mission> DB;

        public Mission GetRandomMission()
        {
            return DB[Random.Range(0, DB.Count)];
        }
        public Mission GetByIndex(int i)
        {
            return DB[i];
        }
        public int IdGiver()
        {
            bool ta = false;
            int a = 0;
            do
            {
                ta = false;
                a = Random.Range(0, 999999);
                for (int i = 0; i < DB.Count; i++)
                {
                    if (DB[i].Id == a)
                    {
                        ta = true;
                        break;
                    }
                }
            } while (ta);
            return a;
        }
#if UNITY_EDITOR
        public void CreateMission(Mission m)
        {
            DB.Add(m);

            EditorUtility.SetDirty(this);
        }
        public void DeleteAtIndex(int i)
        {
            DB.RemoveAt(i);
            EditorUtility.SetDirty(this);
        }
#endif

    }
}