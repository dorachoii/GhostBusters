using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// シーン遷移時のフェード効果を管理するクラスです。
/// </summary>

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeTime = 1.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        fadeImage.color = new Color(0, 0, 0, 1);

        // シーンがロードされると自動的にFadeIn
        StartCoroutine(FadeIn()); 
    }

    public void PlayFadeOut(System.Action onComplete)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    private IEnumerator FadeOutCoroutine(System.Action onComplete)
    {
        for (float t = 0; t <= fadeTime; t += Time.deltaTime)
        {
            float alpha = t / fadeTime;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        onComplete?.Invoke();
    }

    private IEnumerator FadeIn()
    {
        for (float t = 0; t <= fadeTime; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeTime);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
}
