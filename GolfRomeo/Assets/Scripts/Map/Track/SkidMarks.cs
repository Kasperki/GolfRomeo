using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SkidMarks : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Texture2D texture;
    private Color baseMarkColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        texture = new Texture2D(1024, 1024);
        meshRenderer.material.SetTexture("_MainTex", texture);
    }

    public void Init()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }

        texture.Apply();
        UpdateMeshVerticesToTerrain(GetComponent<MeshFilter>());
    }

    private void UpdateMeshVerticesToTerrain(MeshFilter meshFilter)
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        var verticesCopy = new Vector3[vertices.Length];

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            verticesCopy[i] = transform.TransformPoint(vertices[i]);
        }

        var g = new GameObject();
        var d = g.AddComponent<TerrainEditorTools>();
        verticesCopy = d.CoordinatesToTerrain(verticesCopy);
        Destroy(g);

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, verticesCopy[i].z / 5 - 2.4f, vertices[i].z);
        }

        meshFilter.mesh.vertices = vertices;
    }

    public void AddSkidMarks(Vector3 position, float force = 0.05f)
    {
        //Car position in track
        position = position - transform.position + new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
        position *= -1;

        int texturePosX = (int)(texture.width * (position.x / transform.localScale.x));
        int texturePosY = (int)(texture.height * (position.z / transform.localScale.z));

        var oldColor = texture.GetPixel(texturePosX, texturePosY);
        texture.SetPixel(texturePosX, texturePosY, baseMarkColor + new Color(0,0,0, oldColor.a + force));
    }

    public void LateUpdate()
    {
        texture.Apply(false, false);
    }
}
