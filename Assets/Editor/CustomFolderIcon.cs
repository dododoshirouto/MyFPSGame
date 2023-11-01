// CustomFolderIcon.cs
// copyright Kazuki_FUKUNAGA 2016 un-Pro.
// 参考：http://baba-s.hatenablog.com/entry/2015/05/07/103743

using UnityEditor;
using UnityEngine;
using System.IO;

public static class CustomFolderIcon
{
    [InitializeOnLoadMethod]
    private static void OnLoad()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI( string guid, Rect selectionRect )
    {
        string path = AssetDatabase.GUIDToAssetPath( guid );

        string fileName = path.Split('/')[path.Split('/').Length -1];
        Texture texture;
        if (texture = EditorGUIUtility.Load("CustomFolderIcon/" + fileName + ".png") as Texture)
        {
            Rect pos = selectionRect;
            if (IsUnityVersionNewerThan("5.0.1f1")) pos.x += 2;
            if (IsUnityVersionNewerThan("5.6.0f0")) pos.x -= 2;
            if (IsUnityVersionNewerThan("2017.0.0f0")) pos.x -= 2;
            if (pos.height > 16) {
                pos.height -= 16;
            }
            if (pos.height > 64) {
                pos.x += (pos.height - 64)/2;
                pos.y += (pos.height - 64)/2;
                pos.height = 64;
            }
            pos.width = pos.height;
	        GUI.DrawTexture(pos, texture);
        }
    }

    private static bool IsUnityVersionNewerThan(string version = "5.0.1f1") {
        int thresholdVer = ParseUnityVersionToInt(version);
        int nowVer = ParseUnityVersionToInt(Application.unityVersion);
        return (nowVer > thresholdVer);
    }

    private static int ParseUnityVersionToInt(string version = "5.0.1f1") {
        int[] num = new int[]{5, 0, 1, 1};
        num[0] = int.Parse(version.Split('.')[0]);
        num[1] = int.Parse(version.Split('.')[1]);
        num[2] = int.Parse(version.Split('.')[2].Split('f')[0]);
        num[3] = int.Parse(version.Split('.')[2].Split('f')[1]);

        int sum = 0;
        for (int i=0; i<num.Length; i++) {
            sum += num[i] * Mathf.RoundToInt(Mathf.Pow(10, (num.Length - i) * 3));
        }

        return sum;
    }


    private static string[] folders = new string[]
    {
        "/Scripts",
        "/Scripts/Shaders",
        "/StreamingAssets",
        "/Resources",
        "/ResourceData",
        "/ResourceData/Prefabs",
        "/ResourceData/Prefabs/Particles",
        "/ResourceData/Models",
        "/ResourceData/Models/Materials",
        "/ResourceData/Models/Textures",
        "/ResourceData/Audio",
        "/ResourceData/Sprites",
        "/ResourceData/Textures",
        "/ResourceData/Fonts",
        "/ResourceData/Animator",
        "/ResourceData/Animator/Animation",
        "/Scenes",
        "/Editor Default Resources/Gizmos",
    };

    [MenuItem ("Tools/Custom Folder Icon/Create Default Folders")]
    private static void CreateDefaultFolders() {
        foreach (string fname in folders)
        {
            Directory.CreateDirectory(Application.dataPath+fname);
        }
        AssetDatabase.Refresh();
    }
}
