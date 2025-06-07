using UnityEngine;

/// <summary>
/// プレイヤーが取得した時にHPを回復させるアイテムの動作を制御します。
/// 一度使用すると消滅します。
/// </summary>
public class HealItem : MonoBehaviour
{
    // 回復量の設定
    [SerializeField] private int healAmount = 20;  // 回復するHPの量
    // アイテムの使用状態
    private bool used = false;                     // アイテムが使用済みかどうか

    /// <summary>
    /// プレイヤーのHPを回復させ、アイテムを消滅させます。
    /// </summary>
    /// <param name="stats">回復対象のプレイヤーステータス</param>
    public void Heal(PlayerStats stats)
    {
        // 使用済みの場合は何もしない
        if (used) return;
        
        // プレイヤーのHPを回復
        stats.Heal(healAmount);
        // 使用済みフラグを立てる
        used = true;
        // 1秒後にアイテムを消滅
        Destroy(gameObject, 1);
    }
}
