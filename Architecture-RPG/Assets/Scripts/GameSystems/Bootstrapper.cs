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

        PlayerData data = new PlayerData();
        data.Position = playerObject.transform.position;
        data.Hp = playerLC.GetHealth();

        data.Enemies.Clear();

        MobsLifecycle[] sceneMobs = Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None);

        foreach (var mob in sceneMobs)
        {
            data.Enemies.Add(new EnemySaveData
            {
                // ОЧЕНЬ ВАЖНО: сохраняем чистое имя без "(Clone)"
                Type = mob.gameObject.name.Replace("(Clone)", "").Trim(),
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

        // 1. Восстанавливаем игрока
        playerObject.transform.position = data.Position;
        playerObject.GetComponent<PlayerLifecycle>().RestoreHealth((int)data.Hp);

        // 2. ОЧИСТКА СЦЕНЫ
        // Находим всех мобов, которые заспавнились случайно при старте, и удаляем их
        MobsLifecycle[] currentMobs = Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None);
        foreach (var m in currentMobs)
        {
            Destroy(m.gameObject);
        }

        // 3. ВОССТАНОВЛЕНИЕ ИЗ СОХРАНЕНИЯ
        // Теперь создаем только тех мобов, которые были в списке сохранения
        foreach (var savedEnemy in data.Enemies)
        {
            // Ищем нужный префаб в массиве enemies по имени
            GameObject prefab = System.Array.Find(enemies, e => e.name == savedEnemy.Type);

            if (prefab != null)
            {
                // Создаем моба
                GameObject newEnemy = Instantiate(prefab, savedEnemy.Position, Quaternion.identity);
                
                // Настраиваем его AI (так же, как это делал спавнер)
                EnemyAI enemyai = newEnemy.GetComponent<EnemyAI>();
                if (enemyai != null) enemyai.Construct(playerObject.transform);

                // Восстанавливаем ему ХП
                MobsLifecycle lifecycle = newEnemy.GetComponent<MobsLifecycle>();
                if (lifecycle != null) lifecycle.RestoreHealth((int)savedEnemy.CurrentHp);
            }
        }

        Debug.Log($"[Load] Сцена очищена. Восстановлено мобов из сейва: {data.Enemies.Count}");
    }
    
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}