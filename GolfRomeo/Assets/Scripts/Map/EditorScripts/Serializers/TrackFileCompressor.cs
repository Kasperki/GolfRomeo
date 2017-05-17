using System;
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
    public void CreatePackage(string directoryPath, TrackData trackData)
    {
        CompressFile(directoryPath + TrackSerializer.mapFileExtension, trackData.ObjectsData);
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

            compressedMemStream.Seek(0, SeekOrigin.Begin);

            FileStream compressedFileStream = File.Create(path + ".gz");
            compressedMemStream.WriteTo(compressedFileStream);
            compressedFileStream.Close();
        }

        stream.Close();
    }

    public TrackData DecompressPackage(string directoryPath)
    {
        TrackData deserializedTrackStreams = new TrackData();
        deserializedTrackStreams.ObjectsData = DecompressFile(directoryPath + TrackSerializer.mapFileExtension);
        deserializedTrackStreams.HeightMapData = DecompressFile(directoryPath + TerrainSerializer.trackHeightMapExtension);
        deserializedTrackStreams.TextureMapData = DecompressFile(directoryPath + TerrainSerializer.textureMapExtension);

        return deserializedTrackStreams;
    }

    private byte[] DecompressFile(string directoryPath)
    {
        var fileInfo = new FileInfo(directoryPath + ".gz");

        using (FileStream originalFileStream = fileInfo.OpenRead())
        {
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                var decompressedMemoryStream = new MemoryStream();
                decompressionStream.CopyTo(decompressedMemoryStream);
                decompressedMemoryStream.Position = 0;

                return decompressedMemoryStream.ToArray();
            }
        }
    }
}

public static class StreamExtension
{
    public static void CopyTo(this Stream source, Stream output, int bufferSize = 81920)
    {
        byte[] array = new byte[bufferSize];
        int count;
        while ((count = source.Read(array, 0, array.Length)) != 0)
        {
            output.Write(array, 0, count);
        }
    }
}
