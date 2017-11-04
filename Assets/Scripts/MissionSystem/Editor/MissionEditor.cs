using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Alpha.MissionSystem
{

    public class MissionEditor : EditorWindow
    {
        MissionDataBase MissionDataBase;

        const string FileName = @"MissionDataBase.asset";
        const string FolderName = @"DataBase/Mission";
        const string FullPathName = @"Assets/" + FolderName + "/" + FileName;
        static Vector2 size = new Vector2(1000, 300);

        Vector2 scrollPos;
        Mission m = new Mission();


        [MenuItem("Alpha/Create Mission")]
        public static void Init()
        {
            MissionEditor window = EditorWindow.GetWindow<MissionEditor>();
            window.minSize = size;
            window.maxSize = size;
            window.title = "Mission Creator";
            window.Show();
        }
        void OnEnable()
        {
            m = new Mission();
            MissionDataBase = AssetDatabase.LoadAssetAtPath(FullPathName, typeof(MissionDataBase)) as MissionDataBase;
            if (MissionDataBase == null)
            {
                if (!AssetDatabase.IsValidFolder(@"Assets/" + FolderName))
                    AssetDatabase.CreateFolder(@"Assets/DataBase", "Mission");

                MissionDataBase = ScriptableObject.CreateInstance<MissionDataBase>();
                AssetDatabase.CreateAsset(MissionDataBase, FullPathName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
        void OnGUI()
        {

            GUILayout.BeginHorizontal();
            MakePanel();
            EditPanel();
            GUILayout.EndHorizontal();



        }
        void MakePanel()
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.Width(size.x / 3));

            GUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true));
            GUILayout.Label("Mission Persian Title");
            m.PersianTitle = GUILayout.TextField(m.PersianTitle);

            GUILayout.Label("Mission English Title");
            m.EnglishTitle = GUILayout.TextField(m.EnglishTitle);

            GUILayout.BeginHorizontal();
            m.type = (Mission.Type)EditorGUILayout.EnumPopup("Mission Type:", m.type);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Object Amount");
            m.Times = EditorGUILayout.IntField(m.Times);
            GUILayout.EndHorizontal();


            m.Reward.type = (reward.Type)EditorGUILayout.EnumPopup("Reward Type:", m.Reward.type);
            m.level = (Alpha.MissionSystem.Mission.Level)EditorGUILayout.EnumPopup("Mission Level:", m.level);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Reward Amount");
            m.Reward.amount = EditorGUILayout.IntField(m.Reward.amount);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Should Be Done In One Match?");
            m.InMatch = EditorGUILayout.Toggle(m.InMatch);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("is it increaseable?");
            m.Increaseable = EditorGUILayout.Toggle(m.Increaseable);
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Create Mission"))
            {
                MissionDataBase.CreateMission(new Mission(m.PersianTitle,m.EnglishTitle,m.type, m.Times, m.Reward.type, m.Reward.amount, m.InMatch,MissionDataBase.IdGiver()));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


        }
        void EditPanel()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true), GUILayout.Width((size.x / 3)*2));

            for (int i = 0; i < MissionDataBase.DB.Count; i++)
            {
                GUILayout.BeginVertical("Box");

                Mission b = MissionDataBase.GetByIndex(i);

                GUILayout.BeginHorizontal();

                GUILayout.Label("Mission Persian Title");
                b.PersianTitle = GUILayout.TextField(b.PersianTitle);

                GUILayout.Label("Mission English Title");
                b.EnglishTitle = GUILayout.TextField(b.EnglishTitle);

                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    MissionDataBase.DeleteAtIndex(i);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                b.type = (Mission.Type)EditorGUILayout.EnumPopup("Mission Type:", b.type);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("Object Amount");
                b.Times = EditorGUILayout.IntField(b.Times);
                GUILayout.EndVertical();


                b.Reward.type = (reward.Type)EditorGUILayout.EnumPopup("Reward Type:", b.Reward.type);
                b.level = (Alpha.MissionSystem.Mission.Level)EditorGUILayout.EnumPopup("Mission Level:", b.level);

                GUILayout.BeginVertical();
                GUILayout.Label("Reward Amount");
                b.Reward.amount = EditorGUILayout.IntField(b.Reward.amount);
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Should Be Done In One Match?");
                b.InMatch = EditorGUILayout.Toggle(b.InMatch);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("is it increaseable?");
                b.Increaseable = EditorGUILayout.Toggle(b.Increaseable);
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                GUILayout.Label("ID :"+b.Id);
                GUILayout.EndVertical();


                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();

        }
    }
}