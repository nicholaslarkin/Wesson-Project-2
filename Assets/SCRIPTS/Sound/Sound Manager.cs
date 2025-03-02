using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;   // For sound effects
    public AudioSource musicSource; // For background music
    public AudioClip[] soundEffects; // Assign sounds in Inspector
    public AudioClip[] musicTracks;  // Assign music in Inspector

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }
    public void FadeOutAndStop(AudioSource audioSource, float fadeDuration)
    {
        if (audioSource == null) return;
        StartCoroutine(FadeOutSound(audioSource, fadeDuration));
    }

    private IEnumerator FadeOutSound(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * (Time.deltaTime / fadeDuration);
            yield return null;
        }

        audioSource.Stop();

        // Only destroy if it's a dynamically created AudioSource
        if (audioSource.gameObject.name.StartsWith("SFX_"))
        {
            Destroy(audioSource.gameObject);
        }
    }

    public AudioSource PlaySFXWithSource(int index, float volume = 1f)
    {
        if (index < 0 || index >= soundEffects.Length)
        {
            Debug.LogError("Invalid SFX index: " + index);
            return null;
        }

        // Create a temporary AudioSource
        GameObject sfxObject = new GameObject("SFX_" + soundEffects[index].name);
        AudioSource audioSource = sfxObject.AddComponent<AudioSource>();

        audioSource.clip = soundEffects[index];
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy the object after the clip finishes playing
        Destroy(sfxObject, soundEffects[index].length);

        return audioSource;
    }

    // Play a specific sound effect by index
    public void PlaySFX(int index, float volume = 1f)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            Debug.Log($"Playing SFX: {soundEffects[index].name} at volume {volume}");
            sfxSource.PlayOneShot(soundEffects[index], volume);
        }
    }

    // Play a specific music track by index
    public void PlayMusic(int index, float volume = 1f)
    {
        if (index >= 0 && index < musicTracks.Length)
        {
            musicSource.clip = musicTracks[index];
            musicSource.volume = volume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Stop current music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
