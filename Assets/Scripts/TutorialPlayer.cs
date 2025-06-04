using UnityEngine;
using System.Collections;
using System;

public class TutorialPlayer : MonoBehaviour
{
    public Transform targetPosition;
    public float moveDuration = 2.5f;
    public float rotateDuration = 5f;

    public static event Action OnIntroComplete;
    public static event Action OnNearHeart;

    private void Start()
    {
        GetComponent<PlayerController>().enabled = false;
        StartCoroutine(PlayIntro());
    }

    private void Update()
    {
        CheckDistance();
    }

    IEnumerator PlayIntro()
    {
        Vector3 startPos = targetPosition.position + new Vector3(-10f, 0, 0);
        transform.position = startPos;

        float startYAngle = 0f;
        float endYAngle = 720f + 30f;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float smoothT = Mathf.SmoothStep(0, 1, t);

            Vector3 newPos = Vector3.Lerp(startPos, targetPosition.position, smoothT);
            transform.position = new Vector3(newPos.x, targetPosition.position.y, targetPosition.position.z);


            float currentYAngle = Mathf.Lerp(startYAngle, endYAngle, smoothT);
            transform.rotation = Quaternion.Euler(0f, currentYAngle, 0f);

            yield return null;
        }

        transform.position = targetPosition.position;
        transform.rotation = Quaternion.Euler(0f, 30f, 0f);

        GetComponent<PlayerController>().enabled = true;
        OnIntroComplete?.Invoke();
    }

    public void CheckDistance()
    {
        float checkRadius = 3f; 
        Vector3 checkPosition = transform.position; 

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, checkRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Heart"))
            {
                OnNearHeart?.Invoke();
                return;
            }
        }
    }
}
