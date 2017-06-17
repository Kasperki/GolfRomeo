using System.IO;

public static class StreamExtensions
{
    /// <summary>
    /// https://stackoverflow.com/a/7423720
    /// </summary>
    /// <param name="source">Stream source</param>
    /// <param name="output">Stream output</param>
    /// <param name="bufferSize">bufferSize</param>
    public static void CopyTo(this Stream source, Stream output, int bufferSize = 81920)
    {
        byte[] array = new byte[bufferSize];
        int count;
        while ((count = source.Read(array, 0, array.Length)) != 0)
        {
            output.Write(array, 0, count);
        }
    }

    public static byte[] GetBytes(this MemoryStream source)
    {
        source.Position = 0;
        return source.ToArray();
    }
}
