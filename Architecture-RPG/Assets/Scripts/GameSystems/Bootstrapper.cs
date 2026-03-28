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

    // 1. Сохраняем мобов
    data.Enemies.Clear();
    MobsLifecycle[] sceneMobs = Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None);
    foreach (var mob in sceneMobs)
    {
        if (mob.GetHealth() > 0)
        {
            data.Enemies.Add(new EnemySaveData {
                Type = mob.gameObject.name.Replace("(Clone)", "").Trim(),
                Position = mob.transform.position,
                CurrentHp = mob.GetHealth()
            });
        }
    }

    // 2. СОХРАНЯЕМ СНАРЯДЫ (с направлением)
    data.Projectiles.Clear();
    MagicAttackBehaivour[] activeProjectiles = Object.FindObjectsByType<MagicAttackBehaivour>(FindObjectsSortMode.None);
    foreach (var p in activeProjectiles)
    {
        data.Projectiles.Add(new ProjectileSaveData {
            Position = p.transform.position,
            Direction = p.transform.forward // Сохраняем куда он летел в этот момент
        });
    }

    interactor.SaveGame(data);
}

public void LoadGame()
{
    var interactor = ServiceLocator.Get<GameInteractor>();
    interactor.LoadGame();
    PlayerData data = interactor.Data;

    if (data == null || data.Position == Vector3.zero) return;

    // Очистка
    foreach (var m in Object.FindObjectsByType<MobsLifecycle>(FindObjectsSortMode.None)) Destroy(m.gameObject);
    foreach (var p in Object.FindObjectsByType<MagicAttackBehaivour>(FindObjectsSortMode.None)) Destroy(p.gameObject);

    // 1. Игрок
    playerObject.transform.position = data.Position;
    playerObject.GetComponent<PlayerLifecycle>().RestoreHealth((int)data.Hp);

    // 2. Мобы
    foreach (var savedEnemy in data.Enemies)
    {
        GameObject prefab = System.Array.Find(enemies, e => e.name == savedEnemy.Type);
        if (prefab != null)
        {
            GameObject newEnemy = Instantiate(prefab, savedEnemy.Position, Quaternion.identity);
            newEnemy.GetComponent<EnemyAI>()?.Construct(playerObject.transform);
            newEnemy.GetComponent<MobsLifecycle>()?.RestoreHealth((int)savedEnemy.CurrentHp);
        }
    }

    // 3. СНАРЯДЫ (Восстановление полета)
    foreach (var pData in data.Projectiles)
    {
        // Находим префаб шара у первого попавшегося Range врага в массиве enemies
        GameObject ballPrefab = null;
        foreach(var e in enemies) {
            var ai = e.GetComponent<EnemyAI>();
            if(ai != null && ai.MagicAttackPrefab != null) {
                ballPrefab = ai.MagicAttackPrefab;
                break;
            }
        }

        if (ballPrefab != null)
        {
            // Спавним шар
            GameObject newBall = Instantiate(ballPrefab, pData.Position, Quaternion.identity);
            
            // ВАЖНО: вызываем Restore вместо Construct!
            // Передаем сохраненный вектор направления
            newBall.GetComponent<MagicAttackBehaivour>().Restore(playerObject.transform, pData.Direction);
        }
    }
}
    
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}