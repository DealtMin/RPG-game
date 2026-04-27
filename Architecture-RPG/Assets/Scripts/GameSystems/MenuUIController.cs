using TMPro;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private float uiScaleFactor = 1.07f;
    [SerializeField] private TMP_Dropdown dropdown;
    private IAudioService _audio;
    private IUIService _uiService;
    private ISettingsSaver _settings;
    private ISettingsLoader _loader;
    

    private void Awake()
    {
        _audio = ServiceLocator.Get<IAudioService>();
        _uiService = ServiceLocator.Get<IUIService>();
        _audio.SetVolumeFromMixer();
        _settings = ServiceLocator.Get<ISettingsSaver>();
        _loader =ServiceLocator.Get<ISettingsLoader>();
        _loader.LoadAllSettings();
        dropdown.value = _loader.GetPlayModeIndex();

    }
    
    public void SetPlayModeIndex()
    {
        _settings.SetPlayModeIndex(dropdown.value);
    }

    public void LoadGame()
    {
        _settings.SaveAllSettings();
        _uiService.OpenSceneByName("Main");
    }

    public void OpenUIPanel(GameObject panel)
    {
        _uiService.ShowHideElement(settingsPanel, settingsPanel==panel);
        _uiService.ShowHideElement(mainPanel, mainPanel==panel);
    }
    
    public void OnHoverEnter(Transform obj)
    {
        _uiService.IncreaseScale(obj, uiScaleFactor);
    }
    
    public void OnHoverExit(Transform obj)
    {
        _uiService.DecreaseScale(obj, uiScaleFactor);
    }

    public void PlaySound(AudioClip clip)
    {
        _audio.PlaySound(clip);
    }

    public void SetVolumeBySlider()
    {
        _audio.SetMasterVolume();
    }
}
