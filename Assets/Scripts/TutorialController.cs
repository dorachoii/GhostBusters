using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// チュートリアルの進行段階を定義します。
/// </summary>
public enum TutorialPhase
{
    PlayerAppear,        
    MoveTutorial,        
    SuckTutorial,        
    BlowTutorial,        
    SuckNBlowTutorial,   
    Done                 
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
        stats.OnRingChanged += HandleBlowNSuck;
    }

    void OnDisable()
    {
        TutorialPlayer.OnIntroComplete -= HandleIntroComplete;
        TutorialPlayer.OnNearHeart -= MoveToSuckPhase;
        TutorialItem.OnItemDestroyed -= MoveToNext;
        stats.OnRingChanged -= HandleBlowNSuck;
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

    /// スタートボタンが押された時の処理を行います。
    public void PushStartBtn()
    {
        uiManager.OFFTitle();
        GotoNextPhase();
    }

    /// MoveTutorial
    void MoveToSuckPhase()
    {
        if (currentPhase == TutorialPhase.MoveTutorial)
        {
            GotoNextPhase();
        }
    }

    /// SuckTutorial, BlowTutorial
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

    /// BlowNSuck Tutorial
    void HandleBlowNSuck(int currentItemCount)
    {
        if (lastItemCount == null)
        {
            lastItemCount = currentItemCount;
            return;
        }

        // アイテム数が増加した後に減少した場合、吹き飛ばしと判断
        if (currentPhase == TutorialPhase.SuckNBlowTutorial && currentItemCount < lastItemCount)
        {
            GotoNextPhase();
            lastItemCount = null; 
        }
        else
        {
            lastItemCount = currentItemCount;
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneLoader.Instance.LoadNextScene();
    }
}
