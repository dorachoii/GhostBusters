using System;
using UnityEngine;

public class TutorialItem : MonoBehaviour
{
    public static event Action<string> OnItemDestroyed;
    public string itemId;

    void OnDestroy()
    {
        OnItemDestroyed?.Invoke(itemId);        
    }
}
