using UnityEngine;

public class GameController : MonoBehaviour
{
    
    private AudioClip _mainTheme;
    private AudioClip _victoryMusic;

    private IGameEventService _eventService;
    private IAudioService _audioService;

    
    public void Construct(AudioClip mainThemeClip, AudioClip victoryClip)
    {
        _mainTheme = mainThemeClip;
        _victoryMusic = victoryClip;
       
    }

    private void Awake()
    {
        _eventService = ServiceLocator.Get<IGameEventService>();
        _audioService = ServiceLocator.Get<IAudioService>();

        _eventService.OnVictoryConditionMet += HandleVictory;
    }

    private void Start()
    {
  
        if (_mainTheme != null)
        {
            _audioService.PlayMusic(_mainTheme);
            Debug.Log($"GameController.Start(): Playing main theme: {_mainTheme.name}");
        }
        else
        {
            Debug.LogWarning("Main Theme Audio Clip не был передан в Construct GameController!");
        }
    }

    public void RestoreMusicState(int currentKillCount)
    {
        _audioService.StopMusic(); 
        
        if (_mainTheme != null)
        {
            _audioService.PlayMusic(_mainTheme); 
            Debug.Log("Музыка восстановлена: проигрывается основная тема.");
        }
        else
        {
            Debug.LogWarning("Main Theme Audio Clip отсутствует при попытке восстановить музыку.");
        }
    }

    private void HandleVictory()
    {
        if (_victoryMusic != null)
        {
            _audioService.StopMusic(); 
            _audioService.PlayMusic(_victoryMusic);
            Debug.Log("=== Победа! Музыка запущена ===");
        }
        else
        {
            Debug.LogWarning("Victory music не задан в конструкторе GameController!");
        }
    }

    private void OnDestroy()
    {
        if (_eventService != null)
        {
            _eventService.OnVictoryConditionMet -= HandleVictory;
        }
    }
}