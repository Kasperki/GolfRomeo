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

    private bool selected;

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
        var hit = RaycastAgainstTerrain(cursorHitLayer);

        //TODO CLAMP
        if (hit.collider != null)
        {
            raycastPos = new Vector3(hit.point.x, 10, hit.point.z);
            transform.position = hit.point;
            transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
            transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);
        }

        //BRUSH SIZE
        BrushSize += (int)Input.mouseScrollDelta.y * 5;
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
        BrushRenderer.transform.localScale = new Vector3(1 * BrushSize / 10, 1 * BrushSize / 10, 0.5f);

        //DEBUGGGs
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EditMode = EditMode.Terrain;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EditMode = EditMode.Objects;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EditMode = EditMode.Checkpoints;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            EditMode = EditMode.AIWaypoints;


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

            //DUPLICATE

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
    Terrain = 0,
    Objects = 1,
    AIWaypoints = 2,
    Checkpoints = 3,
}
