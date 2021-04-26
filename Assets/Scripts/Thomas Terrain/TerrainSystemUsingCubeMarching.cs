
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TerrainSystemUsingCubeMarching : MonoBehaviour
{

    public void Generate()
    {
        GenerateChunks();
        foreach (var item in chunks)
        {
            item.Generate(this);
        }

    }



    public GameObject chunk;

    public void GenerateChunks()
    {
        int len = terrainDepth * terrainWidth * terrainHeight;
        chunks.Clear();

        int index = 0;
        foreach (var item in GetComponentsInChildren<TerrainChunk>())
        {
            if (index >= len)
            {
                DestroyImmediate(item.gameObject);
            }
            else {
                chunks.Add(item);
                item.terrain = this;
            }

            index++;
        }

        for (int i = chunks.Count; i < len; i++)
        {
            TerrainChunk item = Instantiate(chunk,transform).GetComponent<TerrainChunk>();
            chunks.Add(item);
            item.terrain = this;
        }

        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                for (int z = 0; z < terrainDepth; z++)
                {
                    int i = x + terrainWidth * (y + terrainHeight * z);
                    chunks[i].transform.position = transform.position + new Vector3(x*chunkWidth*voxelSize,y*chunkHeight* voxelSize, z*chunkDepth* voxelSize);
                }
            }
        }

    }

   

    [Header("Chunks")]
    [Range(1, 10)]
    [SerializeField]
    int terrainDepth = 3;
    [Range(1, 10)]
    [SerializeField]
    int terrainWidth = 3;
    [Range(1, 10)]
    [SerializeField]
    int terrainHeight = 2;

    [Header("General")]

    public List<TerrainChunk> chunks = new List<TerrainChunk>();

    [Tooltip("Size of every voxel")]
    [SerializeField]
    public float voxelSize = 2f;
    [Range(10, 50)]
    [SerializeField]
    public int chunkDepth = 50;
    [Range(10, 50)]
    [SerializeField]
    public int chunkWidth = 50;
    [Range(10, 50)]
    [SerializeField]
    public int chunkHeight = 50;

    [Tooltip("Layers of colliders that make up the caves")]
    [SerializeField]
    public LayerMask Mask;

    [Header("Terrain")]
    [Range(0, 1)]
    [SerializeField]
    public float Height = 0.5f;
    [SerializeField]
    public bool smooth = true;


    [Tooltip("Size of physics OverlapSphere check")]
    [SerializeField]
    public float SmoothAmount = 4f;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 1f);
        Vector3 scale = new Vector3(terrainWidth * chunkWidth * voxelSize, terrainHeight * chunkHeight * voxelSize, terrainDepth * chunkDepth * voxelSize);
        Gizmos.DrawWireCube(transform.position+ scale*0.5f, scale+new Vector3(10,10,10));
    }



}