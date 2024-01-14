using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Vector2 flatPoints; // min / max
    public float flatPointsHeight;
    public Vector2 offset;
    public bool useCustomOffset;

    // Set the dimensions of the terrain
    public int width = 256;
    public int height = 256;
    
    // Set the scale 
    public float scale = 20f;

    void Start()
    {
        // Call the terrain generation function when the script starts
        //GenerateTerrain();
        GenerateTerrain();
        //InvokeRepeating("AddTrees", 1f, 2f);
    }

    private void Update() {
        //GenerateTerrain();
        //offset.x += Time.deltaTime;
    }

    void AddTree(Vector3 treeSpot)
    {
        float ran = Random.Range(0, 1000);
        if(ran <= 1)
        {
            Terrain terrain = GetComponent<Terrain>();
            TreeInstance tree = new TreeInstance();
            tree.position = new Vector3(treeSpot.x / width, 0, treeSpot.z / height);
            tree.prototypeIndex = Random.Range(0, 2);
            tree.widthScale = 1f;
            tree.heightScale = 1f;
            tree.color = Color.white;
            tree.lightmapColor = Color.white;
            terrain.AddTreeInstance(tree);
            terrain.Flush();
        }
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();

        // Generate the terrain and apply it to the Terrain component
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the resolution and size of the terrain
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, 20, height);

        // Generate heights using Perlin noise
        terrainData.SetHeights(0, 0, GenerateHeights());
        
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
    
        // Use offset to make the terrain unique each time

        if(!useCustomOffset)
        {
            offset = new Vector2(Random.Range(0, 9999), Random.Range(0, 9999));
        }

        // Loop through each point on the terrain
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                // Define the flat area
                if (x >= flatPoints.x && x <= flatPoints.y && y >= flatPoints.x && y <= flatPoints.y)
                {
                    // Set the height to a constant value 0-1
                    heights[x, y] = flatPointsHeight;
                }
                else
                {
                    // Use Perlin noise to generate terrain heights outside the flat area
                    heights[x, y] = Mathf.PerlinNoise((float)x / width * scale + offset.x, (float)y / height * scale + offset.y);
                    AddTree(new Vector3(x, 0, y));
                }
            }
        }

        return heights;
    }
}
