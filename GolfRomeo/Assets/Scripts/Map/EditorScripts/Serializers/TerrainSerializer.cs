using System;
using System.IO;
using UnityEngine;

public class TerrainSerializer
{
    public const string trackHeightMapExtension = ".heightmap";
    public const string textureMapExtension = ".texturemap";

    public float[,] DeserializeHeightMap(byte[] bytes, Vector2 heightMapSize)
    {
        var heightMap = new float[(int)heightMapSize.x, (int)heightMapSize.y];
        FromBytes(heightMap, bytes);
        return heightMap;
    }

    public byte[] SerializeHeightMap(string name, Track track)
    {
        var heightMap = track.Terrain.terrainData.GetHeights(0, 0, (int)track.HeightMapSize.x, (int)track.HeightMapSize.y);
        var bytes = ToBytes(heightMap);

#if UNITY_EDITOR
        using (FileStream file = new FileStream(name + trackHeightMapExtension, FileMode.Create, FileAccess.Write))
        {
            file.Write(bytes, 0, bytes.Length);
        }
#endif

        return bytes;
    }
    
    public float[,,] DeserializeTextureMap(byte[] bytes, Vector3 textureMapSize)
    {
        var alphaMap = new float[(int)textureMapSize.x, (int)textureMapSize.y, (int)textureMapSize.z];
        FromBytes(alphaMap, bytes);
        return alphaMap;
    }

    public byte[] SerializeTextureMap(string name, Track track)
    {
        var alphaMap = track.Terrain.terrainData.GetAlphamaps(0, 0, (int)track.TextureMapSize.x, (int)track.TextureMapSize.y);
        var bytes = ToBytes(alphaMap);

#if UNITY_EDITOR
        using (FileStream file = new FileStream(name + textureMapExtension, FileMode.Create, FileAccess.Write))
        {
            file.Write(bytes, 0, bytes.Length);
        }
#endif

        return bytes;
    }

    //Converting multidimensional array to byte array
    //https://stackoverflow.com/a/30484982

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
