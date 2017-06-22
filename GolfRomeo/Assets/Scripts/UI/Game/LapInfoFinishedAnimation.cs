using UnityEngine;
using UnityEngine.UI;

public class LapInfoFinishedAnimation : MonoBehaviour
{
    private Image image;
	
    void Start()
    {
        image = GetComponent<Image>();
    }

	void Update ()
    {
        image.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time));
	}
}
