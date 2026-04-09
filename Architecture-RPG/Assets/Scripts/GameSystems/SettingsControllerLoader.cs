using System;
using UnityEditorInternal;
using UnityEngine;

public class SettingsControllerLoader : ISettingsLoader
{
    private ISettingsService _settingsService;
    private string _playModeString = "PlayMode";
    private string _audioString = "AudioVolume";
    private IAudioService _audio;
    private int _playModeIndex;

    public SettingsControllerLoader()
    {
        _settingsService = ServiceLocator.Get<ISettingsService>();
        _audio = ServiceLocator.Get<IAudioService>();
    }

    public int GetPlayModeIndex()
    {
        return _playModeIndex;
    }
    
    public void LoadAllSettings()
    {
        LoadAudioVolume();
        LoadPlayMode();
    }
    
    private void LoadAudioVolume()
    {
        float volume = _settingsService.LoadSettingsFloat(_audioString);
        _audio.SetVolumeSettings(volume);
    }

    public int LoadPlayMode()
    {
        _playModeIndex = _settingsService.LoadSettingsInt(_playModeString);
        return _playModeIndex;
    }

    
}
