using UnityEngine;
#if UNITY_EDITOR_WIN

using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;


public class SceneSelector : EditorWindow
{
    string[] TargetScenes;
    Texture2D Sceneicon;
    Texture2D Foldericon;

    //シーンを検索するフォルダ
    string Scenespath = "Assets/Scenes/";

    [MenuItem("Tools/SceneSelector")]
    private static void Create()
    {
        GetWindow<SceneSelector>("SceneSelector");
    }

    private void Awake()
    {
        Sceneicon = (Texture2D)EditorGUIUtility.Load("d_SceneAsset Icon");
        Foldericon = (Texture2D)EditorGUIUtility.Load("d_Folder Icon");

        SceneSearch();
    }

    void OnGUI()
    {
        EditorGUILayout.Space(10);

        //取得したシーン一覧表示
        foreach (string i in TargetScenes)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(AssetDatabase.GUIDToAssetPath(i).Replace(Scenespath, ""),GUILayout.MaxHeight(20)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(i));
                }
            }
            
            if (GUILayout.Button(Sceneicon, GUILayout.MaxWidth(30) ,GUILayout.MaxHeight(20)))
            {
                //GUIDからPathを、Pathから引き出したSceneにping
                var path = AssetDatabase.GUIDToAssetPath(i);
                var getscene = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                EditorGUIUtility.PingObject(getscene);
            }

            if (GUILayout.Button(Foldericon, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20)))
            {
                //GUIDからPath取ってLightingDataにすげ替えてping
                var path = AssetDatabase.GUIDToAssetPath(i);
                var folder = path.Replace(".unity", "/LightingData.asset");
                var pingtarget = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);

                EditorGUIUtility.PingObject(pingtarget);
            }
            GUILayout.EndHorizontal();

        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("リロード"))
        {
            SceneSearch();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.HelpBox(Scenespath + "のシーンを一覧表示する拡張 \\ クリックでそのシーンを開きます(Windowsのみ対応)", MessageType.Info);

        //ライトベイクボタン欲しい
        //Lightmaping.Bakeはうまくいかなかったメモ
    }


    void SceneSearch()
    {
        TargetScenes = AssetDatabase.FindAssets("t:Scene", new string[] {Scenespath});
    }

    //Scenes配下のシーンファイルを検索して表示、シーン名クリックでそのシーンを開くやーつ
    //さくしゃ→2022のうおずみ　なにかあれば
    //This work is licensed under a CC0 1.0 Universal
}

#endif