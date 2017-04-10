using BLINDED_AM_ME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEditor : MonoBehaviour
{
    public Color Normal;
    public Color Hover;
    public Color Selected;
    private Material material;

    public bool Mouse;

    private GameObject lastObject;
    private Vector3 raycastOrigin, raycastDirection;
    private float raycastLength;

    private Vector3 raycastPos = new Vector3(0,10,0);

    public GameObject RoadNodePrefab;

    public float TerrainHeightEditModifier = 0.001f;
    private TerrainHeightEditor terrainHeightEditor;


    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
        material = GetComponent<Renderer>().material;
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

        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << Map.TerrainMask, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastLength);

        //TODO CLAMP
        raycastPos = new Vector3(hit.point.x, 10, hit.point.z);

        transform.position = hit.point;
        transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
        transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);

        //EDIT ROAD

        if (Input.GetKeyDown(KeyCode.I))
        {
            var obj = GameObject.Instantiate(RoadNodePrefab);
            obj.transform.position = transform.position;
            obj.transform.SetParent(FindObjectOfType<Trail_Mesh>().transform);
            var iEditable = obj.GetComponent(typeof(IEditable)) as IEditable;
            iEditable.OnSelect(transform);
        }


        //EDIT TERRAIN
        if (Input.GetKeyDown(KeyCode.P))
        {
            terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            terrainHeightEditor.SmoothTerrain();
        }


        //EDIT WORLD OBJECTS
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << Map.Road, QueryTriggerInteraction.Collide);

        if (hit.collider != null && hit.collider.gameObject != null)
        {
            var iEditable = hit.collider.gameObject.GetComponent(typeof(IEditable)) as IEditable;

            if (iEditable != null)
            {
                iEditable.OnHover();
                material.color = Hover;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    iEditable.OnSelect(transform);
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
    }
}
