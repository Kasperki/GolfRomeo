using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public RectTransform ContentParent;
    public PlayUI playUI;

    public Button PlayButton;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        ContentParent.gameObject.SetActive(true);
    }

    public void Play()
    {
        ContentParent.gameObject.SetActive(false);
        playUI.Init();
    }
}
