using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class t : EditorWindow {
    GurdianDataBase itemDataBase;

    const string FileName = @"GurdianDataBase.asset";
    const string FolderName = @"DataBase";
    const string FullPathName = @"Assets/" + FolderName + "/" + FileName;



    [MenuItem("Item/Create Item")]
    public static void Init()
    {
        t window = EditorWindow.GetWindow<t>();
        window.minSize = new Vector2(200, 300);
        window.name = "Item Creator";
        window.Show();
    }
    void OnEnable()
    {
        itemDataBase = AssetDatabase.LoadAssetAtPath(FullPathName, typeof(GurdianDataBase)) as GurdianDataBase;
        if (itemDataBase == null)
        {
            if (!AssetDatabase.IsValidFolder(@"Assets/" + FolderName))
                AssetDatabase.CreateFolder(@"Assets", FolderName);

            itemDataBase = ScriptableObject.CreateInstance<GurdianDataBase>();
            AssetDatabase.CreateAsset(itemDataBase, FullPathName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}
