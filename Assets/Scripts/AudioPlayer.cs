using UnityEngine;

/// <summary>
/// 個別のオブジェクトに付属し、指定された音声クリップを再生するコンポーネントです。
/// </summary>

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

public class AudioPlayer : MonoBehaviour
{
    // 再生する音声クリップの配列
    public AudioClip[] clips;
    // 音声再生用のAudioSource
    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSourceコンポーネントの取得
        audioSource = GetComponent<AudioSource>();
    }

    // 指定されたインデックスの音声を再生
    public void Play(int idx)
    {
        if (idx < clips.Length)
        {
            audioSource.PlayOneShot(clips[idx]);
        }
    }
}
