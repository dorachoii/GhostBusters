using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum TutorialPhase
{
    PlayerAppear,
    MoveTutorial,
    SuckTutorial,
    BlowTutorial,
    SuckNBlowTutorial,
    Done
}
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

    // TODO: Refactor , Button의 ONClick과 연결되어 있음
    public void PushStartBtn()
    {
        uiManager.OFFTitle();
        GotoNextPhase();
    }

    void MoveToSuckPhase()
    {
        if (currentPhase == TutorialPhase.MoveTutorial)
        {
            GotoNextPhase();
        }
    }

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
            lastItemCount = null; // 초기화해서 이후에 다시 안 불리도록
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
