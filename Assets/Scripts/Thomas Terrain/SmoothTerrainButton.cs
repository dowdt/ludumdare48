using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Smoother))]
public class SmoothTerrainButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Smoother script = (Smoother) target;
        if(GUILayout.Button("Smooth"))
        {
            script.Smooth();
        }
    }

}

