using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioController : MonoBehaviour
{

    private Camera camera;

    public AudioSource[] AmbientChannels;
    public AudioSource[] SFXChannels;
    public AudioSource[] MusicChannels;

    public AudioClip[] BattleResponseClips;
    public AudioClip[] AmbientClips;
    public AudioClip[] MusicClips;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        PlayAmbient(AmbientClips[0]);
    }

    private void OnEnable()
    {
        BattleManager.CardPlayedEvent += PlayBattleOutcome;
    }

    private void OnDisable()
    {
        BattleManager.CardPlayedEvent -= PlayBattleOutcome;
    }

    //battlemanager.instance.opponent
    public void PlayBattleOutcome(int successCode)
    {
        PlaySFX(BattleResponseClips[successCode]);

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
                break;
            }
        }
    }


    public void PlaySFX(AudioClip clip)
    {
        foreach (AudioSource channel in SFXChannels)
        {
            if (!channel.isPlaying)
            {
                channel.clip = clip;
                channel.Play();
                break;
            }
        }
    }


    public void PlayMusic(AudioClip clip)
    {
        foreach (AudioSource channel in MusicChannels)
        {
            if (!channel.isPlaying)
            {
                channel.clip = clip;
                channel.Play();
                break;
            }
        }
    }
}
