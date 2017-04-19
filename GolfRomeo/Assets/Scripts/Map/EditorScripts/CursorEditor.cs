using BLINDED_AM_ME;
using UnityEngine;

public class CursorEditor : MonoBehaviour
{
    public EditMode EditMode;

    public Renderer Renderer;
    public Color Normal;
    public Color Hover;
    public Color Selected;
    private Material material;

    public bool Mouse;

    private GameObject lastObject;
    private Vector3 raycastOrigin, raycastDirection;
    private float raycastLength;

    private Vector3 raycastPos = new Vector3(0,10,0);

    public GameObject RoadNodePrefab, CheckpointPrefab, WaypointPrefab;

    public float TerrainHeightEditModifier = 0.0005f;
    private TerrainHeightEditor terrainHeightEditor;


    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
        material = Renderer.material;
    }

    void Update ()
    {
        material.color = Normal;

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

        int cursorHitLayer = 1 << Map.TerrainMask;

        if (EditMode == EditMode.Objects || EditMode == EditMode.Checkpoints || EditMode == EditMode.AIWaypoints || EditMode == EditMode.Road)
        {
            cursorHitLayer = (1 << Map.TerrainMask) | (1 << Map.RoadMask);
        }

        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, cursorHitLayer, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastLength);

        //TODO CLAMP
        raycastPos = new Vector3(hit.point.x, 10, hit.point.z);

        transform.position = hit.point;
        transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
        transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);

        //EDIT ROAD

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
            MoveObjects(Map.RoadMask);

            if (Input.GetKeyDown(KeyCode.I))
            {
                var obj = GameObject.Instantiate(RoadNodePrefab);
                obj.transform.position = transform.position;
                obj.transform.SetParent(FindObjectOfType<Trail_Mesh>().transform);
                var iEditable = obj.GetComponent(typeof(IEditable)) as IEditable;

                selectedIEditable = obj;
                iEditable.OnSelect(true, transform);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                var road = FindObjectOfType<Trail_Mesh>();
                var roadVetices = road.GetComponent<MeshFilter>().mesh.vertices;

                terrainHeightEditor.RaiseToCoordinate(roadVetices, 25);

                for (int i = 0; i < roadVetices.Length; i++)
                {
                    if (i % 10 == 0)
                    {
                        var position = road.transform.TransformPoint(roadVetices[i]);
                        terrainHeightEditor.SmoothTerrain(position, 50);
                    }
                }
            }
        }
        else if (EditMode == EditMode.Terrain)
        {
            //EDIT TERRAIN
            if (Input.GetKeyDown(KeyCode.Y))
            {
                terrainHeightEditor.RaiseTerrain(TerrainHeightEditModifier, 100);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier, 100);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier, 50);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                terrainHeightEditor.SmoothTerrain(120);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                terrainHeightEditor.UpdateTerrainTexture(1, 5);
            }
        }
        else if (EditMode == EditMode.Objects)
        {
            MoveObjects(Map.MapObjectsMask);
        }
        else if (EditMode == EditMode.Checkpoints)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var obj = GameObject.Instantiate(CheckpointPrefab);
                obj.transform.position = transform.position;
                obj.transform.SetParent(FindObjectOfType<LapTracker>().transform);

                obj.GetComponent<Checkpoint>().SetOrder();
                var iEditable = obj.GetComponent(typeof(IEditable)) as IEditable;

                selectedIEditable = obj;
                iEditable.OnSelect(true, transform);
            }

            MoveObjects(Map.CheckpointsMask);
        }
        else if (EditMode == EditMode.AIWaypoints)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var obj = GameObject.Instantiate(WaypointPrefab);
                obj.transform.position = transform.position;
                obj.transform.SetParent(FindObjectOfType<WayPointCircuit>().transform);

                FindObjectOfType<WayPointCircuit>().CachePositionsAndDistances();
                var iEditable = obj.GetComponent(typeof(IEditable)) as IEditable;

                selectedIEditable = obj;
                iEditable.OnSelect(true, transform);
            }

            MoveObjects(Map.AIWaypointsMask);
        }
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
                material.color = Hover;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    selectedIEditable = selectedIEditable == null || hit.collider.gameObject != selectedIEditable ? hit.collider.gameObject : null;
                    iEditable.OnSelect((selectedIEditable != null), transform);
                }

                lastObject = hit.collider.gameObject;
            }
            else
            {
                lastObject = null;
            }

            if (lastObject != null && lastObject != hit.collider.gameObject)
            {
                lastObject.GetComponent<MapObject>().OnBlur();
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
