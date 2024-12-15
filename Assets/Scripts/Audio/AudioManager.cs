using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header(" Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header(" Audio Clips")]
    public AudioClip background;
    public AudioClip ShootSound;

    private void Awake()
    {
       
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject); 
        }

        instance = this; 
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
