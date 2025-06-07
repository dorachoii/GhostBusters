using System;
using UnityEngine;

/// <summary>
/// チュートリアルで使用するアイテム（治癒アイテムや建物）の基底クラスです。
/// アイテムが破壊された時に、TutorialControllerに通知を送り、
/// アイテムの種類に応じた次のチュートリアルフェーズへの移行を制御します。
/// </summary>
public class TutorialItem : MonoBehaviour
{
    /// <summary>
    /// アイテムが破壊された時に発火するイベント。
    /// アイテムのIDを引数として渡し、TutorialControllerで次のフェーズへの移行を制御します。
    /// </summary>
    public static event Action<string> OnItemDestroyed;

    /// <summary>
    /// アイテムを識別するためのID。
    /// 例: "Lobstar"（治癒アイテム）, "Pizza"（建物）など
    /// </summary>
    public string itemId;

    void OnDestroy()
    {
        OnItemDestroyed?.Invoke(itemId);        
    }
}
