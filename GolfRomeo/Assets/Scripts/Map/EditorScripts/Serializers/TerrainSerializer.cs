using System;
using System.IO;
using UnityEngine;

public class TerrainSerializer
{
    public const string trackHeightMapExtension = ".heightmap";
    public const string textureMapExtension = ".texturemap";
    private Track track;

    public TerrainSerializer(Track track)
    {
        this.track = track;
    }

    public void Serialize(string name)
    {
        using (FileStream file = new FileStream(name + trackHeightMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] baData = SerializeHeightMap(track.Terrain);
            file.Write(baData, 0, baData.Length);
        }

        using (FileStream file = new FileStream(name + textureMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] baData = SerializeTextureMap(track.Terrain);
            file.Write(baData, 0, baData.Length);
        }
    }

    public float[,] DeserializeHeightMap(string name, Vector2 heightMapSize)
    {
        var heightMap = new float[(int)heightMapSize.x, (int)heightMapSize.y];
        FromBytes(heightMap, File.ReadAllBytes(name + trackHeightMapExtension));
        return heightMap;
    }

    private byte[] SerializeHeightMap(Terrain terrain)
    {
        var heightMap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        return ToBytes(heightMap);
    }
    
    public float[,,] DeserializeTextureMap(string name, Vector3 textureMapSize)
    {
        var alphaMap = new float[(int)textureMapSize.x, (int)textureMapSize.y, (int)textureMapSize.z];
        FromBytes(alphaMap, File.ReadAllBytes(name + textureMapExtension));
        return alphaMap;
    }

    private byte[] SerializeTextureMap(Terrain terrain)
    {
        var alphaMap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        return ToBytes(alphaMap);
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

    private byte[] ToBytes<T>(T[,,] array) where T : struct
    {
        var buffer = new byte[array.GetLength(0) * array.GetLength(1) * array.GetLength(2) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
        Buffer.BlockCopy(array, 0, buffer, 0, buffer.Length);
        return buffer;
    }

    private void FromBytes<T>(T[,,] array, byte[] buffer) where T : struct
    {
        var len = Math.Min(array.GetLength(0) * array.GetLength(1) * array.GetLength(2) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer.Length);
        Buffer.BlockCopy(buffer, 0, array, 0, len);
    }
}
