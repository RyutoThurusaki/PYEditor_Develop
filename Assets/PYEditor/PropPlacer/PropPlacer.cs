using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PropPlacer
{
    //�V�[���r���[��̏���

    static PropPlacer()
    {
        SceneView.duringSceneGui += OnGui;
    }

    private static void OnGui(SceneView sceneView)
    {

        Handles.BeginGUI();

        ShowButtons(sceneView.position.size);

        Handles.EndGUI();
    }

    /// <summary>
    /// �{�^���̕`��֐�
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        var UISize = 65;
        PropPlacerDB propPlacerDB = Resources.Load<PropPlacerDB>("PropPlacerDB");

    int hoge = 5;
        for(int i = 0; i < hoge; i++)
        {
            if (GUILayout.Button("hogehoge"))
            {
                Debug.Log("Press " + i + " Key");
            }
        }

        //#####�S�̉��z�u
        EditorGUILayout.BeginHorizontal();


        //-----Prefab�v�f�c�z�u
        using (var scrollView = new EditorGUILayout.ScrollViewScope(propPlacerDB.scrollPos_Prefab, GUILayout.Height(150)))
        {
            propPlacerDB.scrollPos_Prefab = scrollView.scrollPosition;

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < propPlacerDB.SetPrefabs.Count; i++)
            {
                EditorGUILayout.BeginVertical();


                if (GUILayout.Button(new GUIContent(propPlacerDB.PrefabTex[i]), GUILayout.Width(UISize), GUILayout.Height(UISize)))
                {
                    propPlacerDB.SelectPrefabNum = i;
                }

                EditorGUILayout.Space(5);

                propPlacerDB.SetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("", propPlacerDB.SetPrefabs[i], typeof(GameObject), false, GUILayout.Width(UISize), GUILayout.Height(20));

                EditorGUILayout.Space(5);

                propPlacerDB.PrefabPlacepoints[i] = (Transform)EditorGUILayout.ObjectField("", propPlacerDB.PrefabPlacepoints[i], typeof(Transform), true, GUILayout.Width(UISize));

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndHorizontal();
        }
        //-----Prefab�v�f�c�z�u�I����

        //�I�𒆂�Prefab�摜
        EditorGUILayout.LabelField(new GUIContent(propPlacerDB.PrefabTex[propPlacerDB.SelectPrefabNum]), GUILayout.Height(UISize * 2), GUILayout.Width(UISize * 2));

        //=====���j���[�c�z�u
        using (var scrollView = new EditorGUILayout.ScrollViewScope(propPlacerDB.scrollPos_Menu, GUILayout.Width(UISize * 2.3f)))
        {
            propPlacerDB.scrollPos_Menu = scrollView.scrollPosition;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(5);

            //�ꊇ�ŃX�|�[�����ݒ肷��t�B�[���h
            EditorGUILayout.LabelField("�X�|�[����ꊇ�ݒ�");
            propPlacerDB.DefaultPoint = (Transform)EditorGUILayout.ObjectField("", propPlacerDB.DefaultPoint, typeof(Transform), true, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //Positioning�ݒ�
            EditorGUILayout.LabelField("�|�W�V�������[�h");
            propPlacerDB.PositioningModeInt = EditorGUILayout.Popup("", propPlacerDB.PositioningModeInt, propPlacerDB.PositioningMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //��]�p���̎w��
            EditorGUILayout.LabelField("Rotation����");
            propPlacerDB.RotationModeInt = EditorGUILayout.Popup("", propPlacerDB.RotationModeInt, propPlacerDB.RotationMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //��]�p���̎w��
            EditorGUILayout.LabelField("Position����");
            propPlacerDB.PositionModeInt = EditorGUILayout.Popup("", propPlacerDB.PositionModeInt, propPlacerDB.PositionMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            if (GUILayout.Button("���b�c�v���[�X�I", GUILayout.Width(UISize * 2)))
            {
                //PlaceProp();
            }

            EditorGUILayout.Space(5);

            EditorGUILayout.EndVertical();
        }
        //=====���j���[�c�z�u�I����


        EditorGUILayout.EndHorizontal();
        //#####�S�̉��z�u�I����


        //�S�[�X�g�̕ύX
        if (GUI.changed)
        {
            //GhostReload();

            //�T���l�C���X�V
            for (int i = 0; i < propPlacerDB.SetPrefabs.Count; i++)
            {
                propPlacerDB.PrefabTex[i] = AssetPreview.GetAssetPreview(propPlacerDB.SetPrefabs[i]);
            }
        }

        //�ȉ��V���[�g�J�b�g
        var key = Event.current;
        var validate = key.type == EventType.KeyDown;

        if (validate && key.keyCode == KeyCode.Alpha0)
        {
            Debug.Log("hogehoge");
        }
    }
}