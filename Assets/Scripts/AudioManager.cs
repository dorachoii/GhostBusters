using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip[] clips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PlayClick()
    {
        uiAudioSource.PlayOneShot(clips[0]);
    }

    public void PlayGameOver()
    {
        uiAudioSource.PlayOneShot(clips[1]);
    }
}
