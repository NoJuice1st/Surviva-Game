using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("New Scene");
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void PauseMenu()
    {

    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
}
