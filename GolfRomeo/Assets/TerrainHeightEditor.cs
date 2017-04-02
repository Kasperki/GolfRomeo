using System.Collections;
using System.Collections.Generic;
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
    float desiredHeight = 0.001f;

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

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RaiseTerrainSmooth(desiredHeight);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            RaiseTerrainSmooth(-desiredHeight);
        }
    }

    private Vector2 GetTerrainPosition(Vector3 position)
    {
        var posXInTerrain = position.x / terrain.terrainData.size.x * heightmapWidth - offset;
        var posYInTerrain = position.z / terrain.terrainData.size.z * heightmapHeight - offset;

        return new Vector2(posXInTerrain, posYInTerrain);
    }

    private void RaiseTerrainSmooth(float height)
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

}
