using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public SceneTransition sceneTransition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex + 1);
    }

    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void LoadScene(int sceneIndex)
    {
        if (sceneTransition != null)
        {
            sceneTransition.PlayFadeOut(() =>
            {
                SceneManager.LoadScene(sceneIndex);
            });
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
//Pitch조절하기