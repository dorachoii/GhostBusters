using System.Collections;
using UnityEngine;

/// <summary>
/// bossPhase3への変身のエフェクトを制御します。
/// </summary>

public class BossChange : MonoBehaviour
{
    // マテリアル変更用のコンポーネント
    private steve steve;

    // 最終的なマテリアルインデックス
    private const int FinalBodyMat = 6;
    private const int FinalLEye = 2;
    private const int FinalREye = 3;
    // マテリアルの最大数
    private int maxBodyMat;
    private int maxEyeMat;

    private void Awake()
    {
        steve = GetComponent<steve>();
        maxBodyMat = steve.my_body_materials.Length;
        maxEyeMat = steve.eye_cover_mat.Length;
    }

    // フェーズ変更を開始
    public void PhaseChange()
    {
        StartCoroutine(RandomColorChange());
    }

    // ランダムな色変化
    private IEnumerator RandomColorChange()
    {
        const float Duration = 1.2f;
        const float Interval = 0.2f;
        float elapsed = 0f;

        // ランダムな色変化を繰り返す
        while (elapsed < Duration)
        {
            steve.Body_Materials = Random.Range(0, maxBodyMat);
            steve.L_Eye_Cover_Color = Random.Range(0, maxEyeMat);
            steve.R_Eye_Cover_Color = Random.Range(0, maxEyeMat);

            elapsed += Interval;
            yield return new WaitForSeconds(Interval);
        }

        steve.Body_Materials = FinalBodyMat;
        steve.L_Eye_Cover_Color = FinalLEye;
        steve.R_Eye_Cover_Color = FinalREye;
    }
}


