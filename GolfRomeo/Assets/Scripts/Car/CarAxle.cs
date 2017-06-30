using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class CarAxle
{
    public List<AxleWheel> Wheels;

    public bool Motor;
    public bool Steering;
    public bool AddDownForce;

    [Range(0, 2)]
    public float TractionDefault = 1;
    [Range(0, 2)]
    public float TractionAsfalt = 1;
    [Range(0, 2)]
    public float TractionSandRoad = 1;
    [Range(0, 2)]
    public float TractionSand = 1;
    [Range(0, 2)]
    public float TractionGrass = 1;
    [Range(0, 2)]
    public float TractionIce = 1;

    public void Initialize()
    {
        foreach (var wheel in Wheels)
        {
            wheel.Initialize();
        }
    }

    public void UpdateTireHealth(float tireHealthPercentage)
    {
        foreach (var wheel in Wheels)
        {
            wheel.UpdateExtremumSlip(tireHealthPercentage);
        }
    }

    public void UpdateWheelTerrain()
    {
        foreach (var wheel in Wheels)
        {
            var wheelPosition = wheel.Wheel.transform.position;

            float[] mix = GetTextureMix(wheelPosition);

            float maxMix = 0;
            int terrainIndex = 0;

            for (int i = 0; i < mix.Length; i++)
            {
                if (mix[i] > maxMix)
                {
                    terrainIndex = i;
                    maxMix = mix[i];
                }
            }

            var terrainTexture = (TerrainTextures)terrainIndex;
            var terrainTextureName = Regex.Replace(terrainTexture.ToString(), @"[\d-]", string.Empty);
            wheel.WheelTerrain = (WheelTerrainType)Enum.Parse(typeof(WheelTerrainType), terrainTextureName, true);
        }
    }

    /// <summary>
    /// http://answers.unity3d.com/answers/457390/view.html
    /// </summary>
    /// <param name="worldPos">position</param>
    /// <returns>Array of terrain texture weights</returns>
    private float[] GetTextureMix(Vector3 worldPos)
    {
        var terrainPos = Track.Instance.Terrain.transform.position;
        var terrainData = Track.Instance.Terrain.terrainData;

        int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (int i = 0; i < cellMix.Length; i++)
        {
            cellMix[i] = splatmapData[0, 0, i];
        }

        return cellMix;
    }
}

[Serializable]
public class AxleWheel
{
    public WheelCollider Wheel;
    public WheelTerrainType WheelTerrain;

    private const float MaxForwardSlip = 2;
    private const float MaxSidewaysSlip = 2;

    private float extremumSlipForwardDefault = 0.4f;
    private float extremumSlipSidewaysDefault = 0.2f;

    public float ExtremumSlipForward
    {
        set
        {
            var wheelFrictionCurve = Wheel.forwardFriction;
            wheelFrictionCurve.extremumSlip = value;
            Wheel.forwardFriction = wheelFrictionCurve;
        }
        get
        {
            return Wheel.forwardFriction.extremumSlip;
        }
    }

    public float ExtremumSlipSideways
    {
        set
        {
            var wheelFrictionCurve = Wheel.sidewaysFriction;
            wheelFrictionCurve.extremumSlip = value;
            Wheel.sidewaysFriction = wheelFrictionCurve;
        }
        get
        {
            return Wheel.sidewaysFriction.extremumSlip;
        }
    }

    public void Initialize()
    {
        extremumSlipForwardDefault = Wheel.forwardFriction.extremumSlip;
        extremumSlipSidewaysDefault = Wheel.sidewaysFriction.extremumSlip;
    }

    public void Initialize(float extremumSlipForwardDefault, float extremumSlipSidewaysDefault)
    {
        ExtremumSlipForward = extremumSlipForwardDefault;
        ExtremumSlipSideways = extremumSlipSidewaysDefault;

        this.extremumSlipForwardDefault = extremumSlipForwardDefault;
        this.extremumSlipSidewaysDefault = extremumSlipSidewaysDefault;
    }

    public void UpdateExtremumSlip(float tireHealthPercentage)
    {
        ExtremumSlipForward = Mathf.Lerp(MaxForwardSlip, extremumSlipForwardDefault, tireHealthPercentage);
        ExtremumSlipSideways = Mathf.Lerp(MaxSidewaysSlip, extremumSlipSidewaysDefault, tireHealthPercentage);
    }
}