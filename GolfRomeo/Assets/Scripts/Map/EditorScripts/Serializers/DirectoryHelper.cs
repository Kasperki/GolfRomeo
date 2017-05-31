using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DirectoryHelper
{
    private const string MapFolderName = "Maps"; 

    public string MapRootFolder
    {
        get
        {
            return MapFolderName;
        }
    }

    public string GetPathToDirectory(string trackName)
    {
        return MapFolderName + "/" + trackName;
    }


    public string[] GetAllTracks()
    {
        var directories = Directory.GetDirectories(MapRootFolder);
        string[] trackNames = new string[directories.Length];

        for (int i = 0; i < directories.Length; i++)
        {
            trackNames[i] = GetMapNameFromPath(directories[i]);
        }

        return trackNames;
    } 

    public string GetMapNameFromPath(string path)
    {
        return path.Substring(MapRootFolder.Length + 1);
    }

	public string GetPathToMap(string trackName)
    {
        if (!Directory.Exists(MapFolderName))
        {
            Directory.CreateDirectory(MapFolderName);
        }

        if (!Directory.Exists(MapFolderName + "/" + trackName))
        {
            Directory.CreateDirectory(MapFolderName + "/" + trackName);
        }

        return MapFolderName + "/" + trackName + "/" + trackName;
    }

    public string LoadTrackPath(string trackName)
    {
        if (!Directory.Exists(MapFolderName))
        {
            Directory.CreateDirectory(MapFolderName);
        }

        if (!Directory.Exists(MapFolderName + "/" + trackName))
        {
            throw new System.Exception("Track does not exists");
        }

        return MapFolderName + "/" + trackName + "/" + trackName;
    }

    public void RemoveTrack(string track)
    {
        Directory.Delete(MapFolderName + "/" + track, true);
    }
}

