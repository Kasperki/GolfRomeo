using System;
using System.IO.Compression;
using System.IO;

public class TrackStreams
{
    public MemoryStream TrackStream;
    public MemoryStream HeightMapStream;
    public MemoryStream TextureMapStream;
}

public class TrackFileCompressor
{
    public void CreatePackage(string directoryPath, TrackStreams streams)
    {
        CompressFile(directoryPath + TrackSerializer.mapFileExtension, streams.TrackStream);
        CompressFile(directoryPath + TerrainSerializer.trackHeightMapExtension, streams.HeightMapStream);
        CompressFile(directoryPath + TerrainSerializer.textureMapExtension, streams.TextureMapStream);
    }

    private void CompressFile(string path, MemoryStream stream)
    {
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

    public TrackStreams DecompressPackage(string directoryPath)
    {
        TrackStreams deserializedTrackStreams = new TrackStreams();
        deserializedTrackStreams.TrackStream = DecompressFile(directoryPath + TrackSerializer.mapFileExtension);
        deserializedTrackStreams.HeightMapStream = DecompressFile(directoryPath + TerrainSerializer.trackHeightMapExtension);
        deserializedTrackStreams.TextureMapStream = DecompressFile(directoryPath + TerrainSerializer.textureMapExtension);

        return deserializedTrackStreams;
    }

    private MemoryStream DecompressFile(string directoryPath)
    {
        var fileInfo = new FileInfo(directoryPath + ".gz");

        using (FileStream originalFileStream = fileInfo.OpenRead())
        {
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                var decompressedMemoryStream = new MemoryStream();
                decompressionStream.CopyTo(decompressedMemoryStream);
                decompressedMemoryStream.Position = 0;

                return decompressedMemoryStream;
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
