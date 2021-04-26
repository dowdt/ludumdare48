using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class GenerateTerrainButtonScript : MonoBehaviour {
	public abstract void GenerateChunks();
    public abstract void Populate();
    public abstract void Generate();
}
