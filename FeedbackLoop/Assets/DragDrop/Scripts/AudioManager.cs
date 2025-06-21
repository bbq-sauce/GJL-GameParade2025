using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgmMedievalLoop;
    [SerializeField] private AudioClip sfxDragDrop;
    [SerializeField] private AudioClip sfxPotionBrew;
    [SerializeField] private AudioClip sfxFireRoar;
    [SerializeField] private AudioClip sfxBoiling;
    [SerializeField] private AudioClip sfxPages;
    [SerializeField] private AudioClip sfxMagicAura;
    [SerializeField] private AudioClip sfxMorningBirds;
    [SerializeField] private AudioClip sfxNight;
    [SerializeField] private AudioClip sfxKingArrival;
    [SerializeField] private AudioClip sfxWarHorn;
    [SerializeField] private AudioClip sfxKingAngry;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBGM();
    }

    // Play Background Music
    public void PlayBGM()
    {
        if (audioSource && bgmMedievalLoop)
        {
            audioSource.clip = bgmMedievalLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Drag-drop sound
    public void PlayDragDrop() => PlaySound(sfxDragDrop);

    // Task-based SFX
    public void PlayPotionBrewTask()
    {
        PlaySound(sfxPotionBrew);
        PlaySound(sfxFireRoar, 0.2f);
    }

    public void PlayOilTask()
    {
        PlaySound(sfxBoiling);
        PlaySound(sfxFireRoar, 0.2f);
    }

    public void PlayTomeTask()
    {
        PlaySound(sfxPages);
        PlaySound(sfxMagicAura, 0.2f);
    }


    // Events
    public void PlayKingArrival() => PlaySound(sfxKingArrival);
    public void PlayWarHorn() => PlaySound(sfxWarHorn);
    public void PlayKingAngry() => PlaySound(sfxKingAngry);

    public void PlaySound(AudioClip clip, float delay = 0f)
    {
        if (clip == null || audioSource == null) return;

        if (delay <= 0f)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            StartCoroutine(PlayDelayed(clip, delay));
        }
    }

    private System.Collections.IEnumerator PlayDelayed(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
