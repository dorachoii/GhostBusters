using UnityEngine;
using System.Collections;

public class HitEffectHandler : MonoBehaviour
{
    Material mat;
    Color originColor;
    Color hitColor;

    private float flickerDuration = 1.2f;
    private float flickerSpeed = 0.4f;

    private void Awake()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
        originColor = mat.color;
        hitColor = Color.red * 2f;
    }

    public void PlayFlicker()
    {
        StopAllCoroutines();
        StartCoroutine(FlickerCoroutine());
    }

    private IEnumerator FlickerCoroutine()
    {
        float elapsed = 0f;
        bool isHit = false;

        while (elapsed < flickerDuration)
        {
            if (isHit)
            {
                mat.SetColor("_Emission_Color", originColor);
                mat.SetColor("_Color", originColor);
            }
            else
            {
                mat.SetColor("_Emission_Color", hitColor);
                mat.SetColor("_Color", hitColor);
            }

            isHit = !isHit;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        mat.SetColor("_Emission_Color", originColor);
        mat.SetColor("_Color", originColor);
    }

}
