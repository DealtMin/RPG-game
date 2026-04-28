using UnityEngine;

public class GameController : MonoBehaviour
{
    private AudioClip victoryMusic;

    private IGameEventService _eventService;
    private IAudioService _audioService;

    public void Construct(AudioClip victoryClip)
    {
        victoryMusic = victoryClip;
    }

    private void Awake()
    {
        _eventService = ServiceLocator.Get<IGameEventService>();
        _audioService = ServiceLocator.Get<IAudioService>();

        _eventService.OnVictoryConditionMet += HandleVictory;
    }

    private void HandleVictory()
    {
        if (victoryMusic != null)
        {
            _audioService.PlayMusic(victoryMusic);
            Debug.Log("=== Победа! Музыка запущена ===");
        }
        else
        {
            Debug.LogWarning("Victory music не задан!");
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