using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public PlayUI playUI;

    public void Play()
    {
        playUI.Init();
    }

    public void EditMap()
    {
        GameManager.SetState(State.Edit);
        SceneManager.LoadScene("EditMap");
    }
}
