using UnityEngine;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MoviePlayer : EditorWindow
{
    private Vector2Int resolution = new Vector2Int(50, 30);
    private VideoClip clip;

    [MenuItem("PYEditor/MoviePlayer")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<MoviePlayer>("MoviePlayer");
    }

    private void OnGUI()
    {

        GUILayout.BeginVertical();//#######
        {

            //集合体恐怖症プレイヤー
            for (int yi = 0; yi < resolution.y; yi++)
            {

                GUILayout.BeginHorizontal();//%%%%%%
                {
                    for (int xi = 0; xi < resolution.x; xi++)
                    {
                        //Player描画
                        EditorGUILayout.Toggle("", true, GUILayout.Width(15), GUILayout.Height(15));
                    }
                }
                GUILayout.EndHorizontal();//%%%%%%

            }

            resolution = EditorGUILayout.Vector2IntField("解像度", resolution);
            clip = (VideoClip)EditorGUILayout.ObjectField("動画", clip, typeof(VideoClip));

            GUILayout.BeginHorizontal();//======
            {
                GUILayout.Button("Start");
                GUILayout.Button("Stop");
            }
            GUILayout.EndHorizontal();//======

        }
        GUILayout.EndVertical();//#######

    }
}

#endif