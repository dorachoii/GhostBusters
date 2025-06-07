using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアルのUI表示を管理するクラスです。
/// </summary>

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField]
    private TutorialController tutorialController;

    public TextMeshProUGUI guideText;
    public GameObject startBtn;
    public GameObject Title;

    void OnEnable()
    {
        tutorialController.onPhaseChanged += UpdateGuide;
    }

    void OnDisable()
    {
        tutorialController.onPhaseChanged -= UpdateGuide;
    }


    /// チュートリアルのフェーズに応じて、ガイドテキストを更新
    public void UpdateGuide(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.MoveTutorial:
                StartCoroutine(ChangeText("Move to Lobstar\n using ↑ ← ↓ → keys!"));
                break;
            case TutorialPhase.SuckTutorial:
                StartCoroutine(ChangeText("Press Q to suck Lobstar."));
                break;
            case TutorialPhase.BlowTutorial:
                StartCoroutine(ChangeText("Press W to blow the Pizzashop."));
                break;
            case TutorialPhase.SuckNBlowTutorial:
                StartCoroutine(ChangeText("Suck the rings with Q\nthen blow them out with W."));
                break;
            case TutorialPhase.Done:
                StartCoroutine(ChangeText("Tutorial complete!\nTime for the boss fight."));
                break;
            default:
                break;
        }
    }

    IEnumerator ChangeText(string text)
    {
        yield return new WaitForSeconds(0.1f);

        guideText.text = text;

        Color color = guideText.color;
        color.a = 0f;
        guideText.color = color;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            guideText.color = color;
            yield return null;
        }

        color.a = 1f;
        guideText.color = color;
    }

    /// イントロ完了時に呼び出されます。
    public void ONStartBtn()
    {
        startBtn.SetActive(true);
    }


    /// スタートボタンが押された時に呼び出されます。
    public void OFFTitle()
    {
        startBtn.SetActive(false);
        Title.SetActive(false);
    }
    
    
}
