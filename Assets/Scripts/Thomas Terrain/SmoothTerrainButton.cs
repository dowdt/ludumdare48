using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainSystemUsingCubeMarching))]
public class SmoothTerrainButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainSystemUsingCubeMarching script = (TerrainSystemUsingCubeMarching) target;
        if(GUILayout.Button("Generate"))
        {
            script.Generate();
        }
    }

}

