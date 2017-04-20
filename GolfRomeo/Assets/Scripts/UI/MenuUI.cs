using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void PlayMap()
    {
        GameManager.SetState(State.Pause);
        SceneManager.LoadScene("PlayMap");

        //TODO LOAD SELECTED CARS
    }

    public void EditMap()
    {
        GameManager.SetState(State.Edit);
        SceneManager.LoadScene("EditMap");
    }
}
