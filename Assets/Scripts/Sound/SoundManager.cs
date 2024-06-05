using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField, Tooltip("Extra sound effects")] 
    private Sound[] clickSounds;
    private AudioSource[] sources;
    private bool destroySelf;

    void Start()
    {
        // set variables
        sources = GetComponents<AudioSource>();
        // set destroy self to false
        destroySelf = false;
        // set self to dont destroy on load
        DontDestroyOnLoad(gameObject);
        // subscribe to scene change event
        SceneManager.sceneLoaded += DestroySelf;
    }

    void Update()
    {
        // only check if need to destroy self
        if (!destroySelf) return;
        // check if any audio sources are playing
        foreach (AudioSource source in sources)
        {
            if (source.isPlaying) return;
        }
        // destroy current game object if no sounds are playing
        Destroy(gameObject);
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

    void DestroySelf(Scene scene, LoadSceneMode mode)
    {
        destroySelf = true;
    }
}
