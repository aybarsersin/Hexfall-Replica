using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void ChangeScene(string sceneTitle)
    {
        SceneManager.LoadScene(sceneTitle);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
