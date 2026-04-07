using System;
using UnityEngine;

public class SettingsControllerSaver : ISettingsSaver
{
    private ISettingsService _settingsService;
    private string _playModeString = "PlayMode";
    private string _audioString = "AudioVolume";
    private IAudioService _audio;
    private int _playModeIndex=0;

    public void SetPlayModeIndex(int index)
    {
        _playModeIndex = index;
    }

    public SettingsControllerSaver()
    {
        _settingsService = ServiceLocator.Get<ISettingsService>();
        _audio = ServiceLocator.Get<IAudioService>();
        
    }

    public void SaveAllSettings()
    {
        SaveAudioVolume();
        SavePlayMode();
    }
    
    private void SavePlayMode()
    {
        _settingsService.SaveSettingsInt(_playModeString, _playModeIndex);
    }

    private void SaveAudioVolume()
    {
        float volume = _audio.GetVolumeFromMixer();
        _settingsService.SaveSettingsFloat(_audioString, volume);
    }
}
