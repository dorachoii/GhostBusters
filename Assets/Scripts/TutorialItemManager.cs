using UnityEngine;

/// <summary>
/// チュートリアルで使用するアイテムの表示を管理するクラスです。
/// TutorialControllerのフェーズ変更に応じて、適切なアイテムを表示します。
/// 各アイテムはTutorialItemコンポーネントを持ち、破壊された時に
/// TutorialControllerに通知を送ることで次のフェーズへの移行を制御します。
/// </summary>
public class TutorialItemManager : MonoBehaviour
{
    /// <summary>
    /// チュートリアルで使用するアイテムのGameObject配列。
    /// インデックス0: 移動チュートリアル用のアイテム
    /// インデックス1: 吹き出しチュートリアル用のアイテム
    /// </summary>
    public GameObject[] items;

    [SerializeField]
    private TutorialController controller;

    void OnEnable()
    {
        controller.onPhaseChanged += HandleItem;
    }

    void OnDisable()
    {
        controller.onPhaseChanged -= HandleItem;
    }

    /// <summary>
    /// チュートリアルのフェーズに応じて、対応するアイテムを表示します。
    /// 表示されたアイテムはTutorialItemコンポーネントを通じて、
    /// 破壊された時にTutorialControllerに通知を送ります。
    /// </summary>
    /// <param name="phase">現在のチュートリアルフェーズ</param>
    public void HandleItem(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.MoveTutorial:
                items[0].SetActive(true);
                break;
            case TutorialPhase.BlowTutorial:
                items[1].SetActive(true);
                break;
        }        
    }
}
