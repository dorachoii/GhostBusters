using UnityEngine;

/// <summary>
/// 破壊可能なオブジェクトに付属し、破壊時の効果音を再生するコンポーネントです。
/// </summary>
public class AudioPlayer_BreakSound : MonoBehaviour
{
    // 破壊時の効果音プレハブ
    public GameObject breakSound;

    // 破壊時の効果音を再生
    public void MakeDestroySound()
    {
        GameObject sfx = Instantiate(breakSound, transform.position, Quaternion.identity);
        Destroy(sfx, sfx.GetComponent<AudioSource>().clip.length);
    }
}
