using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTerrain : MonoBehaviour
{
	static float voxelThiccc = 4f;

	[SerializeField] float terrainWidth = 10;
	[SerializeField] float terrainHeight = 10;
	[SerializeField] float terrainDepth = 10;

	byte[] chunkData;

	MeshFilter meshFilter;
	MeshCollider meshCollider;

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
		new Vector3(-1.0f,  0.0f,  0.0f), // right
		new Vector3( 1.0f,  0.0f,  0.0f), // left

		new Vector3( 0.0f,  1.0f,  0.0f), // top
		new Vector3( 0.0f, -1.0f,  0.0f), // bottom

		new Vector3( 0.0f,  0.0f, -1.0f), // back
		new Vector3( 0.0f,  0.0f,  1.0f), //.0front
	};

	Vector2[] cubeUvs = new Vector2[] {
		new Vector2( 1.0f, 1.0f ), // right
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 0.0f ),

		new Vector2( 0.0f, 0.0f ),
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ), // left

		new Vector2( 0.0f, 1.0f ), // top
		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 0.0f ),

		new Vector2( 1.0f, 1.0f ), // bottom
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ),
		new Vector2( 0.0f, 0.0f ),

		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ), // front
		new Vector2( 0.0f, 0.0f ),

		new Vector2( 1.0f, 1.0f ),
		new Vector2( 1.0f, 0.0f ),
		new Vector2( 0.0f, 1.0f ), // back
		new Vector2( 0.0f, 0.0f ),
	};

	void Awake() {
		//GetComponent<MeshCollider>().enabled = false;
	//	GenerateMesh();
	//	GetComponent<MeshCollider>().enabled = true;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Vector3 dimensions = new Vector3(terrainWidth, terrainHeight, terrainDepth);
		Gizmos.DrawWireCube(transform.position + dimensions / 2, dimensions);
	}

    public void Execute()
    {
		meshFilter = GetComponent<MeshFilter>();
		int width  = (int) (terrainWidth  / voxelThiccc);
		int height = (int) (terrainHeight / voxelThiccc);
		int depth =  (int) (terrainDepth  / voxelThiccc);

		Debug.Log("Width: " + width + ", Height: " + height + ", Depth: " + depth);


		// Make voxel array
		chunkData = new byte[width * height * depth];

		for(uint i = 0; i < width * height * depth; i++) {
			chunkData[i] = 1;
		}
		
		Vector3 pos;
		for(int z = 1; z < depth-1; z++)
		{
			for(int y = 1; y < height-1; y++)
			{
				for(int x = 1; x < width-1; x++)
				{
					pos.x = x;
					pos.y = y;
					pos.z = z;

					long index = x + width * (y + height * z);

					if(Physics.OverlapSphere(pos * voxelThiccc + transform.position, 0.0f).Length > 0) {
						chunkData[index] = 0;
					}
				}
			}
		}

		// Find number of faces
		List<Vector3> vertexList = new List<Vector3>();
		List<int> indexList = new List<int>();
		List<Vector3> normalsList = new List<Vector3>();
		List<Vector2> uvsList = new List<Vector2>();
		int vertexCount = 0;
		long maxIndex = 0;
		long voxelCount = 0;

		for(int z = 0; z < depth; z++)
		{
			for(int y = 0; y < height - 1; y++)
			{
				for(int x = 0; x < width; x++)
				{
					long index = x + width * (y + height * z);
					if(chunkData[index] != 0)
					{
						voxelCount++;
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
								long newIndex = newX + width * (newY + height * newZ);
								if(maxIndex < newIndex)
									maxIndex = newIndex;
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
									
									uvsList.Add(cubeUvs[i * 4 + 0]);
									uvsList.Add(cubeUvs[i * 4 + 1]);
									uvsList.Add(cubeUvs[i * 4 + 2]);
									uvsList.Add(cubeUvs[i * 4 + 3]);
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
		Debug.Log("Index count: " + indexList.Count);
		Debug.Log("Max Index: " + maxIndex);
		Debug.Log("Voxel max: " + width * height * depth);
		Debug.Log("Voxel count: " + voxelCount);

		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.vertices = vertexList.ToArray();
		mesh.normals = normalsList.ToArray();
		mesh.triangles = indexList.ToArray();
		mesh.uv = uvsList.ToArray();

		mesh.Optimize();
    }
}
