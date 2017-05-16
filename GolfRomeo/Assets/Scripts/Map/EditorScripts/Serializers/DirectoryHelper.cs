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

