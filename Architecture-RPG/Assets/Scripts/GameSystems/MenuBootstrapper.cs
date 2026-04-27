using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class MenuBootstrapper : MonoBehaviour
{
    [Header("For audio service")]
    [SerializeField] private AudioSource source;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;
    
    [Header("Other")]
    [SerializeField] private AudioClip mainTheme;

    private void Awake()
    {
        ServiceLocator.Register<IAudioService>(new AudioService(source, slider, mixer));
        
        ServiceLocator.Register<ISettingsService>(new SettingsServicePP());
        
        ServiceLocator.Register<ISettingsSaver>(new SettingsControllerSaver());
        
        ServiceLocator.Register<ISettingsLoader>(new SettingsControllerLoader());
        
        ServiceLocator.Get<IAudioService>().PlayMusic(mainTheme);
        GameObject uiBase = new GameObject("Ui controller service");
        UIService uiServ = uiBase.AddComponent<UIService>();
        ServiceLocator.Register<IUIService>(uiServ);
        
        

        Time.timeScale = 1f;

    }
}