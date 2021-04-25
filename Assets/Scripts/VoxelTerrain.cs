using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTerrain : MonoBehaviour
{
	static float voxelThiccc = 0.5f;

	[SerializeField] uint width = 10;
	[SerializeField] uint height = 10;
	[SerializeField] uint depth = 10;

	byte[] chunkData;

	[SerializeField]
	Material material;
	MeshFilter meshFilter;

	Vector3Int[] neighbours = new Vector3Int[]
	{
		new Vector3Int( 1,  0,  0),
		new Vector3Int(-1,  0,  0),
		new Vector3Int( 0,  1,  0),
		new Vector3Int( 0, -1,  0),
		new Vector3Int( 0,  0,  1),
		new Vector3Int( 0,  0, -1)
	};

	Vector3[] cubeVertices = new Vector3[] {
		new Vector3(  .5f,  .5f,  .5f ), // right
		new Vector3(  .5f,  .5f, -.5f ),
		new Vector3(  .5f, -.5f,  .5f ),
		new Vector3(  .5f, -.5f, -.5f ),

		new Vector3( -.5f, -.5f, -.5f ),
		new Vector3( -.5f,  .5f, -.5f ), // left
		new Vector3( -.5f, -.5f,  .5f ),
		new Vector3( -.5f,  .5f,  .5f ),

		new Vector3( -.5f,  .5f, -.5f ), // top
		new Vector3(  .5f,  .5f, -.5f ),
		new Vector3( -.5f,  .5f,  .5f ),
		new Vector3(  .5f,  .5f,  .5f ),

		new Vector3( -.5f, -.5f, -.5f ), // bottom
		new Vector3( -.5f, -.5f,  .5f ),
		new Vector3(  .5f, -.5f, -.5f ),
		new Vector3(  .5f, -.5f,  .5f ),

		new Vector3( -.5f, -.5f,  .5f ), // front
		new Vector3( -.5f,  .5f,  .5f ),
		new Vector3(  .5f, -.5f,  .5f ),
		new Vector3(  .5f,  .5f,  .5f ),

		new Vector3( -.5f, -.5f, -.5f ), // back
		new Vector3(  .5f, -.5f, -.5f ),
		new Vector3( -.5f,  .5f, -.5f ),
		new Vector3(  .5f,  .5f, -.5f ),
	};

	Vector3[] cubeNormals = new Vector3[6] {
		new Vector3( 1.0f,  0.0f,  0.0f), // l.0ft
		new Vector3(-1.0f,  0.0f,  0.0f), // right

		new Vector3( 0.0f,  1.0f,  0.0f), // top
		new Vector3( 0.0f, -1.0f,  0.0f), // bottom

		new Vector3( 0.0f,  0.0f,  1.0f), //.0front
		new Vector3( 0.0f,  0.0f, -1.0f), // back
	};

	Vector2[] cubeUvs = new Vector2[] {
		new Vector2( 1.0f, 0.0f ), // l.0ft
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),

		new Vector2( 1.0f, 0.0f ), // right
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),

		new Vector2( 0.0f, 1.0f ), // top
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),

		new Vector2( 0.0f, 1.0f ), // bottom
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),

		new Vector2( 0.0f, 1.0f ), // front
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),

		new Vector2( 0.0f, 1.0f ), // back
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),
	};


    void Start()
    {
		meshFilter = GetComponent<MeshFilter>();

		// Make voxel array
		chunkData = new byte[width * height * depth];

		for(uint i = 0; i < width * height * depth; i++) {
			chunkData[i] = (byte) (Random.Range(0, 2) - 1);
		}


		// Find number of faces
		List<Vector3> vertexList = new List<Vector3>();
		List<int> indexList = new List<int>();
		List<Vector3> normalsList = new List<Vector3>();
		List<Vector2> uvsList = new List<Vector2>();
		int vertexCount = 0;

		for(int z = 0; z < depth; z++)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					long index = x + width * (y + depth * z);
					if(chunkData[index] != 0)
					{
						for(int i = 0; i < 6; i++) 
						{
							Vector3Int n = neighbours[i];

							int newX, newY, newZ;
							newX = x + n.x;
							newY = y + n.y;
							newZ = z + n.z;

							if( (newX > 0 && newX < width )  &&
								(newY > 0 && newY < height)  &&
								(newZ > 0 && newZ < depth ))
							{
								long newIndex = newX + width * (newY + depth * newZ);
								if(chunkData[newIndex] == 0)
								{
									Vector3 facePos = new Vector3(x, y, z);

									indexList.Add(vertexCount + 0);
									indexList.Add(vertexCount + 2);
									indexList.Add(vertexCount + 1);
									indexList.Add(vertexCount + 2);
									indexList.Add(vertexCount + 3);
									indexList.Add(vertexCount + 1);

									vertexList.Add((cubeVertices[i * 4 + 0] + facePos) * voxelThiccc); // relative top left
									vertexList.Add((cubeVertices[i * 4 + 1] + facePos) * voxelThiccc); // relative bottom left
									vertexList.Add((cubeVertices[i * 4 + 2] + facePos) * voxelThiccc); // relative bottom right
									vertexList.Add((cubeVertices[i * 4 + 3] + facePos) * voxelThiccc); // relative top right

									normalsList.Add(cubeNormals[i]);
									normalsList.Add(cubeNormals[i]);
									normalsList.Add(cubeNormals[i]);
									normalsList.Add(cubeNormals[i]);

									vertexCount += 4;
								}
							}
						}
					}
				}
			}
		}

		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		Debug.Log("Vertex count: " + vertexCount);
		Debug.Log("Vertex count: " + vertexList.Count);
		Debug.Log("Index count: " + vertexList.Count);

		mesh.vertices = vertexList.ToArray();
		mesh.normals = normalsList.ToArray();
		mesh.triangles = indexList.ToArray();

		mesh.Optimize();
		//meshFilter.material = material;
    }
}
