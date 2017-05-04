using UnityEngine;

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
    private Vector3 raycastPos;
    private float raycastLength;

    //Track spesific
    public GameObject CheckpointPrefab, WaypointPrefab;

    private TerrainEditor terrainEditor;

    public KeyCode Select = KeyCode.Space;
    public KeyCode Delete = KeyCode.Delete;

    private bool selected;
    private GameObject selectedIEditable;

    private void Awake()
    {
        cursorMaterial = Renderer.material;
        terrainEditor = GetComponent<TerrainEditor>();
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
            raycastPos += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 4;
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
        //DEBUGGGs
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EditMode = EditMode.Terrain;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EditMode = EditMode.Objects;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EditMode = EditMode.Checkpoints;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            EditMode = EditMode.AIWaypoints;

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

        switch (EditMode)
        {
            case EditMode.Terrain:
                terrainEditor.UpdateTerrainEditorTools();
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
        return InstantiateObject(Resources.Load(name) as GameObject, obj.transform); //TODO LOAD FROM FOLDER, MAKE THESE TO OWN CLASS
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
            if (Input.GetKeyDown(KeyCode.H))
            {
                DuplicateObject(selectedIEditable);
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
    Terrain = 0,
    Objects = 1,
    AIWaypoints = 2,
    Checkpoints = 3,
}
