using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip gunfireSound;
    public AudioClip zombieDeathSound;
    public AudioClip zombieGroanSound;
    public AudioClip playerHurtSound;

    void Awake()
    {
        // Singleton pattern - only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create music source if it does not exist
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = 0.3f;
        }

        // Create SFX source if it does not exist
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = 0.5f;
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlaySFX(string soundName)
    {
        AudioClip clip = null;

        switch (soundName)
        {
            case "Gunfire":
                clip = gunfireSound;
                break;
            case "ZombieDeath":
                clip = zombieDeathSound;
                break;
            case "ZombieGroan":
                clip = zombieGroanSound;
                break;
            case "PlayerHurt":
                clip = playerHurtSound;
                break;
        }

        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
