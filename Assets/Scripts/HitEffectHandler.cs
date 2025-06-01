using UnityEngine;
using System.Collections;

public class HitEffectHandler : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Material originalMaterial;
    public Material hitMaterial;

    public float flickerDuration = 0.5f;
    public float flickerSpeed = 0.1f;

    private void Awake()
    {
    
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
            originalMaterial = meshRenderer.materials[0];
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
            meshRenderer.material = isHit ? hitMaterial : originalMaterial;
            isHit = !isHit;

            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        meshRenderer.material = originalMaterial;
    }
}
