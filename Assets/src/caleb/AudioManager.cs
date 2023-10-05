using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = startMusic;
        musicSource.Play();
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


    // Update is called once per frame
    void Update()
    {
        if(!sfxSource.isPlaying) 
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
}
