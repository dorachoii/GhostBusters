using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの被ダメージや回復時の発光エフェクトを制御します。
/// マテリアルの色を変更することで、視覚的なフィードバックを提供します。
/// </summary>

// 発光エフェクトの種類を定義
public enum FlickerType
{
    Hit,  // 被ダメージ時の赤色発光
    Heal  // 回復時の緑色発光
}

public class PlayerBlink : MonoBehaviour
{
    // マテリアル関連
    private Material mat;           // プレイヤーのマテリアル
    private Color originColor;      // 元の色
    private Color hitColor;         // 被ダメージ時の色（赤）
    private Color healColor;        // 回復時の色（緑）

    // エフェクトの設定
    private const float FlickerDuration = 1.2f;  // 発光エフェクトの持続時間
    private const float FlickerSpeed = 0.4f;     // 発光の切り替え速度

    private void Awake()
    {
        // マテリアルと色の初期化
        mat = GetComponent<SkinnedMeshRenderer>().material;
        originColor = mat.color;
        hitColor = Color.red * 2f;        // 赤色を2倍の明るさに
        healColor = Color.lawnGreen * 2f; // 緑色を2倍の明るさに
    }

    /// <summary>
    /// 指定された種類の発光エフェクトを開始します。
    /// </summary>
    /// <param name="type">発光エフェクトの種類（被ダメージ/回復）</param>
    public void PlayFlicker(FlickerType type)
    {
        // 実行中のコルーチンを停止
        StopAllCoroutines();

        // エフェクトの種類に応じた色を設定
        Color flickerColor = type switch
        {
            FlickerType.Hit => hitColor,
            FlickerType.Heal => healColor,
            _ => originColor
        };

        // 発光エフェクトのコルーチンを開始
        StartCoroutine(FlickerCoroutine(flickerColor));
    }

    /// <summary>
    /// マテリアルの色を交互に切り替えて発光エフェクトを表現します。
    /// </summary>
    /// <param name="color">発光させる色</param>
    private IEnumerator FlickerCoroutine(Color color)
    {
        float elapsed = 0f;
        bool isHit = false;

        // 指定時間まで発光エフェクトを繰り返す
        while (elapsed < FlickerDuration)
        {
            // 発光状態を切り替え
            if (isHit)
            {
                // 元の色に戻す
                mat.SetColor("_Emission_Color", originColor);
                mat.SetColor("_Color", originColor);
            }
            else
            {
                // 発光色を設定
                mat.SetColor("_Emission_Color", color);
                mat.SetColor("_Color", color);
            }

            isHit = !isHit;
            yield return new WaitForSeconds(FlickerSpeed);
            elapsed += FlickerSpeed;
        }

        // エフェクト終了時に元の色に戻す
        mat.SetColor("_Emission_Color", originColor);
        mat.SetColor("_Color", originColor);
    }
}
