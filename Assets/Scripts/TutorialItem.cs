using System;
using UnityEngine;

/// <summary>
/// チュートリアルで使用するアイテムの基底クラスです。
/// </summary>

public class TutorialItem : MonoBehaviour
{
    /// <summary>
    /// アイテムが破壊された時のイベント
    /// アイテムのIDを引数として渡し、TutorialControllerで次のフェーズへの移行を制御
    /// </summary>
    public static event Action<string> OnItemDestroyed;

    /// アイテムを識別するためのID。
    public string itemId;

    void OnDestroy()
    {
        OnItemDestroyed?.Invoke(itemId);        
    }
}
