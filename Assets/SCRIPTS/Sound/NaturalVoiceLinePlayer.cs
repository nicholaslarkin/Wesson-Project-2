using UnityEngine;
using System.Collections;

public class NaturalVoiceLinePlayer : MonoBehaviour
{
    public AudioClip[] voiceLines; // Array of voice lines
    public float minInterval = 40f; // Minimum time between voice lines
    public float maxInterval = 120f; // Maximum time between voice lines

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing! Please add one to the GameObject.");
            return;
        }

        if (voiceLines.Length == 0)
        {
            Debug.LogError("No voice lines assigned! Add audio clips to the voiceLines array.");
            return;
        }

        // Start the coroutine to play voice lines naturally
        StartCoroutine(PlayVoiceLinesNaturally());
    }

    private IEnumerator PlayVoiceLinesNaturally()
    {
        while (true)
        {
            // Wait for a random interval
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Play a random voice line
            PlayRandomVoiceLine();
        }
    }

    private void PlayRandomVoiceLine()
    {
        if (audioSource.isPlaying) return; // Avoid interrupting a playing clip

        int randomIndex = Random.Range(0, voiceLines.Length);
        audioSource.clip = voiceLines[randomIndex];
        audioSource.Play();
    }
}
