using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアルのUI表示を管理するクラスです。
/// 各フェーズに応じたガイドテキストの表示と、
/// スタートボタンやタイトルの表示制御を行います。
/// </summary>
public class TutorialUIManager : MonoBehaviour
{
    [SerializeField]
    private TutorialController tutorialController;

    public TextMeshProUGUI guideText;

    /// <summary>
    /// キー操作の説明用スプライト
    /// </summary>
    public Image[] keySprites;

    /// <summary>
    /// スタートボタン
    /// </summary>
    public GameObject startBtn;

    /// <summary>
    /// タイトル表示
    /// </summary>
    public GameObject Title;

    void OnEnable()
    {
        Debug.Log("OnEnable");
        tutorialController.onPhaseChanged += UpdateGuide;
    }

    void OnDisable()
    {
        tutorialController.onPhaseChanged -= UpdateGuide;
    }

    /// <summary>
    /// チュートリアルのフェーズに応じて、ガイドテキストを更新します。
    /// 各フェーズで異なる操作説明を表示します。
    /// </summary>
    /// <param name="phase">現在のチュートリアルフェーズ</param>
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

    /// <summary>
    /// テキストをフェードインしながら更新します。
    /// 0.1秒の遅延後、1秒かけてフェードインします。
    /// </summary>
    /// <param name="text">表示するテキスト</param>
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

    /// <summary>
    /// スタートボタンを表示します。
    /// イントロ完了時に呼び出されます。
    /// </summary>
    public void ONStartBtn()
    {
        startBtn.SetActive(true);
    }

    /// <summary>
    /// スタートボタンとタイトルを非表示にします。
    /// スタートボタンが押された時に呼び出されます。
    /// </summary>
    public void OFFTitle()
    {
        startBtn.SetActive(false);
        Title.SetActive(false);
    }
    
    
}
