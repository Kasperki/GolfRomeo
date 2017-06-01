using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SkidMarks : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Texture2D texture;
    private Color skidMarkIncrementColor = new Color(0.8f, 0.8f, 0.8f, 0.1f);

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        texture = new Texture2D(1024, 1024);

        meshRenderer.material.SetTexture("_MainTex", texture);
    }

	public void AddSkidMarks(Vector3 position)
    {
        var oldColor = texture.GetPixel((int)position.x, (int)position.z);

        texture.SetPixel((int)position.x, (int)position.z, oldColor + skidMarkIncrementColor);
        texture.Apply(false, false);
    }

    public void DrawSkidMark(Vector3 position)
    {
        var tr = gameObject.AddComponent<TerrainEditorTools>();
        tr.UpdateTerrainTexture(position, (int)TerrainTextures.Asfalt4, 2);
    }
}
