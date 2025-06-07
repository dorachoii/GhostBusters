using UnityEngine;

// プレイヤーの音声タイプ
public enum PlayerAudio
{
    blow,    // 吹き飛ばし
    suck,    // 吸い込み
    healed,  // 回復
    damaged  // ダメージ
}

// ボスの音声タイプ
public enum BossAudio
{
    changed, // フェーズ変更
    damaged  // ダメージ
}

/// <summary>
/// 個別のオブジェクトに付属し、audio clipを再生するコンポーネントです。
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;

    private void Awake()
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
