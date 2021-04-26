using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainSystemUsingCubeMarching))]
public class GenerateTerrainButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainSystemUsingCubeMarching script = (TerrainSystemUsingCubeMarching) target;
        if(GUILayout.Button("GenerateTerrain"))
        {
            script.Generate();
        }
        if (GUILayout.Button("GenerateChunks"))
        {
            script.GenerateChunks();
        }
    }

}

