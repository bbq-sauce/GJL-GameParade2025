using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;

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
        if (bgmSource && bgmMedievalLoop)
        {
            bgmSource.clip = bgmMedievalLoop;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // Drag-drop sound
    public void PlayDragDrop() => PlaySFX(sfxDragDrop);

    // Task-based SFX
    public void PlayPotionBrewTask()
    {
        PlaySFX(sfxPotionBrew);
        PlaySFX(sfxFireRoar, 0.2f);
    }

    public void PlayOilTask()
    {
        PlaySFX(sfxBoiling);
        PlaySFX(sfxFireRoar, 0.2f);
    }

    public void PlayTomeTask()
    {
        PlaySFX(sfxPages);
        PlaySFX(sfxMagicAura, 0.2f);
    }

    // Time-based ambience
    public void PlayMorningAmbience()
    {
        PlayAmbience(sfxMorningBirds);
    }

    public void PlayNightAmbience()
    {
        PlayAmbience(sfxNight);
    }

    // Events
    public void PlayKingArrival() => PlaySFX(sfxKingArrival);
    public void PlayWarHorn() => PlaySFX(sfxWarHorn);
    public void PlayKingAngry() => PlaySFX(sfxKingAngry);

    private void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    private void PlayAmbience(AudioClip clip)
    {
        if (clip != null && ambienceSource != null)
        {
            ambienceSource.clip = clip;
            ambienceSource.loop = true;
            ambienceSource.Play();
        }
    }
}
