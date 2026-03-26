using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuBootstrapper : MonoBehaviour
{
    [Header("For audio service")] [SerializeField]
    private AudioSource source;

    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        ServiceLocator.Register<IAudioService>(new AudioService(source, slider, mixer));
        GameObject uiBase = new GameObject("Ui controller service");
        UIService uiServ = uiBase.AddComponent<UIService>();
        ServiceLocator.Register<IUIService>(uiServ);
        // Saver

        Time.timeScale = 1f;

    }
}