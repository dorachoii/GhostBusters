using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource uiAudioSource;
    private AudioSource bgAudioSource;
    public AudioClip[] clips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        DontDestroyOnLoad(this);
        bgAudioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        uiAudioSource.PlayOneShot(clips[0]);
    }

    public void PlayGameOver()
    {
        uiAudioSource.PlayOneShot(clips[1]);
    }

    public void ChangePlaySpeed(float targetSpeed, float duration = 1f)
    {
       StartCoroutine(ChangePitchSmoothly(targetSpeed, duration));
    }

    IEnumerator ChangePitchSmoothly(float targetSpeed, float duration)
    {
        float startPitch = bgAudioSource.pitch;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            bgAudioSource.pitch = Mathf.Lerp(startPitch, targetSpeed, timeElapsed / duration);
            yield return null;
        }

        bgAudioSource.pitch = targetSpeed;
    }

}
