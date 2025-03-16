using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;           // For one-shot effects
    public AudioSource footstepsSource;     // Dedicated source for footsteps
    public AudioSource fadeSource;          // Separate source for fading sounds

    [Header("Sound Effects")]
    public AudioClip kickSound;
    public AudioClip bounceSound;
    public AudioClip walkSound;
    public AudioClip whistleSound;
    public AudioClip cheerSound;
    public AudioClip booSound;

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

    public void PlayKickSound()
    {
        if (kickSound != null)
        {
            sfxSource.PlayOneShot(kickSound);
        }
    }

    public void PlayBounceSound()
    {
        if (bounceSound != null)
        {
            sfxSource.PlayOneShot(bounceSound);
        }
    }

    public void PlayWalkSound()
    {
        if (walkSound != null && !footstepsSource.isPlaying)
        {
            footstepsSource.clip = walkSound;
            footstepsSource.Play();
        }
    }

    public void PlayWhistleSound()
    {
        if (whistleSound != null)
        {
            sfxSource.PlayOneShot(whistleSound);
        }
    }

    public void PlayCheerSound()
    {
        StartCoroutine(FadeInAndOut(fadeSource, cheerSound, 1.0f));
    }

    public void PlayBooSound()
    {
        StartCoroutine(FadeInAndOut(fadeSource, booSound, 1.0f));
    }

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

    public void StopFootsteps()
    {
        if (footstepsSource != null)
        {
            footstepsSource.Stop();
        }
    }
}