using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioService : IAudioService
{
    private readonly AudioSource _source;
    private readonly Slider _slider;
    private readonly AudioMixer _mixer;

    public AudioService(AudioSource source, Slider controllerSlider, AudioMixer audioMixer)
    {
        _source = source;
        _slider = controllerSlider;
        _mixer = audioMixer;
    } 
    
    public void PlaySound(AudioClip clip) => _source.PlayOneShot(clip);
    
    public void PlayMusic(AudioClip track)
    {
        _source.clip = track;
        _source.Play();
    }
    
    public void StopMusic() => _source.Stop();
    
    
    public void SetVolumeFromMixer()
    {
        float currVolume=GetVolumeFromMixer();
        if (currVolume==-80f)
            _slider.value = 0;
        else
            _slider.value =  Mathf.Lerp(1, 0, currVolume/-20);
    }
    
    public void SetMasterVolume()
    {
        if (_slider.value==0)
            _mixer.SetFloat("musicVolume", -80);
        else    
            _mixer.SetFloat("musicVolume", Mathf.Lerp(-20, 0, _slider.value));
    }

    public void SetVolumeSettings(float volume)
    {
        if (volume==-80f)
            _slider.value = 0;
        else
            _slider.value =  Mathf.Lerp(1, 0, volume/-20);

        SetMasterVolume();
    }

    public float GetVolumeFromMixer()
    {
        _mixer.GetFloat("musicVolume", out float currVolume);
        return currVolume;
    }
}
