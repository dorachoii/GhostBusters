using UnityEngine;

public class TutorialItemManager : MonoBehaviour
{
    public GameObject[] items;

    [SerializeField]
    private TutorialController controller;

    void OnEnable()
    {
        controller.onPhaseChanged += HandleItem;
    }

    void OnDisable()
    {
        controller.onPhaseChanged -= HandleItem;
    }

    public void HandleItem(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.MoveTutorial:
                items[0].SetActive(true);
                break;
            case TutorialPhase.BlowTutorial:
                items[1].SetActive(true);
                break;
        }        
    }
}
