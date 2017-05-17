using System;
using System.IO;
using UnityEngine;

public class TerrainSerializer
{
    public const string trackHeightMapExtension = ".heightmap";
    public const string textureMapExtension = ".texturemap";

    public void Serialize(string name, TrackStreams trackStreams, Track track)
    {
        using (FileStream file = new FileStream(name + trackHeightMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] heightMapByteArray = SerializeHeightMap(track);
            trackStreams.HeightMapStream = new MemoryStream(heightMapByteArray);
            file.Write(heightMapByteArray, 0, heightMapByteArray.Length);
        }

        using (FileStream file = new FileStream(name + textureMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] textureMapByteArray = SerializeTextureMap(track);
            trackStreams.TextureMapStream = new MemoryStream(textureMapByteArray);
            file.Write(textureMapByteArray, 0, textureMapByteArray.Length);
        }
    }

    public float[,] DeserializeHeightMap(MemoryStream stream, Vector2 heightMapSize)
    {
        var heightMap = new float[(int)heightMapSize.x, (int)heightMapSize.y];
        FromBytes(heightMap, stream.ToArray());

        stream.Close();
        return heightMap;
    }

    public byte[] SerializeHeightMap(Track track)
    {
        var heightMap = track.Terrain.terrainData.GetHeights(0, 0, (int)track.HeightMapSize.x, (int)track.HeightMapSize.y);
        return ToBytes(heightMap);
    }
    
    public float[,,] DeserializeTextureMap(MemoryStream stream, Vector3 textureMapSize)
    {
        var alphaMap = new float[(int)textureMapSize.x, (int)textureMapSize.y, (int)textureMapSize.z];
        FromBytes(alphaMap, stream.ToArray());

        stream.Close();
        return alphaMap;
    }

    public byte[] SerializeTextureMap(Track track)
    {
        var alphaMap = track.Terrain.terrainData.GetAlphamaps(0, 0, (int)track.TextureMapSize.x, (int)track.TextureMapSize.y);
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
