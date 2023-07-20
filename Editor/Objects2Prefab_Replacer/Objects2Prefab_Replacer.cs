# if UNITY_EDITOR

using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Objects2Prefab_Replacer : EditorWindow
{
    [MenuItem("Tools/Objects2Prefab_Replacer")]

    public static void ShowWindow()
    {
        GetWindow<Objects2Prefab_Replacer>("Objects2Prefab_Replacer");
    }

    private GameObject[] selectobjects;
    private Object Target;
    private string Addname = "_rep";
    private void OnGUI()
    {
        selectobjects = Selection.gameObjects;
        //重たいだろうけどエディタ上だけだし許容

        Target = EditorGUILayout.ObjectField("置き換え後のPrefab", Target, typeof(GameObject), false);
        Addname = EditorGUILayout.TextField("名前の末尾に付与する文字列", Addname);
        
        if (GUILayout.Button("レッツリプレイス！"))
        {
            Replace();
        }
    }

    private void Replace()
    {
        Undo.RecordObjects(selectobjects, "Mesh2Prefab Replaced");

        for (int i = 0; i < selectobjects.Length; i++)
        {
            GameObject place = selectobjects[i];

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(Target);

            //トランスフォームを適用
            obj.transform.position = place.transform.position;
            obj.transform.rotation = place.transform.rotation;
            obj.transform.localScale = place.transform.localScale;

            var parent = place.transform.parent;
            obj.transform.parent = parent.transform;

            obj.name = place.name + Addname;


            DestroyImmediate(selectobjects[i]);

            selectobjects[i] = obj;
        }

        Debug.Log(selectobjects.Length + "個のオブジェクトを置き換えました。");
    }
}

#endif