using BLINDED_AM_ME;
using UnityEngine;

public class CursorEditor : MonoBehaviour
{
    public EditMode EditMode;

    public Renderer Renderer;
    public Renderer BrushRenderer;
    public Color Normal;
    public Color Hover;
    public Color Selected;
    private Material cursorMaterial;

    public bool Mouse;

    private GameObject lastHoveredObject;
    private Vector3 raycastOrigin, raycastDirection;
    private Vector3 raycastPos;
    private float raycastLength;

    //Track spesific
    public GameObject RoadNodePrefab, CheckpointPrefab, WaypointPrefab;

    //TerrainEditor
    private const int MIN_BRUSH_SIZE = 1;
    private const int MAX_BRUSH_SIZE = 100;
    public int BrushSize;
    public float TerrainHeightEditModifier = 0.0005f;
    private TerrainHeightEditor terrainHeightEditor;

    public KeyCode Select = KeyCode.Space;
    public KeyCode Delete = KeyCode.Delete;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
        cursorMaterial = Renderer.material;
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
            raycastPos += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 2;
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

        int cursorHitLayer = 1 << Track.TerrainMask;
        if (EditMode == EditMode.Objects || EditMode == EditMode.Checkpoints || EditMode == EditMode.AIWaypoints || EditMode == EditMode.Road)
        {
            cursorHitLayer = (1 << Track.TerrainMask) | (1 << Track.RoadMask);
        }

        var hit = RaycastAgainstTerrain(cursorHitLayer);

        //TODO CLAMP
        raycastPos = new Vector3(hit.point.x, 10, hit.point.z);
        transform.position = hit.point;
        transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
        transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);

        //BRUSH SIZE
        BrushSize += (int)Input.mouseScrollDelta.y * 5;
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
        BrushRenderer.transform.localScale = new Vector3(1, 1, 0.5f) * BrushSize / 10;

        //DEBUGGG
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EditMode = EditMode.Road;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EditMode = EditMode.Terrain;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EditMode = EditMode.Objects;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EditMode = EditMode.Checkpoints;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            EditMode = EditMode.AIWaypoints;


        if (EditMode == EditMode.Road)
        {
            MoveObjects(Track.RoadMask);

            if (Input.GetKeyDown(KeyCode.I))
            {
                var obj = InstantiateObject(RoadNodePrefab, Track.Instance.RoadsParent.transform);
                obj.GetComponent<Checkpoint>().SetOrder();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                var road = FindObjectOfType<Trail_Mesh>();
                var roadVetices = road.GetComponent<MeshFilter>().mesh.vertices;

                terrainHeightEditor.RaiseToCoordinate(roadVetices, BrushSize);

                for (int i = 0; i < roadVetices.Length; i++)
                {
                    if (i % 10 == 0)
                    {
                        var position = road.transform.TransformPoint(roadVetices[i]);
                        terrainHeightEditor.SmoothTerrain(position, BrushSize);
                    }
                }
            }
        }
        else if (EditMode == EditMode.Terrain)
        {
            //EDIT TERRAIN
            if (Input.GetKeyDown(KeyCode.Y))
            {
                terrainHeightEditor.RaiseTerrain(TerrainHeightEditModifier, BrushSize);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier, BrushSize);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier, BrushSize);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                terrainHeightEditor.SmoothTerrain(BrushSize);
            }

            if (Input.GetKey(KeyCode.B))
            {
                terrainHeightEditor.UpdateTerrainTexture(1, BrushSize);
            }
        }
        else if (EditMode == EditMode.Objects)
        {
            MoveObjects(Track.MapObjectsMask);
        }
        else if (EditMode == EditMode.Checkpoints)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var obj = InstantiateObject(CheckpointPrefab, Track.Instance.LapTracker.transform);
                obj.GetComponent<Checkpoint>().SetOrder();
            }

            MoveObjects(Track.CheckpointsMask);
        }
        else if (EditMode == EditMode.AIWaypoints)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                InstantiateObject(WaypointPrefab, Track.Instance.WayPointCircuit.transform);
                FindObjectOfType<WayPointCircuit>().CachePositionsAndDistances();
            }

            MoveObjects(Track.AIWaypointsMask);
        }
    }

    private GameObject InstantiateObject(GameObject obj, Transform parent)
    {
        var gameObj = GameObject.Instantiate(obj);
        gameObj.transform.position = transform.position;
        gameObj.transform.SetParent(parent);

        var iEditable = gameObj.GetComponent(typeof(IEditable)) as IEditable;

        selectedIEditable = gameObj;
        iEditable.OnSelect(true, transform);

        return gameObj;
    }

    private GameObject selectedIEditable;

    private void MoveObjects(int layer)
    {
        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << layer, QueryTriggerInteraction.Collide);

        if (hit.collider != null && hit.collider.gameObject != null)
        {
            var iEditable = hit.collider.gameObject.GetComponent(typeof(IEditable)) as IEditable;

            if (iEditable != null)
            {
                iEditable.OnHover();
                cursorMaterial.color = Hover;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    selectedIEditable = selectedIEditable == null || hit.collider.gameObject != selectedIEditable ? hit.collider.gameObject : null;
                    iEditable.OnSelect((selectedIEditable != null), transform);
                }

                lastHoveredObject = hit.collider.gameObject;
            }
            else
            {
                lastHoveredObject = null;
            }

            if (lastHoveredObject != null && lastHoveredObject != hit.collider.gameObject)
            {
                lastHoveredObject.GetComponent<IEditable>().OnBlur();
            }
        }

        if (selectedIEditable != null)
        {
            selectedIEditable.GetComponent<IEditable>().Move(transform, Input.mouseScrollDelta.y * 5);

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                selectedIEditable.GetComponent<IEditable>().OnDelete();
                selectedIEditable = null;
            }
        }
    }
}

public enum EditMode
{
    Road = 0,
    Terrain = 1,
    Objects = 2,
    AIWaypoints = 3,
    Checkpoints = 4,
}
