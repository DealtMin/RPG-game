using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private AudioService _audioService;
    private PlayerController _player;
    private void Awake()
    {
// Инициализация сервисов
        //_audioService = CreateAudioService();
// Создание игрока
        _player = InstantiatePlayer();
// Связывание компонентов
        ConfigureDependencies();
// Переход к основной сцене
        LoadGameScene();
    }
    
    /*
    private AudioService CreateAudioService()
    {
        GameObject obj = new GameObject("AudioService");
        AudioService service = obj.AddComponent<AudioService>();
        DontDestroyOnLoad(obj);
        return service;
    }
  */  
  
    private PlayerController InstantiatePlayer()
    {
        GameObject obj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        return obj.GetComponent<PlayerController>();
    }
    
    private void ConfigureDependencies()
    {
        // _player.Construct(_inputService);
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}