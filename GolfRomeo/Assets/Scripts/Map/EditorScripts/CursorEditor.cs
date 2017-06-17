using System.Linq;
using UnityEngine;

/*
    TODO
    - With Controllers seconds axis, to control the camera.
    - Controllers should be able to rotate objects.
    - Use should be able to control objects position in Y-axis
*/

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

    public CursorUI CursorUI;
    public ControllerScheme ControlScheme;

    public GameObject GetSelectedObject { get { return selectedIEditable; } }

    public GameObject GetLastHoveredObject { get { return lastHoveredObject; } }

    private void Awake()
    {
        cursorMaterial = Renderer.material;
        terrainEditor = GetComponent<TerrainEditor>();
        CursorUI = GetComponentInChildren<CursorUI>();
    }
    
    private RaycastHit RaycastAgainstTerrain(int cursorHitLayer)
    {
        if (Mouse) //TODO FIRST PLAYER IS MOUSE, NEXT INPUT IS CONTROLLER?
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            raycastOrigin = ray.origin;
            raycastDirection = ray.direction;
            raycastLength = 50;
        }
        else
        {
            if (CursorUI.IsActive() == false)
            {
                if (CursorUI.DriveTesting == false)
                {
                    raycastPos += new Vector3(Input.GetAxis(ControlScheme.HorizontalAxis), 0, Input.GetAxis(ControlScheme.VerticalAxis)) * Time.deltaTime * 6;
                }
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

        if (Input.GetKeyUp(ControlScheme.Cancel))
        {
            CursorUI.Init();
        }

        if (CursorUI.IsActive() == false)
        {
            if (InputManager.StartPressed() || InputManager.BackPressed())
            {
                CursorUI.Save();
            }
        }

        UpdateZoom();

        int cursorHitLayer = 1 << (int)TrackMask.Terrain;
        var hit = RaycastAgainstTerrain(cursorHitLayer);
        terrainEditor.BrushRenderer.enabled = false;
        Renderer.enabled = true;

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
                Renderer.enabled = false;
                terrainEditor.UpdateTerrainHeightMap();
                break;
            case EditMode.TerrainTexture:
                Renderer.enabled = false;
                terrainEditor.UpdateTerrainTexture();
                break;
            case EditMode.Objects:
                MoveObjects();
                break;
            default:
                break;
        }
    }

    float lastTime;
    Vector3 cachedPosition;

    private void UpdateZoom()
    {
        if (CursorUI.IsActive() == false && selectedIEditable == null)
        {
            if (Time.time > lastTime)
            {
                lastTime = Time.time + 0.75f;
                cachedPosition = transform.position;
            }

            FindObjectOfType<CameraZoom>().Zoom(-Input.mouseScrollDelta.y, cachedPosition); //TODO GET AXIS FROM CONTROL SCHEME
        }
    }

    public void CreateWaypoint()
    {
        InitializeObject(Instantiate(WaypointPrefab), Track.Instance.WayPointCircuit.transform);
        FindObjectOfType<WayPointCircuit>().CachePositionsAndDistances();
    }

    public void CreateCheckpoint()
    {
        var obj = InitializeObject(Instantiate(CheckpointPrefab), Track.Instance.LapTracker.transform);
        obj.GetComponent<Checkpoint>().SetOrder();
    }

    public void CreateNewObject(GameObject obj)
    {
        InitializeObject(obj, Track.Instance.TrackObjectsParent.transform);
    }

    private GameObject InitializeObject(GameObject obj, Transform parent)
    {
        obj.transform.position = transform.position;
        obj.transform.SetParent(parent);

        var iEditable = obj.GetComponent(typeof(IEditable)) as IEditable;

        selectedIEditable = obj;
        iEditable.OnSelect(true, transform);

        return obj;
    }

    private GameObject DuplicateObject(GameObject obj)
    {
        string name = obj.GetComponent<TrackObject>().ID;

        var newObj = ResourcesLoader.LoadTrackObject(name);
        newObj.transform.rotation = obj.transform.rotation;

        return InitializeObject(newObj, obj.transform);
    }

    private void MoveObjects()
    {
        selected = false;
        int layer = (1 << (int)TrackMask.TrackObjects) | (1 << (int)TrackMask.AIWaypoints) | (1 << (int)TrackMask.Checkpoints);

        bool hitIEditable = false;
        var hits = Physics.RaycastAll(raycastOrigin, raycastDirection, raycastLength, layer, QueryTriggerInteraction.Collide);

        foreach (var hit in hits.OrderBy(m => (raycastOrigin - m.point).sqrMagnitude))
        {
            if (hit.collider.gameObject.GetComponent<IEditable>() != null)
            {
                hitIEditable = true;

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

                            if (Input.GetKeyDown(ControlScheme.Submit))
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
                            if (Input.GetKeyDown(ControlScheme.Duplicate))
                            {
                                DuplicateObject(lastHoveredObject);
                            }
                        }
                    }
                }

                break;
            }
        }

        if (selectedIEditable != null)
        {
            cursorMaterial.color = Hover;
            selectedIEditable.GetComponent<IEditable>().OnHover();

            //MOVE
            selectedIEditable.GetComponent<IEditable>().Move(transform, Input.mouseScrollDelta.y * 5);

            //REMOVE
            if (Input.GetKeyDown(ControlScheme.Delete))
            {
                selectedIEditable.GetComponent<IEditable>().OnDelete();
                selectedIEditable = null;
            }

            //DESELECT
            if (Input.GetKeyDown(ControlScheme.Submit) && !selected)
            {
                selectedIEditable.GetComponent<IEditable>().OnSelect(false, transform);
                selectedIEditable.GetComponent<IEditable>().OnBlur();
                selectedIEditable = null;
            }
        }

        //BLUR
        if (hitIEditable == false && lastHoveredObject != null && selectedIEditable == null)
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
}
