using UnityEngine;

public class AudioPlayer_BreakSound : MonoBehaviour
{
    public GameObject breakSound;

    public void MakeDestroySound()
    {
        GameObject sfx = Instantiate(breakSound, transform.position, Quaternion.identity);
        Destroy(sfx, sfx.GetComponent<AudioSource>().clip.length);
    }
}
