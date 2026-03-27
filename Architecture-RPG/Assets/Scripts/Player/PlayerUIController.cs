using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
    private void Start()
    {
        _playerLifeCycle = GetComponent<PlayerLifecycle>();
        _playerLifeCycle.PlayerTakeDamage += ReduceHealth;
        _audio = ServiceLocator.Get<IAudioService>();
        _uiService = ServiceLocator.Get<IUIService>();
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
        
    }


    public void LoadData()
    {
        
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
        Time.timeScale = 0f;
        _uiService.ShowHideElement(pausePanel, false);
        _uiService.ShowHideElement(deathPanel, true);
    }
    
    public void ReduceHealth(int health)
    {
        _uiService.SetFillAmountImage(healthBar, health);
        _uiService.SetTMPRoText(hpText, health);
    }
    
    public void MagicTimerUI(float coolDown)
    {
        _uiService.SetFillAmountImage(shootTimer, 0);
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
        _playerLifeCycle.PlayerTakeDamage -= ReduceHealth;
    }
}
