using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class ExposeMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string ChannelString;

    private string[] validChannels = new string[4] { "MasterVolume", "Ambient", "SFXVolume", "MusicVolume" };

    private void Awake()
    {
        SetChannelLevel(1.0f);
    }


    public void SetChannelLevel(float sliderValue)
    {
        //if the user-defined channel string is in the list of valid channel string then
        if (System.Array.Exists(validChannels, element => element == ChannelString))
        {
            //represents slider with logarithmic scale like it is in the audio mixer system and sets it accordingly
            mixer.SetFloat(ChannelString, Mathf.Log10(sliderValue) * 20);
        }
        else
        {
            Debug.LogWarning("Invalid channel string: " + ChannelString);

        }
    }
}