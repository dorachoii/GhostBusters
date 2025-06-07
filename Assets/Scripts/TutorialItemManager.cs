using UnityEngine;

/// <summary>
/// チュートリアルで使用するアイテムの表示を管理するクラスです。
/// TutorialControllerのフェーズ変更に応じて、適切なアイテムを表示します。
/// </summary>

public class TutorialItemManager : MonoBehaviour
{
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

    /// Tutorial Phaseに応じて、対応するアイテム
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
