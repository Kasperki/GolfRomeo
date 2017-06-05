using AutoMapper;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class TrackSerializer
{
    public const string mapFileExtension = ".xml";

    private Track track;
    private TrackFolderHelper directoryHelper;
    private TrackFileCompressor trackCompressor;
    private TerrainSerializer terrainSerializer;

    public TrackSerializer(Track track)
    {
        this.track = track;

        terrainSerializer = new TerrainSerializer();
        directoryHelper = new TrackFolderHelper();
        trackCompressor = new TrackFileCompressor();
    }
	
    public void SaveWorld(string name)
    {
        var trackPath = directoryHelper.GetTrackPath(name);

        var trackStreams = new TrackData();

        trackStreams.ObjectsData = SerializeMap(trackPath);
        trackStreams.HeightMapData = terrainSerializer.SerializeHeightMap(trackPath, track);
        trackStreams.TextureMapData = terrainSerializer.SerializeTextureMap(trackPath, track);

        trackCompressor.CreatePackage(trackPath, trackStreams);
    }

    public TrackDTO LoadWorld(string name)
    {
        var trackPath = directoryHelper.LoadTrackPath(name);

        XmlSerializer serializer = new XmlSerializer(typeof(TrackDTO));

        TrackFileCompressor trackCompressor = new TrackFileCompressor();
        TrackData decompressedTrackStreams = trackCompressor.DecompressPackage(trackPath);
        TrackDTO mapObject = (TrackDTO)serializer.Deserialize(new MemoryStream(decompressedTrackStreams.ObjectsData));

        track.Terrain.terrainData.SetHeights(0, 0, terrainSerializer.DeserializeHeightMap(decompressedTrackStreams.HeightMapData, track.HeightMapSize));
        track.Terrain.terrainData.SetAlphamaps(0, 0, terrainSerializer.DeserializeTextureMap(decompressedTrackStreams.TextureMapData, track.TextureMapSize));

        return mapObject;
    }

    private byte[] SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TrackDTO));

        TrackDTO mapDTO = Mapper.Map<Track, TrackDTO>(track);

        MapObjectsToDTO(mapDTO);
        CheckpointsToDTO(mapDTO);
        WaypointsToDTO(mapDTO);

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        var bytes = stream.GetBytes();
        stream.Close();

        using (FileStream file = new FileStream(name + mapFileExtension, FileMode.Create, FileAccess.Write))
        {
            file.Write(bytes, 0, bytes.Length);
        }

        return bytes;
    }

    private void MapObjectsToDTO(TrackDTO mapDTO)
    {
        mapDTO.MapObjects = new TrackObjectDTO[track.MapObjects.Length];
        for (int i = 0; i < track.MapObjects.Length; i++)
        {
            mapDTO.MapObjects[i] = Mapper.Map<TrackObject, TrackObjectDTO>(track.MapObjects[i]);
        }
    }

    private void CheckpointsToDTO(TrackDTO mapDTO)
    {
        mapDTO.Checkpoints = new CheckpointDTO[track.LapTracker.Checkpoints.Length];
        for (int i = 0; i < track.LapTracker.Checkpoints.Length; i++)
        {
            mapDTO.Checkpoints[i] = Mapper.Map<Checkpoint, CheckpointDTO>(track.LapTracker.Checkpoints[i]);
        }
    }

    private void WaypointsToDTO(TrackDTO mapDTO)
    {
        mapDTO.Waypoints = new WaypointDTO[track.WayPointCircuit.GetComponentsInChildren<WaypointNode>().Length];
        for (int i = 0; i < mapDTO.Waypoints.Length; i++)
        {
            mapDTO.Waypoints[i] = Mapper.Map<WaypointNode, WaypointDTO>(track.WayPointCircuit.GetComponentsInChildren<WaypointNode>()[i]);
        }
    }
}
