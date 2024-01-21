using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainManager : MonoBehaviour
{
    public Vector2 offset;
    public bool useCustomOffset;
    public GameObject treeParent;
    public bool regenFlatArea;
    public ParticleSystem ps;

    //Flat Area
    public float flatPointsHeight;
    public Vector2 circleCenter = new Vector2(125, 125);
    public float circleRadius = 25f;
    public float blendWidth = 10f;

    // Set the dimensions of the terrain
    public int width = 256;
    public int height = 256;
    
    // Set the scale 
    public float scale = 20f;

    void AddTree(Vector3 treeSpot)
    {
        float ran = Random.Range(0, 300);
        if(ran <= 1)
        {
            Terrain terrain = GetComponent<Terrain>();
            TreeInstance tree = new TreeInstance();
            tree.position = new Vector3(treeSpot.x / width, 0, treeSpot.z / height);
            tree.prototypeIndex = Random.Range(0, 8);
            tree.widthScale = 1f;
            tree.heightScale = 1f;
            tree.color = Color.white;
            tree.lightmapColor = Color.white;
            terrain.AddTreeInstance(tree);
            terrain.Flush();
        }
    }

    public void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();

        // reset trees
        foreach (Transform child in treeParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate the terrain
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Convert Trees to actual trees
        foreach (TreeInstance treeInstance in terrain.terrainData.treeInstances)
        {
            // Get the position in world space
            Vector3 position = Vector3.Scale(treeInstance.position, terrain.terrainData.size) + terrain.transform.position;

            // Get the original tree prototype index
            int treePrototypeIndex = treeInstance.prototypeIndex;
            
            GameObject treePrefab = terrain.terrainData.treePrototypes[treePrototypeIndex].prefab;
            
            // Actual Tree
            GameObject treeGameObject = Instantiate(treePrefab, position, Quaternion.identity);

            treeGameObject.transform.SetParent(treeParent.transform);
            treeGameObject.transform.localScale = new Vector3(2,2,2);

            terrain.terrainData.treeInstances = new TreeInstance[0];
        }

        ps.Play();
        /*
        // make trees collide
        terrain.GetComponent<Collider>().enabled = false;
        terrain.GetComponent<Collider>().enabled = true;*/
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the resolution and size
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
                // Calculate the distance from the current point to the center of the circle
                float distanceToCenter = Vector2.Distance(new Vector2(x, y), circleCenter);

                // Define the flat area as a circle
                if (distanceToCenter <= circleRadius)
                {
                    if (regenFlatArea)
                    {
                        // Calculate a blend factor based on the distance to create a smooth transition
                        float blendFactor = Mathf.Clamp01((distanceToCenter - (circleRadius - blendWidth)) / blendWidth);

                        // Set the height to a constant value with a smooth blend
                        heights[x, y] = Mathf.Lerp(flatPointsHeight,  Mathf.PerlinNoise((float)x / width * scale + offset.x, (float)y / height * scale + offset.y), blendFactor);
                    }
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
