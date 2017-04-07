using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEditor : MonoBehaviour
{
    public bool Mouse;

    private GameObject lastObject;
    private Vector3 raycastOrigin, raycastDirection;
    private float raycastLength;

    private Vector3 raycastPos = new Vector3(0,10,0);

    public float TerrainHeightEditModifier = 0.001f;
    private TerrainHeightEditor terrainHeightEditor;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
    }

    void Update ()
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
            raycastPos += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            raycastOrigin = raycastPos;
            raycastDirection = -Vector3.up;
            raycastLength = 20;
        }

        RaycastHit hit;
        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << World.TerrainMask, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastLength);

        //TODO CLAMP
        raycastPos = new Vector3(hit.point.x, 10, hit.point.z);

        transform.position = hit.point;
        transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
        transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);

        //EDIT ROAD



        //EDIT TERRAIN
        if (Input.GetKeyDown(KeyCode.P))
        {
            terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier);
        }


        //EDIT WORLD OBJECTS

        Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength, 1 << World.TerrainObjects, QueryTriggerInteraction.Collide);

        if (hit.collider != null && hit.collider.gameObject != null)
        {
            var worldObject = hit.collider.gameObject.GetComponent<WorldObject>();

            if (worldObject != null)
            {
                worldObject.OnHover();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    worldObject.OnSelect(transform);
                }

                lastObject = hit.collider.gameObject;
            }
            else
            {
                lastObject = null;
            }
            
            if (lastObject != null && lastObject != hit.collider.gameObject)
            {
                lastObject.GetComponent<WorldObject>().OnBlur();
            }
        }
    }
}
