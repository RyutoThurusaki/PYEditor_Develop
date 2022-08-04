#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PropPlacer_Scene : EditorWindow
{
    //�V�[���r���[��̏����AShortcut��p
    //Unity��Shortcut���ƃR���t���N�g�x�����ʓ|�Ȃ̂ŗ��֐��d���ł��̎���
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