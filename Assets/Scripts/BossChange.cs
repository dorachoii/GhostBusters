using System.Collections;
using UnityEngine;

public class BossChange : MonoBehaviour
{
    steve steve;

    int finalBodyMat = 6, finalLEye = 2, finalREye = 3;
    int maxBodyMat, maxEyeMat;

    void Awake()
    {
        steve = GetComponent<steve>();
        maxBodyMat = steve.my_body_materials.Length;
        maxEyeMat = steve.eye_cover_mat.Length;
    }

    public void PhaseChange()
    {
        StartCoroutine(RandomColorChange());
    }

    IEnumerator RandomColorChange()
    {
        float duration = 1.2f;
        float elapsed = 0f;
        float interval = 0.2f;

        while (elapsed < duration)
        {
            steve.Body_Materials = Random.Range(0, maxBodyMat);
            steve.L_Eye_Cover_Color = Random.Range(0, maxEyeMat);
            steve.R_Eye_Cover_Color = Random.Range(0, maxEyeMat);

            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }

        steve.Body_Materials = finalBodyMat;
        steve.L_Eye_Cover_Color = finalLEye;
        steve.R_Eye_Cover_Color = finalREye;
    }
}


