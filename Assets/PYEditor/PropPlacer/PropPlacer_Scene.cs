#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PropPlacer_Scene : EditorWindow
{
    //シーンビュー上の処理、Shortcut専用
    //UnityのShortcutだとコンフリクト警告が面倒なので利便性重視でこの実装
    static PropPlacer_Scene()
    {
        SceneView.duringSceneGui += PropPlacer_Shortcut;
    }

    private static void PropPlacer_Shortcut(SceneView sceneView)
    {
        var keyinput = Event.current;

        if (keyinput.type == EventType.KeyDown && keyinput.keyCode == KeyCode.Q)
        {
            Debug.Log("hoge");
        }
    }
}

#endif