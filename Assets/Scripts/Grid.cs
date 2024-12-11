using UnityEngine;
using System;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Material terrainMaterial;
    public Material edgeMaterial;
    public float scale = .1f;
    bool isWater;
    bool isSand;
    public float nodeRadius;
    float nodeDiameter;

    public LayerMask unwalkableMask;

    public int gridWorldSizeX = 100;
    public int gridWorldSizeY = 100;
    public int size = 100;
    public GameObject enemySpawn;
    public GameObject towerSpawn;

    public enum TerrainType
    {
        Water,
        Sand,
        Land
    }

    public TerrainType[,] terrainGrid;
    public float waterThreshold = .4f;
    public float sandThreshold = .5f;

    //store grid
    Node[,] grid;


    //start
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;

        // Create a new noise map for random generation heights
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        // Create edge map
        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        // Initialize terrain grid
        terrainGrid = new TerrainType[size, size];

        // Create our grid of cells
        grid = new Node[size, size];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSizeX / 2 - Vector3.forward * gridWorldSizeY / 2;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                // Determine terrain type
                float noiseValue = noiseMap[x, y] - falloffMap[x, y];
                if (noiseValue < waterThreshold)
                {
                    terrainGrid[x, y] = TerrainType.Water;
                }
                else if (noiseValue < sandThreshold)
                {
                    terrainGrid[x, y] = TerrainType.Sand;
                }
                else
                {
                    terrainGrid[x, y] = TerrainType.Land;
                }

                // Determine walkability
                bool isWater = terrainGrid[x, y] == TerrainType.Water;
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)) && !isWater;
                grid[x, y] = new Node(walkable, worldPoint, x, y, isWater);
            }
        }

        // Draw terrain and meshes
        DrawTerrainMesh(grid);
        DrawEdgeMesh(grid);
        DrawTexture(grid);
        prefabSpawns(grid);
    }

    public int MaxSize
    {
        get
        {
            return gridWorldSizeX * gridWorldSizeY;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridWorldSizeX && checkY >= 0 && checkY < gridWorldSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        var pos = transform.position;
        float percentX = ((worldPosition.x - pos.x) + gridWorldSizeX / 2) / gridWorldSizeX;
        float percentY = ((worldPosition.z - pos.z) + gridWorldSizeY / 2) / gridWorldSizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridWorldSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridWorldSizeY - 1) * percentY);
        return grid[x, y];
    }


    //create out vertices for meshs
    void DrawTerrainMesh(Node[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSizeX / 2 - Vector3.forward * gridWorldSizeY / 2;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Node cell = grid[x, y];
                //if were not draw our vertices
                if (!cell.isWater)
                {
                    Vector3 a = worldBottomLeft + new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = worldBottomLeft + new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = worldBottomLeft + new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = worldBottomLeft + new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }

        //add our vertices
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();



        //add our mesh
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    void prefabSpawns(Node[,] grid)
    {
        List<Vector3> validCoords = new List<Vector3>(); 

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Node cell = grid[x, y];
                TerrainType type = terrainGrid[x, y];  

                if (type == TerrainType.Sand)
                {

                    validCoords.Add(cell.worldPosition);
                }
            }
        }

        if (validCoords.Count >= 2)
        {

            int index1 = UnityEngine.Random.Range(0, validCoords.Count);
            Vector3 coord1 = validCoords[index1];


            Vector3 coord2 = Vector3.zero;
            bool found = false;

            while (!found)
            {
                int index2 = UnityEngine.Random.Range(0, validCoords.Count);


                if (index2 != index1 && Vector3.Distance(coord1, validCoords[index2]) >= 20)
                {
                    coord2 = validCoords[index2];
                    found = true;
                }
            }
            Pathfinding pathFinding = GetComponent<Pathfinding>();
            pathFinding.StartFindPath(coord1, coord2);

            //This code will need to be updated for the spawner to loop through all generated guys, but its very important that we set the enemy as a gameobject to get the units script off of it to set the target as it will not accept the target placed beforehand for some reason
            if(found)
            {
                var newObject2 = Instantiate(towerSpawn, coord1, Quaternion.identity).transform;
                newObject2.name = "player";

                for (int i = 0; i <= 5; i++)
                {

                    GameObject go = Instantiate(enemySpawn, coord2, Quaternion.identity);
                    go.name = "Enemey";
                    Units unit = go.GetComponent<Units>();
                    unit.target = newObject2;
                }

            }

            Debug.Log($"Selected coordinates: {coord1} and {coord2}");
        }
        else
        {
            Debug.LogWarning("Not enough sand cells to select two random points.");
        }
    }


    //draw our edges for our land
    void DrawEdgeMesh(Node[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSizeX / 2 - Vector3.forward * gridWorldSizeY / 2;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Node cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Node left = grid[x - 1, y];
                        //cehck where water is
                        if (left.isWater)
                        {
                            Vector3 a = worldBottomLeft + new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = worldBottomLeft + new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = worldBottomLeft + new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = worldBottomLeft + new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Node right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = worldBottomLeft + new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = worldBottomLeft + new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = worldBottomLeft + new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = worldBottomLeft + new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Node down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = worldBottomLeft + new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = worldBottomLeft + new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = worldBottomLeft + new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = worldBottomLeft + new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Node up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = worldBottomLeft + new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = worldBottomLeft + new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = worldBottomLeft + new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = worldBottomLeft + new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform, false);

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
    }


    //draw our textures
    void DrawTexture(Node[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                TerrainType type = terrainGrid[x, y];
                if (type == TerrainType.Water)
                    colorMap[y * size + x] = Color.blue;
                else if (type == TerrainType.Sand)
                    colorMap[y * size + x] = Color.yellow;
                else
                    colorMap[y * size + x] = Color.green;
            }
        }

        // Apply texture to the material
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }


    //draw our map
    void OnDrawGizmos()
    {
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSizeX / 2 - Vector3.forward * gridWorldSizeY / 2;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeY));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
            
            if (!Application.isPlaying) return;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Node cell = grid[x, y];
                    if (cell.isWater)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;
                    Vector3 pos = worldBottomLeft + new Vector3(x, 0, y);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
            
        }
    }
}
