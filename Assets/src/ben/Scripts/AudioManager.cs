using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//PATTERN: Singleton
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip startMusic;
    public AudioClip loopedMusic;
    public AudioClip cursorMove;
    public AudioClip select;
    public AudioClip back;
    public AudioClip placed;
    public AudioClip death;
    public AudioClip error;

    private bool pauseMusic = false;

    // Ensures there is only one instance of AudioManager
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = startMusic;
        musicSource.Play();

        // Gets the stored volume settings from the player's preferences
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
        if (clip == death)
        {
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
                pauseMusic = true;
            }

        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        if (!sfxSource.isPlaying)
        {
            pauseMusic = false;
        }
        if (!musicSource.isPlaying && !pauseMusic)
        {
            // Switch to the second song
            musicSource.clip = loopedMusic;
            musicSource.Play();
        }
        //Conditionals for stopping the music for the victory and death sfxs

    }

    public bool SFXisPlaying()
    {
        return sfxSource.isPlaying;
    }

    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
        Debug.Log(musicSource.volume);
    }
}

