using UnityEngine;
using System.Collections;

public enum FlickerType
{
    Hit,
    Heal
}

public class FlickerEffectHandler : MonoBehaviour
{
    Material mat;
    Color originColor;
    Color hitColor;
    Color healColor;

    private float flickerDuration = 1.2f;
    private float flickerSpeed = 0.4f;

    private void Awake()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
        originColor = mat.color;
        hitColor = Color.red * 2f;
        healColor = Color.lawnGreen * 2f;
    }

    public void PlayFlicker(FlickerType type)
    {
        StopAllCoroutines();

        Color flickerColor;

        switch (type)
        {
            case FlickerType.Hit:
                flickerColor = hitColor;
                break;
            case FlickerType.Heal:
                flickerColor = healColor;
                break;
            default:
                flickerColor = originColor;
                break;
        }

        StartCoroutine(FlickerCoroutine(flickerColor));
    }

    private IEnumerator FlickerCoroutine(Color color)
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
                mat.SetColor("_Emission_Color", color);
                mat.SetColor("_Color", color);
            }

            isHit = !isHit;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        mat.SetColor("_Emission_Color", originColor);
        mat.SetColor("_Color", originColor);
    }

}
