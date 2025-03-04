using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneIndex;

    private void Awake()
    {
        GoToScene();
    }

    void GoToScene()
    {
        if (sceneIndex == 0)
        {
            SceneManager.LoadScene(0);
        }
        if (sceneIndex == 1)
        {
            SceneManager.LoadScene(1);
        }
        if (sceneIndex == 2)
        {
            SceneManager.LoadScene(2);
        }
        if (sceneIndex == 3)
        {
            SceneManager.LoadScene(3);
        }
        if (sceneIndex == 4)
        {
            SceneManager.LoadScene(4);
        }
        if (sceneIndex == 5)
        {
            SceneManager.LoadScene(5);
        }
    }
}
