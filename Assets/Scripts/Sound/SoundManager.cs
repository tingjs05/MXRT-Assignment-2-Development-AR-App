using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField, Tooltip("Extra sound effects")] 
    private Sound[] clickSounds;
    private AudioSource[] sources;

    // Start is called before the first frame update
    void Start()
    {
        // set variables
        sources = GetComponents<AudioSource>();
    }

    public void PlayClick(int index)
    {
        // check if there is sound to play
        if (clickSounds == null) { Debug.LogWarning("Audio clips are not provided, process has been terminated!"); return; }
        if (index > (clickSounds.Length - 1)) { Debug.LogWarning("Audio clip of index " + index + " cannot be found, process has been terminated!"); return; }

        // play sound
        foreach (AudioSource source in sources)
        {
            if (source.isPlaying) continue;
            clickSounds[index].Play(source);
            return;
        }

        // play from first audio source if other audio sources are playing
        clickSounds[index].Play(sources[0]);
    }
}
