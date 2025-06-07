using UnityEngine;
using System.Collections;

/// <summary>
/// Player発光エフェクトを制御します。
/// 被ダメージ、回復の視覚的なフィードバックを提供
/// </summary>

// 発光エフェクトの種類を定義
public enum FlickerType
{
    Hit,  
    Heal  
}

public class PlayerBlink : MonoBehaviour
{
    // マテリアル関連
    private Material mat;           
    private Color originColor;      
    private Color hitColor;         
    private Color healColor;        

    // エフェクトの設定
    private const float FlickerDuration = 1.2f;  
    private const float FlickerSpeed = 0.4f;     

    private void Awake()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
        originColor = mat.color;
        hitColor = Color.red * 2f;        
        healColor = Color.lawnGreen * 2f; 
    }

    /// 指定された種類の発光エフェクトを開始
    public void PlayFlicker(FlickerType type)
    {
        StopAllCoroutines();

        // エフェクトの種類に応じた色を設定
        Color flickerColor = type switch
        {
            FlickerType.Hit => hitColor,
            FlickerType.Heal => healColor,
            _ => originColor
        };

        StartCoroutine(FlickerCoroutine(flickerColor));
    }


    /// マテリアルの色を交互に切り替えて発光エフェクトを表現します。
    private IEnumerator FlickerCoroutine(Color color)
    {
        float elapsed = 0f;
        bool isHit = false;

        while (elapsed < FlickerDuration)
        {
            if (isHit)
            {
                mat.SetColor("_Emission_Color", originColor);
                mat.SetColor("_Color", originColor);
            }
            else
            {
                mat.SetColor("_Emission_Color", color);
                mat.SetColor("_Color", color);
            }

            isHit = !isHit;
            yield return new WaitForSeconds(FlickerSpeed);
            elapsed += FlickerSpeed;
        }

        mat.SetColor("_Emission_Color", originColor);
        mat.SetColor("_Color", originColor);
    }
}
