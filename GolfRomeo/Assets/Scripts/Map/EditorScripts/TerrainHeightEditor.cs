using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainHeightEditor : MonoBehaviour
{
    public Terrain terrain;
    public bool InitOnStart;

    private const float MaxHeight = 0.025f;
    private const float MinHeight = 0;
    private const int size = 45;
    private const int offset = size / 2;
    private const float BaseHeight = 0.01f;

    int heightmapWidth;
    int heightmapHeight;

    void Start ()
    {
        heightmapWidth = terrain.terrainData.heightmapWidth;
        heightmapHeight = terrain.terrainData.heightmapHeight;

        if (InitOnStart)
        {
            InitTerrain(BaseHeight);
        }
    }
	
    //TODO MOVE ELSEWHERE!
    void InitTerrain(float height)
    {
        float[,] heigthmapSize = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        for (int x = 0; x < heigthmapSize.GetLength(0); x++)
        {
            for (int y = 0; y < heigthmapSize.GetLength(1); y++)
            {
                heigthmapSize[x, y] = height;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heigthmapSize);
    }

    private Vector2 GetTerrainPosition(Vector3 position)
    {
        var posXInTerrain = position.x / terrain.terrainData.size.x * heightmapWidth - offset;
        var posYInTerrain = position.z / terrain.terrainData.size.z * heightmapHeight - offset;

        return new Vector2(posXInTerrain, posYInTerrain);
    }

    public void RaiseTerrainSmooth(float height)
    {
        Vector2 terrainPosition = GetTerrainPosition(transform.position - terrain.gameObject.transform.position);
        float[,] heights = terrain.terrainData.GetHeights((int)terrainPosition.x, (int)terrainPosition.y, size, size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var distance = (new Vector2(i, j) - new Vector2(offset, offset)).magnitude / offset;

                float heightChange = heights[i, j] + height * Mathf.Lerp(1, 0, distance);
                heightChange = Mathf.Max(MinHeight, heightChange);
                heightChange = Mathf.Min(MaxHeight, heightChange);

                heights[i, j] = heightChange;
            }
        }

        terrain.terrainData.SetHeights((int)terrainPosition.x, (int)terrainPosition.y, heights);
    }

    public void SmoothTerrain()
    {
        Vector2 terrainPosition = GetTerrainPosition(transform.position - terrain.gameObject.transform.position);
        float[,] heights = terrain.terrainData.GetHeights((int)terrainPosition.x, (int)terrainPosition.y, size, size);

        heights = Smooth(heights);
        terrain.terrainData.SetHeights((int)terrainPosition.x, (int)terrainPosition.y, heights);
    }

    private float[,] Smooth(float[,] heightMap)
    {
        int Tw = size;
        int Th = size;
        int xNeighbours;
        int yNeighbours;
        int xShift;
        int yShift;
        int xIndex;
        int yIndex;
        int Tx;
        int Ty;

        // Start iterations...
        for (int iter = 0; iter < 5; iter++)
        {
            for (Ty = 0; Ty < Th; Ty++)
            {
                // y...
                if (Ty == 0)
                {
                    yNeighbours = 2;
                    yShift = 0;
                    yIndex = 0;
                }
                else if (Ty == Th - 1)
                {
                    yNeighbours = 2;
                    yShift = -1;
                    yIndex = 1;
                }
                else
                {
                    yNeighbours = 3;
                    yShift = -1;
                    yIndex = 1;
                }
                for (Tx = 0; Tx < Tw; Tx++)
                {
                    // x...
                    if (Tx == 0)
                    {
                        xNeighbours = 2;
                        xShift = 0;
                        xIndex = 0;
                    }
                    else if (Tx == Tw - 1)
                    {
                        xNeighbours = 2;
                        xShift = -1;
                        xIndex = 1;
                    }
                    else
                    {
                        xNeighbours = 3;
                        xShift = -1;
                        xIndex = 1;
                    }
                    int Ny;
                    int Nx;
                    float hCumulative = 0.0f;
                    int nNeighbours = 0;
                    for (Ny = 0; Ny < yNeighbours; Ny++)
                    {
                        for (Nx = 0; Nx < xNeighbours; Nx++)
                        {
                            float heightAtPoint = heightMap[Tx + Nx + xShift, Ty + Ny + yShift]; // Get height at point
                            hCumulative += heightAtPoint;
                            nNeighbours++;
                        }
                    }
                    float hAverage = hCumulative / nNeighbours;
                    heightMap[Tx + xIndex + xShift, Ty + yIndex + yShift] = hAverage;
                }
            }
            // Show progress...
            float percentComplete = (iter + 1) / 5;
        }
        return heightMap;
    }

}
