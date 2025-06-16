using UnityEngine;

public class audioscipt : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource Music;
    public AudioClip steps;
    public AudioClip music;
    public AudioClip chain;
    public AudioClip swallow;
    public AudioClip lever;
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    private void Awake()
    {
        Music.PlayOneShot(music);
    }
}
