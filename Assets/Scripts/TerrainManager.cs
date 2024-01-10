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
    
    // Set the scale to control the terrain features
    public float scale = 20f;

    void Start()
    {
        // Call the terrain generation function when the script starts
        //GenerateTerrain();
    }

    private void Update() {
        GenerateTerrain();
        offset.x += Time.deltaTime;
    }

    void GenerateTerrain()
    {
        // Get the Terrain component attached to the GameObject
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
            // Define the flat area within specific bounds (e.g., from x=50 to x=100 and y=50 to y=100)
            if (x >= flatPoints.x && x <= flatPoints.y && y >= flatPoints.x && y <= flatPoints.y)
            {
                // Set the height to a constant value (e.g., 0.5 for a flat surface)
                heights[x, y] = flatPointsHeight;
            }
            else
            {
                // Use Perlin noise to generate terrain heights outside the flat area
                heights[x, y] = Mathf.PerlinNoise((float)x / width * scale + offset.x, (float)y / height * scale + offset.y);
            }
        }
    }

    return heights;
    }
}
