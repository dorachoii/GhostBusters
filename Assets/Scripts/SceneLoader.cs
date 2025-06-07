using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を管理するシングルトンクラスです。
/// シーンの読み込み、再読み込み、フェード遷移などの機能を提供します。
/// 
/// 主な機能:
/// - シーンの非同期読み込み
/// - フェード遷移エフェクト
/// - 現在シーンの再読み込み
/// - シングルトンパターンによるグローバルアクセス
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// シーンローダーのシングルトンインスタンス
    /// </summary>
    public static SceneLoader Instance { get; private set; }

    /// <summary>
    /// シーン遷移時のフェードエフェクトを制御するコンポーネント
    /// </summary>
    [SerializeField] private SceneTransition sceneTransition;

    /// <summary>
    /// シングルトンパターンの初期化を行います。
    /// 既存のインスタンスが存在する場合は破棄します。
    /// </summary>
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

    /// <summary>
    /// 次のシーンを読み込みます。
    /// 現在のシーンのインデックスに1を加えたシーンに遷移します。
    /// </summary>
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex + 1);
    }

    /// <summary>
    /// 現在のシーンを再読み込みします。
    /// オーディオの再生速度を1にリセットします。
    /// </summary>
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        AudioManager.Instance.ChangePlaySpeed(1);
    }

    /// <summary>
    /// 指定されたインデックスのシーンを読み込みます。
    /// フェード遷移が設定されている場合は、フェードアウト後にシーンを読み込みます。
    /// </summary>
    /// <param name="sceneIndex">読み込むシーンのインデックス</param>
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