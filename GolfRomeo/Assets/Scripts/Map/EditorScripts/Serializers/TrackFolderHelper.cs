using System.IO;
using System.Xml;
using UnityEngine;

public class TrackFolderHelper
{
    private const string TracksRootFolder = "Maps"; 

    public string GetTrackFolder(string trackName)
    {
        return Path.Combine(TracksRootFolder, trackName);
    }

	public string GetTrackPath(string trackName)
    {
        if (!Directory.Exists(TracksRootFolder))
        {
            Directory.CreateDirectory(TracksRootFolder);
        }

        var trackFolder = GetTrackFolder(trackName);

        if (!Directory.Exists(trackFolder))
        {
            Directory.CreateDirectory(trackFolder);
        }

        return Path.Combine(trackFolder, trackName);
    }

    public string[] GetAllTracks()
    {
        var directories = Directory.GetDirectories(TracksRootFolder);
        string[] trackNames = new string[directories.Length];

        for (int i = 0; i < directories.Length; i++)
        {
            trackNames[i] = GetMapNameFromPath(directories[i]);
        }

        return trackNames;
    }

    private string GetMapNameFromPath(string path)
    {
        return path.Substring(TracksRootFolder.Length + 1);
    }

    public void RemoveTrack(string track)
    {
        Directory.Delete(TracksRootFolder + "/" + track, true);
    }

    public void CopyTrack(string track, string newName = "")
    {
        if (string.IsNullOrEmpty(newName)) 
        {
            newName = track + "_copy";
        }

        string destinationDirectory = TracksRootFolder + "/" + newName;

        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(TracksRootFolder + "/" + track);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + track);
        }

        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destinationDirectory, newName + file.Name.Substring(file.Name.IndexOf('.'))); //TODO: DO NOT ALLOW Extra '.' in map names. will break this naive solution
            file.CopyTo(temppath, false);
        }

        var trackXMLEditor = new TrackXMLDataEditor(newName);
        trackXMLEditor.ChangeTrackName(newName);
    }

    public void RenameTrack(string track, string newName)
    {
        CopyTrack(track, newName);
        RemoveTrack(track);
    }
}

