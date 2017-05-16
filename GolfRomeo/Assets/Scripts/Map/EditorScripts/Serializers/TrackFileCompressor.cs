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
    public void CreatePackage(string directoryPath)
    {
        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

        foreach (FileInfo fileToCompress in directorySelected.GetFiles())
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
        }
    }

    public TrackStreams DecompressPackage(string directoryPath)
    {
        TrackStreams deserializedTrackStreams = new TrackStreams();
        deserializedTrackStreams.TrackStream = DecompressFile(directoryPath, TrackSerializer.mapFileExtension);
        deserializedTrackStreams.HeightMapStream = DecompressFile(directoryPath, TerrainSerializer.trackHeightMapExtension);
        deserializedTrackStreams.TextureMapStream = DecompressFile(directoryPath, TerrainSerializer.textureMapExtension);

        return deserializedTrackStreams;
    }

    private MemoryStream DecompressFile(string directoryPath, string fileExtension)
    {
        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

        foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*" + fileExtension + ".gz"))
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
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

        return null;
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
