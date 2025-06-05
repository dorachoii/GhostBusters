using UnityEngine;

public enum PlayerAudio
{
    blow,
    suck,
    healed,
    damaged
}

public enum BossAudio
{
    changed,
    damaged
}

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(int idx)
    {
        if (idx < clips.Length)
        {
            audioSource.PlayOneShot(clips[idx]);
        }
    }
}
