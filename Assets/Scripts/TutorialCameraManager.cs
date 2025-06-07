using System.Collections;
using UnityEngine;

/// <summary>
/// チュートリアルで使用するカメラの種類を定義
/// </summary>

public enum Cameras
{
    Intro,          
    PlayerFollow,   
    Item_Lobstar,   
    Item_pizzashop, 
    Outro           
}

/// <summary>
/// チュートリアル中のカメラ切り替えを管理するクラスです。
/// Cinemachineを使用して、各フェーズに応じたカメラワークを制御します。
/// </summary>

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


    /// フェーズに応じてカメラを切り替え
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
