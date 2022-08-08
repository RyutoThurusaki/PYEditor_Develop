using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PropPlacer_Editor : EditorWindow
{
    public Vector2 scrollPos_Prefab;
    public Vector2 scrollPos_Menu;
    public int UISize = 65;

    //Dropdownlist
    public string[] RotationMode = new string[] { "�I���W�i��", "���_", "�X�i�b�v45", "�����X�i�b�v45", "�����_��Y", "�����_��XYZ" };
    public int RotationModeInt = 0;
    public string[] PositionMode = new string[] { "�x�[�V�b�N", "�[���؎̂�", "�X�i�b�v0.5", "�X�i�b�v1", "�m�C�Y" };
    public int PositionModeInt = 0;
    public string[] PositioningMode = new string[] { "�}�E�X�x�[�X", "���_�x�[�X" };
    public int PositioningModeInt = 0;

    //Raycast
    Camera SceneRaycam = UnityEditor.SceneView.lastActiveSceneView.camera;
    public Vector3 ViewPos = new Vector3(0.5f, 0.5f, 0);

    //Prefabselector
    public List<GameObject> SetPrefabs = new List<GameObject>(0);
    public List<Texture> PrefabTex = new List<Texture>(0);
    public int SelectPrefabNum = 0;

    //Spownsetting
    public List<Transform> PrefabPlacepoints = new List<Transform>(10);
    public Transform DefaultPoint;
    public GameObject LatestProp;

    //Situation
    public GameObject GhostProp;
    public bool TryBool = true;
    public bool Buttontoggle = true;

    //Debug


    [MenuItem("PYEditor/PropPlacer")]

    private static void Create()
    {
        GetWindow<PropPlacer_Editor>("PropPlacer");
    }

    public void OnGUI()
    {
        /*
        //ScriptableObject�̎擾
        var guids = UnityEditor.AssetDatabase.FindAssets("t:propplacerDB");
        if (guids.Length == 0)
        {
            throw new System.IO.FileNotFoundException("propplacerDB does not found");
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var ljhgv = AssetDatabase.LoadAssetAtPath<PropPlacerDB>(path);
        */


        //-----�S�̉��z�u
        EditorGUILayout.BeginHorizontal();

        //=====InfoBox�p�c�z�u
        EditorGUILayout.BeginVertical();

        //-----Prefab�v�f���z�u
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos_Prefab, GUILayout.Height(150), GUILayout.Height(UISize * 1.7f)))
        {
            scrollPos_Prefab = scrollView.scrollPosition;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < SetPrefabs.Count; i++)
            {
                EditorGUILayout.BeginVertical();


                if (GUILayout.Button(new GUIContent(PrefabTex[i]), GUILayout.Width(UISize), GUILayout.Height(UISize)))
                {
                    SelectPrefabNum = i;
                }

                EditorGUILayout.Space(5);

                SetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("", SetPrefabs[i], typeof(GameObject), false, GUILayout.Width(UISize), GUILayout.Height(20));
               
                EditorGUILayout.Space(5);

                PrefabPlacepoints[i] = (Transform)EditorGUILayout.ObjectField("", PrefabPlacepoints[i], typeof(Transform), false, GUILayout.Width(UISize));

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.HelpBox("aaa", MessageType.Info);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        //-----Prefab�v�f���z�u�I����

        EditorGUILayout.EndVertical();
        //=====InfoBox�p�c�z�u�I����

        //=====���j���[�c�z�u
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos_Menu, GUILayout.Width(UISize * 2.3f)))
        {
            scrollPos_Menu = scrollView.scrollPosition;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(5);

            //�ꊇ�ŃX�|�[�����ݒ肷��t�B�[���h
            EditorGUILayout.LabelField("�X�|�[����ꊇ�ݒ�");
            DefaultPoint = (Transform)EditorGUILayout.ObjectField("", DefaultPoint, typeof(Transform), true, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //Positioning�ݒ�
            EditorGUILayout.LabelField("�|�W�V�������[�h");
            PositioningModeInt = EditorGUILayout.Popup("", PositioningModeInt, PositioningMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //��]�p���̎w��
            EditorGUILayout.LabelField("Rotation����");
            RotationModeInt = EditorGUILayout.Popup("", RotationModeInt, RotationMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //��]�p���̎w��
            EditorGUILayout.LabelField("Position����");
            PositionModeInt = EditorGUILayout.Popup("", PositionModeInt, PositionMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            if (GUILayout.Button("���b�c�v���[�X�I(Q)", GUILayout.Width(UISize * 2)))
            {
                //PlaceProp();
            }

            EditorGUILayout.Space(5);

            //-----�����{�^��������
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("�|", GUILayout.Width(UISize)))
            {
                if (SetPrefabs.Count > 1)
                {
                    int count = SetPrefabs.Count - 1;
                    SetPrefabs.RemoveAt(count);
                    PrefabTex.RemoveAt(count);
                    PrefabPlacepoints.RemoveAt(count);
                }
            }

            if (GUILayout.Button("�{",GUILayout.Width(UISize)))
            {
                SetPrefabs.Add(null);
                PrefabTex.Add(null);
                PrefabPlacepoints.Add(null);
            }

            GUILayout.EndHorizontal();
            //-----�����{�^�������уI����

            UISize = EditorGUILayout.IntField("UISize", UISize,GUILayout.Width(UISize * 2));

            if (UISize < 29)
            {
                UISize = 30;
            }

            EditorGUILayout.EndVertical();
            //=====���j���[�c�z�u�I����
        }



        EditorGUILayout.EndHorizontal();
        //-----�S�̉��z�u�I����


        //�S�[�X�g�̕ύX
        if (GUI.changed)
        {
            //GhostReload();

            //�T���l�C���X�V
            for (int i = 0; i < SetPrefabs.Count; i++)
            {
                PrefabTex[i] = AssetPreview.GetAssetPreview(SetPrefabs[i]);
            }
        }
    }

    private void OnDisable()
    {
        EditorApplication.update -= PrefabGhost;
        DestroyImmediate(GhostProp);
    }

    private void OnEnable()
    {
        EditorApplication.update += PrefabGhost;

        if (SetPrefabs.Count > 0)
        {
            //�������Ȃ�
        }
        else
        {
            SetPrefabs.Add(null);
            PrefabTex.Add(null);
            PrefabPlacepoints.Add(null);
        }
    }

    public void PrefabGhost()
    {
        //update�Ŏ��s

        //�S�[�X�g�̈ʒu���X�V����
        if (GhostProp == null)
        {
            GhostReload();
        }
        else if (SetPrefabs[SelectPrefabNum] != null && PositioningModeInt == 1)
        {
            //���_���[�h
            var ray = SceneRaycam.ViewportPointToRay(ViewPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GhostProp.transform.position = hit.point;
            }
        }
        else if (SetPrefabs[SelectPrefabNum] != null && PositioningModeInt == 0)
        {
            //�}�E�X���[�h
            var ray = SceneRaycam.ViewportPointToRay(Input.mousePosition);
            float depth = -15;
            var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);
            GhostProp.transform.position = SceneRaycam.ScreenToWorldPoint(pos);
        }
    }

    public void GhostReload()
    {
        //�S�[�X�g�̃����[�h�p
        if (SetPrefabs[SelectPrefabNum] != null)
        {
            DestroyImmediate(GhostProp);

            GhostProp = (GameObject)PrefabUtility.InstantiatePrefab(SetPrefabs[SelectPrefabNum]) as GameObject;

            //�S�[�X�g�̃R���C�_�[���폜
            Collider[] GhostinCollider = GhostProp.GetComponentsInChildren<Collider>();
            foreach (Collider coli in GhostinCollider)
            {
                Debug.Log(coli);
                GameObject.DestroyImmediate(coli);
            }
        }
        else
        {
            if (GhostProp != null)
            {
                DestroyImmediate(GhostProp);
            }
        }
    }

    public void PlaceProp()
    {
        Transform parent = null;

        //�e�̐ݒ�
        if (PrefabPlacepoints[SelectPrefabNum] != null)
        {
            //�X�|�[���悪�ʎw�肳��Ă���ꍇ
            parent = PrefabPlacepoints[SelectPrefabNum];

        }
        else if (DefaultPoint != null)
        {
            //�X�|�[���悪�ʎw�肳��Ă��炸�A���ʐe���ݒ肳��Ă���ꍇ
            parent = DefaultPoint;
        }

        //�X�|�[������
        if (parent != null)
        {
            LatestProp = (GameObject)PrefabUtility.InstantiatePrefab(SetPrefabs[SelectPrefabNum]);

            Undo.RecordObject(LatestProp.gameObject, "PropPlace");

            LatestProp.transform.SetParent(parent);
            LatestProp.transform.SetPositionAndRotation(GhostProp.transform.position, GhostProp.transform.localRotation);
        }
        else
        {
            Debug.LogError("�X�|�[���悪���w��ł��B�ꊇ�w�肩�ʎw������Ă��������B Says PYEditor/PropPlacerDB");
        }
    }
}
#endif