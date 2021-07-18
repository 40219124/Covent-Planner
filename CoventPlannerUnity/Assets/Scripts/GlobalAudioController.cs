using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioController : MonoBehaviour
{

    private Camera camera;

    public AudioSource[] AmbientChannels;
    public AudioSource[] SFXChannels;
    public AudioSource[] MusicChannels;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    private void OnEnable()
    {
        BattleManager.CardPlayedEvent += PlayBattleOutcome;
    }

    //battlemanager.instance.opponent
    public void PlayBattleOutcome(int successCode)
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.transform.position;
    }

    public void PlayAmbient(AudioClip clip)
    {
        foreach (AudioSource channel in AmbientChannels)
        {
            if (!channel.isPlaying)
            {
                channel.clip = clip;
                channel.Play();
            }
        }
    }


    public void PlaySFX(AudioClip clip)
    {
        foreach (AudioSource channel in AmbientChannels)
        {
            if (!channel.isPlaying)
            {
                channel.clip = clip;
                channel.Play();
            }
        }
    }


    public void PlayMusic(AudioClip clip)
    {
        foreach (AudioSource channel in AmbientChannels)
        {
            if (!channel.isPlaying)
            {
                channel.clip = clip;
                channel.Play();
            }
        }
    }
}
