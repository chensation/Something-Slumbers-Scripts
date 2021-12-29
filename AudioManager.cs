using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private AssetStore _assetStore;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        _assetStore = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetStore>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonClickSound()
    {
        _audioSource.PlayOneShot(_assetStore.ButtonClick);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
