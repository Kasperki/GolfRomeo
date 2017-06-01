using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DirectoryHelper
{
    private const string TrackFolderName = "Maps"; 

    public string TrackRootFolder
    {
        get
        {
            return TrackFolderName;
        }
    }

    public string GetPathToDirectory(string trackName)
    {
        return TrackFolderName + "/" + trackName;
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

	public string GetPathToMap(string trackName)
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
}

