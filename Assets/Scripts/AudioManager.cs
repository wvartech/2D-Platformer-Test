using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private Dictionary<string,AudioClip> audioClips = new Dictionary<string,AudioClip>();
    private AudioSource audioSource;
    private AudioSource musicSource;
    public static AudioManager Instance { get; private set; }

    private void Awake() {

      if (Instance == null) { Instance = this;} else { Destroy(this.gameObject); }

        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        Debug.Log("Loading clips..");
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips) {
            audioClips.Add(clip.name, clip);            
        }
        PlayBackgroundMusic("bg_music00");

    }

    public void PlaySound(string name) {
        if (audioClips.ContainsKey(name)) {
            audioSource.PlayOneShot(audioClips[name], 1f);
        } else { Debug.Log("Tried to play a clip but it was not found. ."); }
    }

    public void PlayBackgroundMusic(string name) {
        if (audioClips.ContainsKey(name)) {
            musicSource.clip = audioClips[name];
            musicSource.volume = 0.6f;
            musicSource.loop = true;
            musicSource.Play();
        }
        
    }


}
