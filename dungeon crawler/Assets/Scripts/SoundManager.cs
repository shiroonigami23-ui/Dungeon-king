using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Clips")]
    public AudioClip swordSwing;
    public AudioClip hitHurt;
    public AudioClip coinPickup;
    public AudioClip dash;

    private AudioSource source;

    void Awake()
    {
        if (Instance == null) Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }
}