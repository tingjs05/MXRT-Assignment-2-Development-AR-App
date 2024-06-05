using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundVariations;

    // Start is called before the first frame update
    void Start()
    {
        if (soundVariations == null)
            Debug.LogError("Audio file(s) need to be provided!");
    }

    public void Play(AudioSource source)
    {
        // randomly play a sound from the array
        source.clip = soundVariations[Random.Range(0, soundVariations.Length)];
        source.Play();
    }
}