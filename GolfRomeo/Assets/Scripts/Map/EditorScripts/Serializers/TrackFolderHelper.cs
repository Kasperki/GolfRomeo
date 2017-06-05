using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrackFolderHelper
{
    private const string TrackFolderName = "Maps"; 

    public string TrackRootFolder
    {
        get
        {
            return TrackFolderName;
        }
    }

    public string[] GetAllTracks()
    {
        var directories = Directory.GetDirectories(TrackRootFolder);
        string[] trackNames = new string[directories.Length];

        for (int i = 0; i < directories.Length; i++)
        {
            trackNames[i] = GetMapNameFromPath(directories[i]);
        }

        return trackNames;
    } 

    public string GetMapNameFromPath(string path)
    {
        return path.Substring(TrackRootFolder.Length + 1);
    }

	public string GetTrackPath(string trackName)
    {
        if (!Directory.Exists(TrackFolderName))
        {
            Directory.CreateDirectory(TrackFolderName);
        }

        if (!Directory.Exists(TrackFolderName + "/" + trackName))
        {
            Directory.CreateDirectory(TrackFolderName + "/" + trackName);
        }

        return TrackFolderName + "/" + trackName + "/" + trackName;
    }

    public string LoadTrackPath(string trackName)
    {
        if (!Directory.Exists(TrackFolderName))
        {
            Directory.CreateDirectory(TrackFolderName);
        }

        if (!Directory.Exists(TrackFolderName + "/" + trackName))
        {
            throw new System.Exception("Track does not exists");
        }

        return TrackFolderName + "/" + trackName + "/" + trackName;
    }

    public void RemoveTrack(string track)
    {
        Directory.Delete(TrackFolderName + "/" + track, true);
    }

    public void CopyTrack(string track)
    {
        string destinationDirectory = track + "_copy";

        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(TrackFolderName + "/" + track);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + track);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destinationDirectory, file.Name);
            file.CopyTo(temppath, false);
        }
    }

    public void RenameTrack(string track)
    {
        throw new System.NotImplementedException();
    }
}

