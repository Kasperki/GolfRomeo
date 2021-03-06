﻿using AutoMapper;
using System;
using UnityEngine;

public class TrackLoader
{
    private Track track;

    public TrackLoader(Track track)
    {
        this.track = track;
    }

    public void NewTrack(string name)
    {
        track.Metadata = new TrackMetadata(name);

        var editorTools = track.gameObject.AddComponent<TerrainEditorTools>();
        editorTools.NewEmptyTerrain();
        UnityEngine.GameObject.Destroy(editorTools);

        track.TrackObjectsParent.transform.DestroyChildren();
        track.WayPointCircuit.transform.DestroyChildren();
        track.LapTracker.transform.DestroyChildren();

        Track.Instance.SkidMarks.Init();
    }

    public void SaveTrack()
    {
        var WorldSerialization = new TrackSerializer(track);
        WorldSerialization.SaveWorld(track.Metadata.Name);
    }

    public void LoadTrack(string trackName)
    {
        var trackSerializer = new TrackSerializer(track);
        var mapDTO = trackSerializer.LoadWorld(trackName);
        track.SkidMarks.Init();

        //MAP METADATA
        Mapper.Map(mapDTO, track.Metadata);

        //Init track objects
        InstantiateMapObjects(mapDTO);

        //Init checkpoints
        InstantiateCheckpoints(mapDTO);

        //Init waypoints
        InstantiateWaypoints(mapDTO);
    }

    private void InstantiateMapObjects(TrackDTO mapDTO)
    {
        track.TrackObjectsParent.transform.DestroyChildren();

        foreach (var mapObjectDTO in mapDTO.MapObjects)
        {
            GameObject gameObj = ResourcesLoader.LoadTrackObject(mapObjectDTO.ID);
            gameObj.transform.SetParent(track.TrackObjectsParent.transform);

            Mapper.Map(mapObjectDTO, gameObj.GetComponent<TrackObject>());
        }
    }

    private void InstantiateCheckpoints(TrackDTO mapDTO)
    {
        track.LapTracker.transform.DestroyChildren();

        foreach (var checkpointDTO in mapDTO.Checkpoints)
        {
            GameObject gameObj = ResourcesLoader.LoadRoadObject("Checkpoint");
            gameObj.transform.SetParent(track.LapTracker.transform);

            Mapper.Map(checkpointDTO, gameObj.GetComponent<Checkpoint>());
            gameObj.GetComponent<Checkpoint>().SetOrder(gameObj.GetComponent<Checkpoint>().CheckpointOrder);
        }
    }

    private void InstantiateWaypoints(TrackDTO mapDTO)
    {
        track.WayPointCircuit.transform.DestroyChildren();

        foreach (var waypointDTO in mapDTO.Waypoints)
        {
            GameObject gameObj = ResourcesLoader.LoadRoadObject("WaypointNode");
            gameObj.transform.SetParent(track.WayPointCircuit.transform);

            Mapper.Map(waypointDTO, gameObj.GetComponent<WaypointNode>());
        }

        if (mapDTO.Waypoints.Length > 0)
        {
            track.WayPointCircuit.CachePositionsAndDistances();
        }
    }
}
