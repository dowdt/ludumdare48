using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VoxelTerrain))]
public class CustomButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoxelTerrain myScript = (VoxelTerrain) target;
        if(GUILayout.Button("Generate"))
        {
            myScript.Execute();
        }
    }

}

