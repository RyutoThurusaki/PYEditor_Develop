#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDCursor_DrawGizmo : MonoBehaviour
{
    [HideInInspector]
    public Vector3 center = Vector3.zero;
    [HideInInspector]
    public Boolean isAutogenerate = false;
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(center, "3DCursor", false);
    }

    public void EraseThis()
    {
        GameObject.DestroyImmediate(transform.gameObject);
    }
}

#endif