using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shootTimer;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private float uiScaleFactor=1.07f;
    private IAudioService _audio;
    private IUIService _uiService;
    private PlayerLifecycle _playerLifeCycle;
    private ISaveSystem _saver;
    private void Start()
    {
        _playerLifeCycle = GetComponent<PlayerLifecycle>();
        _playerLifeCycle.TakeDamage += ReduceHealth;
        _audio = ServiceLocator.Get<IAudioService>();
        _uiService = ServiceLocator.Get<IUIService>();
        _saver = ServiceLocator.Get<ISaveSystem>();
        ServiceLocator.Get<ISettingsLoader>().LoadAllSettings();
    }

    public void Pause(bool pauseOn)
    {
        _uiService.ShowHideElement(pausePanel, pauseOn);
    }

    public void GoToMenu()
    {
        _uiService.OpenSceneByName("Menu");
    }
    
    public void ReloadGame()
    {
        _uiService.OpenSceneByName("Main");
    }

    public void SaveData()
    {
        _saver.SaveGame();
    }


    public void LoadData()
    {
        _saver.LoadGame();
    }
    
  
    public void OnHoverEnter(Transform obj)
    {
        _uiService.IncreaseScale(obj, uiScaleFactor);
    }
    
    public void OnHoverExit(Transform obj)
    {
        _uiService.DecreaseScale(obj, uiScaleFactor);
    }
    
    public void Death()
    {
        _uiService.ShowHideElement(pausePanel, false);
        _uiService.ShowHideElement(deathPanel, true);
        Time.timeScale = 0f;
    }
    
    public void ReduceHealth(int health, int _maxHealth)
    {
        _uiService.SetFillAmountImage(healthBar, health, _maxHealth);
        _uiService.SetTMPRoText(hpText, health);
    }
    
    public void MagicTimerUI(float coolDown)
    {
        _uiService.SetFillAmountImage(shootTimer, 0, 1);
        _uiService.StartFillCoroutine(shootTimer, coolDown);
    }
    
    public void PlaySound(AudioClip clip)
    {
        _audio.PlaySound(clip);
    }
    
    public void SetVolumeBySlider()
    {
        _audio.SetMasterVolume();
    }
    void OnDestroy()
    {
        _playerLifeCycle.TakeDamage -= ReduceHealth;
    }
}
