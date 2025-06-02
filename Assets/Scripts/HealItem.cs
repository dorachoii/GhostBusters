using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20;
    private bool used = false;

    public void Heal(PlayerStats stats)
    {
        if (used) return;
        
        stats.Heal(healAmount);
        used = true;
        Destroy(gameObject, 1);
    }
}
