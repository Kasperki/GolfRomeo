using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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

    public void CopyTrack(string track, string newName = "")
    {
        if (string.IsNullOrEmpty(newName)) 
        {
            newName = track + "_copy";
        }

        string destinationDirectory = TrackFolderName + "/" + newName;

        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(TrackFolderName + "/" + track);

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

        ChangeTrackName(newName, newName);
    }

    public void RenameTrack(string track, string newName)
    {
        CopyTrack(track, newName);
        RemoveTrack(track);
    }

    private void ChangeTrackName(string track, string newName)
    {
        var filePath = TrackFolderName + "/" + track + "/" + track + ".xml";
        XmlDocument xml = new XmlDocument();

        xml.Load(filePath);
        xml.SelectSingleNode("/Track/MapName").InnerText = newName;
        xml.Save(filePath);
    }
}

