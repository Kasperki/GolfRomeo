using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public RectTransform ContentParent;
    public PlayUI playUI;
    public OptionsUI optionsUI;
    public EditorUI editorUI;

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

    public void Edit()
    {
        ContentParent.gameObject.SetActive(false);
        editorUI.Init();
    }

    public void Options()
    {
        ContentParent.gameObject.SetActive(false);
        optionsUI.Init();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
