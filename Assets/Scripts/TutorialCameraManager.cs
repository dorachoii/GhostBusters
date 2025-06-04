using System.Collections;
using UnityEngine;

public enum Cameras
{
    Intro,
    PlayerFollow,
    Item_Lobstar,
    Item_pizzashop,
    Outro
}
public class TutorialCameraManager : MonoBehaviour
{
    public GameObject[] cams;

    [SerializeField]
    private TutorialController tutorialController;

    void OnEnable()
    {
        tutorialController.onPhaseChanged += ChangeCamera;
    }

    void OnDisable()
    {
        tutorialController.onPhaseChanged -= ChangeCamera;
    }

    public void ChangeCamera(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.MoveTutorial:
                TurnOffAllCameras();
                StartCoroutine(TurnOffCamera((int)Cameras.Item_Lobstar));
                break;
            case TutorialPhase.BlowTutorial:
                TurnOffAllCameras();
                StartCoroutine(TurnOffCamera((int)Cameras.Item_pizzashop));
                break;
        }
    }

    IEnumerator TurnOffCamera(int idx)
    {
        cams[idx].SetActive(true);
        yield return new WaitForSeconds(2f);
        cams[idx].SetActive(false);
        cams[(int)Cameras.PlayerFollow].SetActive(true);
    }

    void TurnOffAllCameras()
    {
        foreach (var cam in cams)
        {
            cam.SetActive(false);
        }
    }

}
