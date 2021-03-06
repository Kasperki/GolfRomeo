﻿using System;
using System.IO.Compression;
using System.IO;

public class TrackData
{
    public byte[] ObjectsData;
    public byte[] HeightMapData;
    public byte[] TextureMapData;
}

public class TrackFileCompressor
{
    private const string Gzip_Extension = ".gz";

    public void CreatePackage(string directoryPath, TrackData trackData)
    {
        CompressFile(directoryPath + TerrainSerializer.trackHeightMapExtension, trackData.HeightMapData);
        CompressFile(directoryPath + TerrainSerializer.textureMapExtension, trackData.TextureMapData);
    }

    private void CompressFile(string path, byte[] data)
    {
        MemoryStream stream = new MemoryStream(data);
        using (MemoryStream compressedMemStream = new MemoryStream())
        {
            using (GZipStream compressionStream = new GZipStream(compressedMemStream, CompressionMode.Compress, leaveOpen: true))
            {
                stream.CopyTo(compressionStream);
            }

            compressedMemStream.Position = 0;

            FileStream compressedFileStream = File.Create(path + Gzip_Extension);
            compressedMemStream.WriteTo(compressedFileStream);
            compressedFileStream.Close();
        }

        stream.Close();
    }

    public TrackData DecompressPackage(string directoryPath)
    {
        TrackData deserializedTrackStreams = new TrackData()
        {
            ObjectsData = File.ReadAllBytes(directoryPath + TrackSerializer.Track_File_Extension),
            HeightMapData = DecompressFile(directoryPath + TerrainSerializer.trackHeightMapExtension),
            TextureMapData = DecompressFile(directoryPath + TerrainSerializer.textureMapExtension)
        };

        return deserializedTrackStreams;
    }

    private byte[] DecompressFile(string directoryPath)
    {
        var fileInfo = new FileInfo(directoryPath + Gzip_Extension);

        using (FileStream originalFileStream = fileInfo.OpenRead())
        {
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                var decompressedMemoryStream = new MemoryStream();
                decompressionStream.CopyTo(decompressedMemoryStream);

                return decompressedMemoryStream.GetBytes();
            }
        }
    }
}
