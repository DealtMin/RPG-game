using UnityEngine;

public interface IAudioService
{
    void PlaySound(AudioClip clip);
    void PlayMusic(AudioClip track);
    void StopMusic();

    void SetVolumeFromMixer();

    void SetMasterVolume();
    void SetVolumeSettings(float volume);
    float GetVolumeFromMixer();
}