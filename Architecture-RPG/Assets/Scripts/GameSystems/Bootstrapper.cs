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

    [Header("Other")]
    [SerializeField] private AudioClip mainTheme;
    
    private void Awake()
    {
        ServiceLocator.Register<IAudioService>(new AudioService(source, slider, mixer));
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
        spawner.Construct(enemies, enemiesCount, maxBound, minBound, playerObject.transform);
        spawner.Spawn();
        
        Time.timeScale = 1f;
        //LoadGameScene();
    }

    public void SaveGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        var playerLC = playerObject.GetComponent<PlayerLifecycle>();

        PlayerData data= new PlayerData();

        data.Position= playerObject.transform.position;
        data.Hp=playerLC.GetHealth();

        data.Enemies.Clear();

        MobsLifecycle[] sceneMobs = Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None);

        foreach (var mob in sceneMobs)
        {
            data.Enemies.Add(new EnemySaveData
            {
                Type = mob.gameObject.name,
                Position = mob.transform.position,
                CurrentHp = mob.GetHealth()
            });
        }
        interactor.SaveGame(data);
        Debug.Log($"[Save] Сохранено! Игрок (HP:{data.Hp}) и {data.Enemies.Count} мобов.");
    }
    public void LoadGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        interactor.LoadGame();
        PlayerData data = interactor.Data;

        if (data == null || data.Position == Vector3.zero)
        {
            Debug.LogWarning("Нет данных для загрузки");
            return;
        }

        // --- ЗАГРУЗКА ИГРОКА ---
        playerObject.transform.position = data.Position;
        playerObject.GetComponent<PlayerLifecycle>().RestoreHealth((int)data.Hp);

        // --- ЗАГРУЗКА МОБОВ ---
        MobsLifecycle[] currentMobs = Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None);

        // Для лабы: перемещаем тех мобов, что есть на сцене, на сохраненные позиции
        for (int i = 0; i < currentMobs.Length; i++)
        {
            if (i < data.Enemies.Count)
            {
                currentMobs[i].transform.position = data.Enemies[i].Position;
                currentMobs[i].RestoreHealth((int)data.Enemies[i].CurrentHp);
                currentMobs[i].gameObject.SetActive(true);
            }
            else
            {
                // Если мобов в сейве меньше — отключаем лишних
                currentMobs[i].gameObject.SetActive(false);
            }
        }

        Debug.Log("[Load] Данные восстановлены.");
    }
    
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}