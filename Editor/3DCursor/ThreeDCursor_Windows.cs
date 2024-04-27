using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThreeDCursor_SettingWindow;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;

//3DCursorを開いたとき最初に表示される開きっぱなしのウィンドウ。
//もろもろの処理はこのクラスで行う。
public class ThreeDCursor_SettingWindow : EditorWindow
{
    public enum KEY_BINDMODE
    {
        Blender27x,
        Blender,
    }
    private KEY_BINDMODE Bindmode;

    public enum LANGUAGE
    {
        Japanese,
        English,
    }
    private LANGUAGE Languagemode;

    private bool Debugmode = false;

    bool shift = false;
    bool alphabet = false;

    //3DCursor
    ThreeDCursor_DrawGizmo DrawGizmo;

    [MenuItem("PYEditor/3DCursor")]
    private static void Create()
    {
        GetWindow<ThreeDCursor_SettingWindow>("3DCursor");
    }
    private void OnEnable()
    {
        //シーンアップデートの開始
        SceneView.duringSceneGui += Update_GetKeyInput;

        //設定の取得
        Bindmode = (KEY_BINDMODE)EditorPrefs.GetInt("3DC_Bindmode");
        Languagemode = (LANGUAGE)EditorPrefs.GetInt("3DC_Languagemode");

        //Gizmoファイル確認の下ごしらえ
        string gizmopng_master = "Packages/com.github.ryutothurusaki.PYEditor/Editor/3DCursor/3DCursor.png";
        string gizmopng_copy = "Assets/Gizmos/3DCursor.png";
        //Gizmo用ファイルの確認、ない場合は生成
        if (!System.IO.File.Exists(gizmopng_copy))
        {
            Debug.LogWarning("必要なアセットが見つかりませんでした。生成します。says 3DCursor");

            if (!System.IO.Directory.Exists("Assets/Gizmos"))
            {
                //フォルダが存在しない場合は無言で生成
                System.IO.Directory.CreateDirectory("Assets/Gizmos");
            }

            if (System.IO.File.Exists(gizmopng_master))
            {
                //PackagesのReadonlyな元ファイルをAssetsにコピー。
                System.IO.File.Copy(gizmopng_master, gizmopng_copy);
                AssetDatabase.Refresh();
                //完了した旨のログだそうと思ったけど、可能性の薄いことに対して丁寧にログ出すよりConsole汚さないほうが優先なのでやめた。
            }
            else
            {
                Debug.LogError("コピー元のファイルが見つかりませんでした。エディタ拡張を再インストールしてください。says 3DCursor");
            }
        }

        //Gizmoの生成
        InitializeGizmoObject();
    }
    private void OnDisable()
    {
        //シーンアップデートの終了
        SceneView.duringSceneGui -= Update_GetKeyInput;

        //一時オブジェクトの削除
        DrawGizmo.EraseThis();
    }

    public void OnGUI()
    {
        //infoText
        switch (Languagemode)
        {
            case LANGUAGE.Japanese:
                EditorGUILayout.HelpBox("このウィンドウは開いたままにしてください。", MessageType.Info);
                break;

            case LANGUAGE.English:
                EditorGUILayout.HelpBox("このウィンドウは開いたままにしてください。a", MessageType.Info);
                break;
        }
        //End InfoText

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();//~~~~~~~~~~~~

        //Settings
        GUILayout.Label("Settings");
        Bindmode = (KEY_BINDMODE)EditorGUILayout.EnumPopup("Select Bind ver", Bindmode);
        Languagemode = (LANGUAGE)EditorGUILayout.EnumPopup("Select Language", Languagemode);

        GUILayout.Space(10);

        if (EditorGUI.EndChangeCheck())//~~~~~~~~~~~~
        {
            EditorPrefs.SetInt("3DC_Bindmode", ((int)Bindmode));
            EditorPrefs.SetInt("3DC_Languagemode", ((int)Languagemode));
        }

        GUILayout.Space(10);

        Debugmode = EditorGUILayout.Toggle("Debug Mode", Debugmode);
        if (Debugmode)
        {
            GUILayout.Label("Shift Key is " + shift.ToString());
            GUILayout.Label("Alphabet Key is " + alphabet.ToString());
        }
        //End Settings
    }

    private void Update_GetKeyInput(SceneView scene)
    {
        var Currentkey = Event.current;
        if (Currentkey.keyCode == KeyCode.None) { return; }

        if (Currentkey.type == EventType.KeyDown)
        {
            //change to true
            if (Currentkey.keyCode == KeyCode.LeftShift){shift = true;}
            if (Currentkey.keyCode == KeyCode.S){alphabet = true;}
            ShowDebug(Currentkey.ToString());
        }
        else if(Currentkey.type == EventType.KeyUp)
        {
            //change to false
            if (Currentkey.keyCode == KeyCode.LeftShift) {shift = false;}
            if (Currentkey.keyCode == KeyCode.S){alphabet = false;}
            ShowDebug(Currentkey.ToString());
        }

        if (Currentkey.keyCode != KeyCode.LeftShift && Currentkey.keyCode != KeyCode.S) 
        { shift = false; alphabet = false;  }

        if (shift && alphabet) 
        { 
            //Shortcut
            OpenSnapwindow();
            ShowDebug("Open SnapWindow");

            shift= false;
            alphabet= false;
        }
    }

    private void OpenSnapwindow()
    {
        var window = CreateInstance<ThreeDCursor_SnapWindow>();
        window.ShowAuxWindow();

        Vector2 mousepos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        window.position = new Rect(mousepos.x - 100, mousepos.y - 100, 200, 200);
    }

    private void ShowDebug(string logtext)
    {
        //コンソールにデバッグログを出す。エディタ拡張の設定から、表示するかどうか選択できる。
        if (Debugmode)
        {
            Debug.Log(logtext + " says 3DCursor");
        }
    }

    private void InitializeGizmoObject()
    {
        //シーンにオブジェクトを生成、生成物であると明示する
        GameObject targetobject = new GameObject("--Dont Delete--");
        DrawGizmo = (ThreeDCursor_DrawGizmo)targetobject.AddComponent(typeof(ThreeDCursor_DrawGizmo));
        DrawGizmo.isAutogenerate = true;
    }

    //3DCursorのgetset
    public Vector3 get_3dcursor()
    {
        return DrawGizmo.center;
    }
    public void set_3dcursor(Vector3 vector3)
    {
        DrawGizmo.center = vector3;
    }

    //SelectObjectsのPositionをgetset
    public Vector3 get_selectobjects()
    {
        GameObject[] selectobjects = Selection.gameObjects;

        Vector3 addvector = Vector3.zero;

        for (int i = 0; i < selectobjects.Length; i++)
        {
            addvector += selectobjects[i].transform.position;
        }

        return addvector / selectobjects.Length;
    }
    public void set_selectobjects(Vector3 setposition)
    {
        GameObject[] selectobjects = Selection.gameObjects;

        Undo.RecordObjects(selectobjects,"Move to 3DCursor the Objects");
        for (int i = 0; i < selectobjects.Length; i++)
        {
            selectobjects[i].transform.position = setposition;
        }
    }
}



//コマンド入力したときに一時的に表示されるウィンドウ。
//ThreeDCursor_SettingWindow（↑）の関数を叩くだけ。処理はしない。
public class ThreeDCursor_SnapWindow : EditorWindow
{

    ThreeDCursor_SettingWindow.LANGUAGE Languagemode;
    ThreeDCursor_SettingWindow SW;
    private static void Create()
    {
        GetWindow<ThreeDCursor_SnapWindow>("3DCursor");
    }
    private void OnEnable()
    {
        Languagemode = (ThreeDCursor_SettingWindow.LANGUAGE)EditorPrefs.GetInt("3DC_Languagemode");
        SW = GetWindow<ThreeDCursor_SettingWindow>("3DCursor");
    }

    public void OnGUI()
    {
        switch (Languagemode)
        {
            case LANGUAGE.Japanese:
                if (GUILayout.Button("選択物→グリッド")) { }
                if (GUILayout.Button("選択物→カーソル")) { }
                if (GUILayout.Button("選択物→カーソル（オフセット維持）")) { SW.set_selectobjects(SW.get_3dcursor()); }
                if (GUILayout.Button("選択→アクティブ")) { }

                GUILayout.Label("---------------");

                if (GUILayout.Button("カーソル→選択物")) { SW.set_3dcursor(SW.get_selectobjects()); }
                if (GUILayout.Button("カーソル→ワールド原点")) { SW.set_3dcursor(Vector3.zero); }
                if (GUILayout.Button("カーソル→グリッド")) { }
                if (GUILayout.Button("カーソル→アクティブ")) { }
                break;

            case LANGUAGE.English:
                GUILayout.Label("someone translate!!!!!!");
                if (GUILayout.Button("選択物→グリッド")) { }
                if (GUILayout.Button("選択物→カーソル")) { }
                if (GUILayout.Button("選択物→カーソル（オフセット維持）")) { }
                if (GUILayout.Button("選択→アクティブ")) { }

                GUILayout.Label("---------------");

                if (GUILayout.Button("カーソル→選択物")) { }
                if (GUILayout.Button("カーソル→ワールド原点")) { }
                if (GUILayout.Button("カーソル→グリッド")) { }
                if (GUILayout.Button("カーソル→アクティブ")) { }
                break;
        }


    }
}



//Gizmoを表示する用のコンポーネントに、ユーザーのためのInfoを表示するためだけのクラス。
[CustomEditor(typeof(ThreeDCursor_DrawGizmo))]
public class ThreeDCursor_Infomation : Editor
{
    float time = 0;
    ThreeDCursor_DrawGizmo script;
    public override void OnInspectorGUI()
    {
        script = target as ThreeDCursor_DrawGizmo;

        EditorGUILayout.HelpBox("3DCursorで使用するコンポーネントです",MessageType.Info);

        if (script.isAutogenerate)
        {
            EditorGUILayout.HelpBox("このコンポーネントとオブジェクトは自動で生成されました。削除しないでください。",MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("このコンポーネントはAddComponentしないでください。数秒後に自動で削除されます。", MessageType.Error);
        }
    }

    private void OnSceneGUI()
    {
        if (!script.isAutogenerate)
        {
            time += Time.deltaTime;
            if (time >= 25)
            {
                DestroyImmediate(script);
            }
        }
    }
}
#endif