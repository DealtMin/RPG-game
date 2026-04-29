using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class Bootstrapper : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerObject;
    
    [Header("For audio service")]
    [SerializeField] private AudioSource source;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;
    
    [Header("For enemy spawner")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int[] enemiesCount;
    [SerializeField] private Transform maxBound;
    [SerializeField] private Transform minBound;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Other")]
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip victoryMusic;

    [SerializeField] private GameObject[] magicBalls;
    
    private void Awake()
    {
        ServiceLocator.Register<IAudioService>(new AudioService(source, slider, mixer));
        
        ServiceLocator.Register<ISettingsService>(new SettingsServicePP());
        
        ServiceLocator.Register<ISettingsSaver>(new SettingsControllerSaver());
        
        ServiceLocator.Register<ISettingsLoader>(new SettingsControllerLoader());

        ServiceLocator.Register<IGameEventService>(new GameEventService());

        ServiceLocator.Register<IScoreService>(new ScoreService());
        
        
        GameObject controllerObj = new GameObject("[Controller] GameController");
        GameController controller = controllerObj.AddComponent<GameController>();
        controller.Construct(mainTheme, victoryMusic);
        
        
        ServiceLocator.Get<IAudioService>().PlayMusic(mainTheme);
        
        GameObject uiBase = new GameObject("Ui controller service");
        UIService uiServ = uiBase.AddComponent<UIService>();
        ServiceLocator.Register<IUIService>(uiServ);
        // Saver
        GameRepository repository = new GameRepository();
        GameInteractor saveInteractor = new GameInteractor(repository);

        // 3. Регистрируем интерактор в ServiceLocator, чтобы UI мог его найти
        ServiceLocator.Register<GameInteractor>(saveInteractor);

        GameObject spawnerObj = new GameObject("EnemiesSpawner");
        EnemySpawner spawner = spawnerObj.AddComponent<EnemySpawner>();
        spawner.Construct(enemies[..^1], enemiesCount, maxBound, minBound, playerObject.transform, bossSpawnPoint, bossPrefab );
        ServiceLocator.Register<EnemySpawner>(spawner);
        spawner.Spawn();

        GameObject saverObj = new GameObject("Saver");
        SaveSystem saver = saverObj.AddComponent<SaveSystem>();
        saver.Construct(playerObject, enemies, magicBalls);
        ServiceLocator.Register<ISaveSystem>(saver);
        
        
        Time.timeScale = 1f;
        //LoadGameScene();
    }


    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}