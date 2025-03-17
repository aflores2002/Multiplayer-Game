using UnityEngine;
using System.Collections;

// Manages audio playback for various sound effects in the game
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;       // For one-shot effects
    public AudioSource footstepsSource; // Dedicated source for footsteps
    public AudioSource fadeSource;      // Separate source for fading sounds

    [Header("Sound Effects")]
    public AudioClip kickSound;
    public AudioClip bounceSound;
    public AudioClip walkSound;
    public AudioClip whistleSound;
    public AudioClip cheerSound;
    public AudioClip booSound;
    public AudioClip topPostSound;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Create audio sources if not assigned
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }

            if (footstepsSource == null)
            {
                footstepsSource = gameObject.AddComponent<AudioSource>();
                footstepsSource.playOnAwake = false;
                footstepsSource.volume = 0.7f;
            }

            if (fadeSource == null)
            {
                fadeSource = gameObject.AddComponent<AudioSource>();
                fadeSource.playOnAwake = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Plays the kick sound effect
    public void PlayKickSound()
    {
        if (kickSound != null)
        {
            sfxSource.PlayOneShot(kickSound);
        }
    }

    // Plays the bounce sound effect
    public void PlayBounceSound()
    {
        if (bounceSound != null)
        {
            sfxSource.PlayOneShot(bounceSound);
        }
    }

    // Plays the walk sound effect if not already playing
    public void PlayWalkSound()
    {
        if (walkSound != null && !footstepsSource.isPlaying)
        {
            footstepsSource.clip = walkSound;
            footstepsSource.Play();
        }
    }

    // Plays the whistle sound effect
    public void PlayWhistleSound()
    {
        if (whistleSound != null)
        {
            sfxSource.PlayOneShot(whistleSound);
        }
    }

    // Plays the cheer sound with fade in and out
    public void PlayCheerSound()
    {
        StartCoroutine(FadeInAndOut(fadeSource, cheerSound, 1.0f));
    }

    // Plays the boo sound with fade in and out
    public void PlayBooSound()
    {
        StartCoroutine(FadeInAndOut(fadeSource, booSound, 1.0f));
    }

    // Coroutine to fade in and out an audio clip
    private IEnumerator FadeInAndOut(AudioSource audioSource, AudioClip clip, float fadeDuration)
    {
        audioSource.clip = clip;
        audioSource.volume = 0;
        audioSource.Play();

        // Fade in
        float fadeInTime = fadeDuration / 2;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / fadeInTime;
            yield return null;
        }

        // Fade out
        float fadeOutTime = fadeDuration / 2;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / fadeOutTime;
            yield return null;
        }

        audioSource.Stop();
    }

    // Stops the footsteps sound
    public void StopFootsteps()
    {
        if (footstepsSource != null)
        {
            footstepsSource.Stop();
        }
    }

    // Plays the top post sound effect
    public void PlayTopPostSound()
    {
        if (topPostSound != null)
        {
            sfxSource.PlayOneShot(topPostSound);
        }
    }
}