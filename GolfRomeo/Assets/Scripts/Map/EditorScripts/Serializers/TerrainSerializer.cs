using System;
using System.IO;
using UnityEngine;

public class TerrainSerializer
{
    public const string mapHeightMapExtension = ".heightmap";
    private Map map;

    public TerrainSerializer(Map map)
    {
        this.map = map;
    }

    public void Serialize(string name)
    {
        using (FileStream file = new FileStream(name + mapHeightMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] baData = SerializeTerrain(map.Terrain);
            file.Write(baData, 0, baData.Length);
        }
    }

    public float[,] Deserialize(string name, Vector2 MapSize)
    {
        var heightMap = new float[(int)MapSize.x, (int)MapSize.y];
        FromBytes(heightMap, File.ReadAllBytes(name + mapHeightMapExtension));
        return heightMap;
    }

    private byte[] SerializeTerrain(Terrain terrain)
    {
        var heightMap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        return ToBytes(heightMap);
    }

    private byte[] ToBytes<T>(T[,] array) where T : struct
    {
        var buffer = new byte[array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
        Buffer.BlockCopy(array, 0, buffer, 0, buffer.Length);
        return buffer;
    }

    private void FromBytes<T>(T[,] array, byte[] buffer) where T : struct
    {
        var len = Math.Min(array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer.Length);
        Buffer.BlockCopy(buffer, 0, array, 0, len);
    }
}
