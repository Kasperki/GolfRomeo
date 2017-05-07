﻿using UnityEngine;

[RequireComponent(typeof(TerrainEditor))]
public class CursorEditor : MonoBehaviour
{
    public EditMode EditMode;

    public Renderer Renderer;
    private Material cursorMaterial;
    public Color Normal;
    public Color Hover;
    public Color Selected;

    public bool Mouse;

    private GameObject lastHoveredObject;
    private Vector3 raycastOrigin, raycastDirection;
    private Vector3 raycastPos = new Vector3(-30,10,10);
    private float raycastLength;

    private bool selected;
    private GameObject selectedIEditable;

    public GameObject ObjectPrefab, CheckpointPrefab, WaypointPrefab;
    public TerrainEditor terrainEditor;

    private CursorUI cursorUI;

    private void Awake()
    {
        cursorMaterial = Renderer.material;
        terrainEditor = GetComponent<TerrainEditor>();
        cursorUI = GetComponentInChildren<CursorUI>();
    }
    
    private RaycastHit RaycastAgainstTerrain(int cursorHitLayer)
    {
        if (Mouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            raycastOrigin = ray.origin;
            raycastDirection = ray.direction;
            raycastLength = 50;
        }
        else
        {
            if (cursorUI.EditorObject.activeSelf == false)
            {
                raycastPos += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 6;
            }

            raycastOrigin = raycastPos;
            raycastDirection = -Vector3.up;
            raycastLength = 20;
        }

        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, cursorHitLayer, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastLength);

        return hit;
    }

    void Update ()
    {
        cursorMaterial.color = Normal;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            cursorUI.Init();
        }

        int cursorHitLayer = 1 << Track.TerrainMask;
        var hit = RaycastAgainstTerrain(cursorHitLayer);
        terrainEditor.BrushRenderer.enabled = false;

        //TODO CLAMP
        if (hit.collider != null)
        {
            raycastPos = new Vector3(hit.point.x, 10, hit.point.z);
            transform.position = hit.point;
            transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
            transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);
        }

        switch (EditMode)
        {
            case EditMode.TerrainHeightMap:
                terrainEditor.UpdateTerrainHeightMap();
                break;
            case EditMode.TerrainTexture:
                terrainEditor.UpdateTerrainTexture();
                break;
            case EditMode.Objects:
                MoveObjects(Track.TrackObjectsMask);
                break;
            case EditMode.AIWaypoints:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    InstantiateObject(WaypointPrefab, Track.Instance.WayPointCircuit.transform);
                    FindObjectOfType<WayPointCircuit>().CachePositionsAndDistances();
                }

                MoveObjects(Track.AIWaypointsMask);
                break;
            case EditMode.Checkpoints:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    var obj = InstantiateObject(CheckpointPrefab, Track.Instance.LapTracker.transform);
                    obj.GetComponent<Checkpoint>().SetOrder();
                }

                MoveObjects(Track.CheckpointsMask);

                break;
            default:
                break;
        }
    }

    public void CreateNewObject(GameObject prefab)
    {
        InstantiateObject(prefab, Track.Instance.ObjectsParent.transform);
    }

    private GameObject InstantiateObject(GameObject obj, Transform parent)
    {
        var gameObj = Instantiate(obj);
        gameObj.transform.position = transform.position;
        gameObj.transform.SetParent(parent);

        var iEditable = gameObj.GetComponent(typeof(IEditable)) as IEditable;

        selectedIEditable = gameObj;
        iEditable.OnSelect(true, transform);

        return gameObj;
    }

    private GameObject DuplicateObject(GameObject obj)
    {
        string name = obj.GetComponent<TrackObject>().ID;
        return InstantiateObject(Resources.Load("Objects/" + name) as GameObject, obj.transform); //TODO LOAD FROM FOLDER, MAKE THESE TO OWN CLASS
    }

    private void MoveObjects(int layer)
    {
        selected = false;

        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << layer, QueryTriggerInteraction.Collide);

        if (hit.collider != null && hit.collider.gameObject != null)
        {
            //BLUR
            if (lastHoveredObject != null && lastHoveredObject != hit.collider.gameObject)
            {
                lastHoveredObject.GetComponent<IEditable>().OnBlur();
            }

            //SELECT
            var iEditable = hit.collider.gameObject.GetComponent(typeof(IEditable)) as IEditable;

            if (iEditable != null)
            {
                cursorMaterial.color = Hover;

                if (selectedIEditable == null)
                {
                    iEditable.OnHover();

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        selectedIEditable = hit.collider.gameObject;
                        iEditable.OnSelect(true, transform);
                        selected = true;
                    }
                }

                lastHoveredObject = hit.collider.gameObject;

                //DUPLICATE
                if (selectedIEditable == null)
                {
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        DuplicateObject(lastHoveredObject);
                    }
                }
            }
        }

        if (selectedIEditable != null)
        {
            cursorMaterial.color = Hover;
            selectedIEditable.GetComponent<IEditable>().OnHover();

            //MOVE
            selectedIEditable.GetComponent<IEditable>().Move(transform, Input.mouseScrollDelta.y * 5);

            //REMOVE
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                selectedIEditable.GetComponent<IEditable>().OnDelete();
                selectedIEditable = null;
            }

            //DESELECT
            if (Input.GetKeyDown(KeyCode.Space) && !selected)
            {
                selectedIEditable.GetComponent<IEditable>().OnSelect(false, transform);
                selectedIEditable.GetComponent<IEditable>().OnBlur();
                selectedIEditable = null;
            }
        }

        //BLUR
        if (hit.collider == null && lastHoveredObject != null && selectedIEditable == null)
        {
            lastHoveredObject.GetComponent<IEditable>().OnBlur();
        }
    }
}

public enum EditMode
{
    TerrainHeightMap = 0,
    TerrainTexture = 1,
    Objects = 2,
    AIWaypoints = 3,
    Checkpoints = 4,
}
