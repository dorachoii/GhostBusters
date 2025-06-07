using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// チュートリアルの進行段階を定義します。
/// </summary>
public enum TutorialPhase
{
    PlayerAppear,        // プレイヤー登場
    MoveTutorial,        // 移動チュートリアル
    SuckTutorial,        // 吸い込みチュートリアル
    BlowTutorial,        // 吹き出しチュートリアル
    SuckNBlowTutorial,   // 吸い込みと吹き出しの組み合わせチュートリアル
    Done                 // チュートリアル完了
}

/// <summary>
/// チュートリアルの進行を管理するクラスです。
/// 各フェーズの状態を監視し、条件が満たされたら次のフェーズに進みます。
/// </summary>
public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    public TutorialPhase currentPhase = TutorialPhase.PlayerAppear;
    public event Action<TutorialPhase> onPhaseChanged;
    public TutorialUIManager uiManager;

    void OnEnable()
    {
        TutorialPlayer.OnIntroComplete += HandleIntroComplete;
        TutorialPlayer.OnNearHeart += MoveToSuckPhase;
        TutorialItem.OnItemDestroyed += MoveToNext;
        stats.OnCoinChanged += HandleBlowNSuck;
    }

    void OnDisable()
    {
        TutorialPlayer.OnIntroComplete -= HandleIntroComplete;
        TutorialPlayer.OnNearHeart -= MoveToSuckPhase;
        TutorialItem.OnItemDestroyed -= MoveToNext;
        stats.OnCoinChanged -= HandleBlowNSuck;
    }

    void Update()
    {
        switch (currentPhase)
        {
            case TutorialPhase.PlayerAppear:
                break;
            case TutorialPhase.MoveTutorial:
                break;
            case TutorialPhase.SuckTutorial:
                break;
            case TutorialPhase.BlowTutorial:
                break;
            case TutorialPhase.SuckNBlowTutorial:
                break;
            case TutorialPhase.Done:
                StartCoroutine(LoadNextScene());
                break;

            default:
                break;
        }
    }

    public void GotoNextPhase()
    {
        currentPhase++;
        onPhaseChanged?.Invoke(currentPhase);
    }

    void HandleIntroComplete()
    {
        uiManager.ONStartBtn();
    }

    /// <summary>
    /// スタートボタンが押された時の処理を行います。
    /// </summary>
    public void PushStartBtn()
    {
        uiManager.OFFTitle();
        GotoNextPhase();
    }

    /// <summary>
    /// ハートに近づいた時に吸い込みチュートリアルに移行します。
    /// </summary>
    void MoveToSuckPhase()
    {
        if (currentPhase == TutorialPhase.MoveTutorial)
        {
            GotoNextPhase();
        }
    }

    /// <summary>
    /// アイテムが破壊された時に次のフェーズに進みます。
    /// </summary>
    void MoveToNext(string item)
    {
        switch (item)
        {
            case "Lobstar":
                GotoNextPhase();
                break;
            case "Pizza":
                GotoNextPhase();
                break;
        }
    }

    private int? lastItemCount = null;

    /// <summary>
    /// アイテム数の変化を監視し、吸い込みと吹き出しの組み合わせチュートリアルの完了を判定します。
    /// </summary>
    void HandleBlowNSuck(int currentItemCount)
    {
        if (lastItemCount == null)
        {
            lastItemCount = currentItemCount;
            return;
        }

        Debug.Log("current" + currentItemCount);
        if (currentPhase == TutorialPhase.SuckNBlowTutorial && currentItemCount < lastItemCount)
        {
            GotoNextPhase();
            lastItemCount = null; // 初期化して再度呼ばれないようにする
        }
        else
        {
            lastItemCount = currentItemCount;
        }
    }

    /// <summary>
    /// 次のシーンに遷移します。
    /// </summary>
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneLoader.Instance.LoadNextScene();
    }
}
