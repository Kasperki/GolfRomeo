using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainEditorTools : MonoBehaviour
{
    public Terrain terrain;
    private TerrainToolkit terrainToolkit;

    private const float MaxHeight = 0.020f;
    private const float MinHeight = 0;
    private const float BaseHeight = 0.01f;

    private const float BezierStep = 200;

    int heightmapWidth;
    int heightmapHeight;

    void Awake ()
    {
        terrain = Track.Instance.Terrain;
        terrainToolkit = new TerrainToolkit();

        heightmapWidth = terrain.terrainData.heightmapWidth;
        heightmapHeight = terrain.terrainData.heightmapHeight;
    }
	
    public void NewEmptyTerrain()
    {
        float[,] heigthmapSize = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (int x = 0; x < heigthmapSize.GetLength(0); x++)
        {
            for (int y = 0; y < heigthmapSize.GetLength(1); y++)
            {
                heigthmapSize[x, y] = BaseHeight;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heigthmapSize);

        float[,,] alphaMap = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, terrain.terrainData.alphamapLayers];
        for (int x = 0; x < alphaMap.GetLength(0); x++)
        {
            for (int y = 0; y < alphaMap.GetLength(1); y++)
            {
                for (int i = 0; i < alphaMap.GetLength(2); i++)
                {
                    if (i == 0)
                    {
                        alphaMap[x, y, i] = 1;
                    }
                    else
                    {
                        alphaMap[x, y, i] = 0;
                    }
                }
            }
        }

        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private Vector4 GetTerrainPosition(Vector3 position, int size, out int offset)
    {
        offset = size / 2;
        var posXInTerrain = position.x / terrain.terrainData.size.x * heightmapWidth - offset;
        var posYInTerrain = position.z / terrain.terrainData.size.z * heightmapHeight - offset;

        int width = posXInTerrain + size > heightmapWidth ? (int)(heightmapWidth - posXInTerrain) : size;

        if (posXInTerrain < 0)
        {
            offset += (int)posXInTerrain;
            posXInTerrain = 0;
        }

        int height = posYInTerrain + size > heightmapHeight ? (int)(heightmapHeight - posYInTerrain) : size;

        if (posYInTerrain < 0)
        {
            offset += (int)posYInTerrain;
            posYInTerrain = 0;
        }

        return new Vector4(posXInTerrain, posYInTerrain, width, height);
    }

    public Vector3[] CoordinatesToTerrain(Vector3[] pos)
    {
        float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        for (int i = 0; i < pos.Length; i++)
        {
            int offset = 0;
            Vector4 terrainPosition = GetTerrainPosition(pos[i] - terrain.transform.position, 1, out offset);

            int x = (int)terrainPosition.x;
            int y = (int)terrainPosition.y;
            pos[i] = new Vector3(pos[i].x, pos[i].y, heights[y, x] * 1200); //1200
        }

        return pos;
    }

    public void UpdateTerrainTexture(int textureID, int size)
    {
        UpdateTerrainTexture(transform.position, textureID, size);
    }

    public void UpdateTerrainTexture(Vector3 pos, int textureID, int size)
    {
        int offset = 0;
        Vector4 terrainPosition = GetTerrainPosition(pos - terrain.transform.position, size, out offset);

        float[,,] alphas = terrain.terrainData.GetAlphamaps((int)terrainPosition.x, (int)terrainPosition.y, (int)terrainPosition.z, (int)terrainPosition.w);

        int xLength = alphas.GetLength(0);
        int yLength = alphas.GetLength(1);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                var distance = (new Vector2(x, y) - new Vector2(offset, offset)).magnitude / offset;
                if (distance < 1)
                {
                    alphas[x, y, textureID] = 1;

                    for (int i = 0; i < terrain.terrainData.alphamapLayers; i++)
                    {
                        if (i != textureID)
                        {
                            alphas[x, y, i] = 0;
                        }
                    }
                }

                /*var x1 = Input.GetAxis("Horizontal");
                var y1 = Input.GetAxis("Vertical");

                var dotProduct = Vector2.Dot(new Vector2(x1, y1), new Vector2(y, x) - new Vector2(offset, offset));

                if (dotProduct >= 0 && Mathf.Abs(x1 + y1) == 1)
                {
                    float mindis = 0.90f;
                    float maxdis = 1.2f;

                    if (distance > mindis && distance < maxdis)
                    {
                        alphas[x, y, 2] = 1; //Mathf.Lerp(0, 1, (maxdis - distance) / (maxdis - mindis));

                        if (x + 1 < alphas.GetLength(0) && y + 1 < alphas.GetLength(1) && x - 1 > 0 && y - 1 > 0)
                        {
                            alphas[x - 1, y - 1, 2] = 0.5f;
                            alphas[x - 1, y + 1, 2] = 0.5f;
                            alphas[x - 1, y, 2] = 0.5f;
                            alphas[x, y - 1, 2] = 0.5f;
                            alphas[x, y + 1, 2] = 0.5f;
                            alphas[x + 1, y - 1, 2] = 0.5f;
                            alphas[x + 1, y + 1, 2] = 0.5f;
                            alphas[x + 1, y, 2] = 0.5f;
                        }
                    }
                }*/
            }
        }

        terrain.terrainData.SetAlphamaps((int)terrainPosition.x, (int)terrainPosition.y, alphas);
    }

    public void UpdateTerrainTextureOnBezierCurvePath(int textureID, BezierCurve bzCurve, int size)
    {
        for (float t = 0; t < 1.0f; t += 1.0f / BezierStep)
        {
            var bzVector = bzCurve.GetPositionAt(t);
            UpdateTerrainTexture(new Vector3(bzVector.x, 0, bzVector.y), textureID, size);
        }
    }

    public void RaiseTerrain(float raiseAmount, int size)
    {
        int offset = 0;
        Vector4 terrainPosition = GetTerrainPosition(transform.position - terrain.transform.position, size, out offset);
        float[,] heights = terrain.terrainData.GetHeights((int)terrainPosition.x, (int)terrainPosition.y, (int)terrainPosition.z, (int)terrainPosition.w);

        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                float heightChange = heights[x, y] + raiseAmount;
                heightChange = Mathf.Clamp(heightChange, MinHeight, MaxHeight);

                heights[x, y] = heightChange;
            }
        }

        terrain.terrainData.SetHeights((int)terrainPosition.x, (int)terrainPosition.y, heights);
    }

    public void RaiseTerrainSmooth(float raiseAmount, int size)
    {
        int offset = 0;
        Vector4 terrainPosition = GetTerrainPosition(transform.position - terrain.transform.position, size, out offset);
        float[,] heights = terrain.terrainData.GetHeights((int)terrainPosition.x, (int)terrainPosition.y, (int)terrainPosition.z, (int)terrainPosition.w);

        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                var distance = (new Vector2(x, y) - new Vector2(offset, offset)).magnitude / offset;

                float heightChange = heights[x, y] + raiseAmount * Mathf.Lerp(1, 0, distance);
                heightChange = Mathf.Clamp(heightChange, MinHeight, MaxHeight);

                heights[x, y] = heightChange;
            }
        }

        terrain.terrainData.SetHeights((int)terrainPosition.x, (int)terrainPosition.y, heights);
    }

    public void SmoothTerrain(int size)
    {
        SmoothTerrain(transform.position, size);
    }

    public void SmoothTerrain(Vector3 pos, int size)
    {
        int offset = 0;
        Vector4 terrainPosition = GetTerrainPosition(pos - terrain.transform.position, size, out offset);
        float[,] heights = terrain.terrainData.GetHeights((int)terrainPosition.x, (int)terrainPosition.y, (int)terrainPosition.w, (int)terrainPosition.z);

        heights = terrainToolkit.Smooth(heights, (int)size);
        terrain.terrainData.SetHeights((int)terrainPosition.x, (int)terrainPosition.y, heights);
    }
}
