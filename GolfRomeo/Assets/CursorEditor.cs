using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEditor : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 20, 1 << World.TerrainMask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, -transform.up);

        //transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        //transform.rotation = Quaternion.FromToRotation(Vector3.right, hit.normal);


        //EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 50, 1 << World.TerrainMask, QueryTriggerInteraction.Ignore);

        transform.position = hit.point;

        transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 10);
        transform.RotateAround(transform.position, transform.up, transform.localEulerAngles.y);
    }
}
