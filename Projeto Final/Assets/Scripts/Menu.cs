using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void goMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 0;
    }

    public void ActiveConfig(GameObject go)
    {
        go.SetActive(true);
    }

    public void DisableConfig(GameObject go) 
    { 
        go.SetActive(false); 
    }

    public void ActivePause(GameObject go)
    { 
        go.SetActive(true);
        Time.timeScale = 0;
    }
    public void DisablePause(GameObject go)
    {
        go.SetActive(false);
        Time.timeScale = 1;
    }

}
