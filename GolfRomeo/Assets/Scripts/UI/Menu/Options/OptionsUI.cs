using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public GameObject ContentParent;

    public void Init()
    {
        gameObject.SetActive(true);
        ContentParent.gameObject.SetActive(true);
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (InputManager.BackPressed() && GameManager.CheckState(State.Menu))
        {
            Back();
        }
    }
}
