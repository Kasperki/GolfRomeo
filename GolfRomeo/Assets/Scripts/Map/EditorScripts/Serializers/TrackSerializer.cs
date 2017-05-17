using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

//TODO CREATE NOT COMPRESSED FILES FOR ONLY DEBUG!
public class TrackSerializer
{
    public const string mapFileExtension = ".xml";

    private Track track;
    private DirectoryHelper directoryHelper;
    private TrackFileCompressor trackCompressor;
    private TerrainSerializer terrainSerializer;

    public TrackSerializer(Track track)
    {
        this.track = track;

        terrainSerializer = new TerrainSerializer();
        directoryHelper = new DirectoryHelper();
        trackCompressor = new TrackFileCompressor();
    }
	
    public void SaveWorld(string name)
    {
        var trackPath = directoryHelper.GetPathToMap(name);

        var trackStreams = new TrackStreams();

        var byteArray = SerializeMap(trackPath);
        trackStreams.TrackStream = new MemoryStream(byteArray);

        using (FileStream file = new FileStream(trackPath + mapFileExtension, FileMode.Create, FileAccess.Write))
        {
            file.Write(byteArray, 0, byteArray.Length);
        }

        terrainSerializer.Serialize(trackPath, trackStreams, track);
        trackCompressor.CreatePackage(trackPath, trackStreams);
    }

    public TrackDTO LoadWorld(string name)
    {
        var trackPath = directoryHelper.LoadTrackPath(name);

        XmlSerializer serializer = new XmlSerializer(typeof(TrackDTO));

        TrackFileCompressor trackCompressor = new TrackFileCompressor();
        TrackStreams decompressedTrackStreams = trackCompressor.DecompressPackage(trackPath);
        TrackDTO mapObject = (TrackDTO)serializer.Deserialize(decompressedTrackStreams.TrackStream);

        track.Terrain.terrainData.SetHeights(0, 0, terrainSerializer.DeserializeHeightMap(decompressedTrackStreams.HeightMapStream, track.HeightMapSize));
        track.Terrain.terrainData.SetAlphamaps(0, 0, terrainSerializer.DeserializeTextureMap(decompressedTrackStreams.TextureMapStream, track.TextureMapSize));

        return mapObject;
    }

    private byte[] SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TrackDTO));

        TrackDTO mapDTO = new TrackDTO().MapToDTO(track);

        MapObjectsToDTO(mapDTO);
        CheckpointsToDTO(mapDTO);
        WaypointsToDTO(mapDTO);

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        var bytes = stream.ToArray();
        stream.Close();

        return bytes;
    }

    private void MapObjectsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < track.MapObjects.Length; i++)
        {
            mapDTO.MapObjects[i] = new TrackObjectDTO().MapToDTO(track.MapObjects[i]);
        }
    }

    private void CheckpointsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < track.LapTracker.Checkpoints.Length; i++)
        {
            mapDTO.Checkpoints[i] = new CheckpointDTO().MapToDTO(track.LapTracker.Checkpoints[i]);
        }
    }

    private void WaypointsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < mapDTO.Waypoints.Length; i++)
        {
            mapDTO.Waypoints[i] = new WaypointDTO().MapToDTO(track.WayPointCircuit.GetComponentsInChildren<WaypointNode>()[i]);
        }
    }
}
