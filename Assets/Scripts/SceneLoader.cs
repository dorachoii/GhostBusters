using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を管理するsingletonクラスです。
/// </summary>

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private SceneTransition sceneTransition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// 次のシーンを読み込み
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex + 1);
    }

    /// 現在のシーンを再読み込み
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        AudioManager.Instance.ChangePlaySpeed(1);
    }

    /// 指定されたインデックスのシーンを読み込み
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