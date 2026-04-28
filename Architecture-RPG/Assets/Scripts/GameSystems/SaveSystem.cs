using UnityEngine;

public class SaveSystem : MonoBehaviour, ISaveSystem
{
    private GameObject _playerObject;
    private GameObject[] _enemies;
    private GameObject[] _allMagicAttacks;

    public void Construct(GameObject player, GameObject[] enemies, GameObject[] allMagicAttacks)
    {
        _playerObject = player;
        _enemies = enemies;
        _allMagicAttacks = allMagicAttacks;
    }
    
    
    public void SaveGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        var playerLC = _playerObject.GetComponent<PlayerLifecycle>();

        

        PlayerData data = new PlayerData();

        data.Position = _playerObject.transform.position;
        data.Hp = playerLC.GetHealth();
        data.Rotation = playerLC.transform.rotation;

        
        var scoreService = ServiceLocator.Get<IScoreService>();
        var eventService = ServiceLocator.Get<IGameEventService>();

        data.Score = scoreService.CurrentScore;
        data.KillCount = eventService.CurrentKillCount;

        
       
        // 1. СОХРАНЯЕМ МОБОВ
        data.Enemies.Clear();
        Enemy[] sceneMobs = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var mob in sceneMobs)
        {
            if (mob.GetHealthController().GetHealth() > 0)
            {
                mob.TryGetComponent(out MultipleMobsWeaponsController weaponsController);
                int weaponInx = -1;
                if (weaponsController) weaponInx = weaponsController.weaponInx;

                data.Enemies.Add(new EnemySaveData
                {
                    Type = mob.gameObject.name.Replace("(Clone)", "").Trim(),
                    Position = mob.transform.position,
                    Rotation = mob.transform.rotation,
                    CurrentHp = mob.GetHealthController().GetHealth(),
                    WeaponIndex = weaponInx
                });
            }
        }
        
        EnemyBoss boss = FindAnyObjectByType<EnemyBoss>();
        if (boss != null)
        {
            data.Enemies.Add(new EnemySaveData
            {
                Type = boss.gameObject.name.Replace("(Clone)", "").Trim(),
                Position = boss.transform.position,
                CurrentHp = boss.GetHealthController().GetHealth()
            });
        }

        // 2. СОХРАНЯЕМ СНАРЯДЫ (Автоматически)
        data.Projectiles.Clear();
        MagicAttackBehaivour[] activeProjectiles =
            Object.FindObjectsByType<MagicAttackBehaivour>(FindObjectsSortMode.None);
        foreach (var p in activeProjectiles)
        {
            data.Projectiles.Add(new ProjectileSaveData
            {
                Type = p.gameObject.name.Replace("(Clone)", "").Trim(),
                Position = p.transform.position,
                Direction = p.transform.forward
            });
        }
        MushroomBallBehaviour[] activeMushroomProjectiles =
            Object.FindObjectsByType<MushroomBallBehaviour>(FindObjectsSortMode.None);
        foreach (var p in activeProjectiles)
        {
            data.Projectiles.Add(new ProjectileSaveData
            {
                Type = p.gameObject.name.Replace("(Clone)", "").Trim(),
                Position = p.transform.position,
                Direction = p.transform.forward
            });
        }

        interactor.SaveGame(data);
        Debug.Log($"[Save] Сохранено! Живых мобов: {data.Enemies.Count}, Летящей магии: {data.Projectiles.Count}");
    }

    public void LoadGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        var settings = ServiceLocator.Get<ISettingsLoader>();
        
        PlayerData data = interactor.LoadGame();

        if (data == null || data.Position == Vector3.zero) return;

        // --- ОЧИСТКА ---
        foreach (var m in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None)) Destroy(m.gameObject);
        foreach (var p in Object.FindObjectsByType<MagicAttackBehaivour>(FindObjectsSortMode.None))
            Destroy(p.gameObject);
        foreach (var p in Object.FindObjectsByType<MushroomBallBehaviour>(FindObjectsSortMode.None))
            Destroy(p.gameObject);
        foreach (var p in Object.FindObjectsByType<EnemyBoss>(FindObjectsSortMode.None)) Destroy(p.gameObject);
        

        // 1. ИГРОК
        _playerObject.transform.position = data.Position;
        _playerObject.transform.rotation = data.Rotation;
        _playerObject.GetComponent<PlayerLifecycle>().RestoreHealth((int)data.Hp);

        var scoreService = ServiceLocator.Get<IScoreService>();
        var eventService = ServiceLocator.Get<IGameEventService>();

       
        scoreService.SetScore(data.Score);

       
        
        eventService.SetKillCount(data.KillCount);

        //eventService.ForceCheck();
        // Достаем префаб магии игрока для сравнения
        GameObject playerMagicPrefab = _playerObject.GetComponent<PlayerCombat>().MagicAttackPrefab;

        // 2. МОБЫ
        foreach (var savedEnemy in data.Enemies)
        {
            GameObject prefab = System.Array.Find(_enemies, e => e.name == savedEnemy.Type);
            if (prefab != null)
            {
                GameObject newEnemy = Instantiate(prefab, savedEnemy.Position, savedEnemy.Rotation);
                if (newEnemy.TryGetComponent(out Enemy enemy))
                {
                    enemy.Construct(_playerObject.transform, settings.LoadPlayMode());
                    enemy.GetHealthController().RestoreHealth((int)savedEnemy.CurrentHp);
                    if (savedEnemy.WeaponIndex!=-1)
                    {
                        enemy.GetComponent<MultipleMobsWeaponsController>().SelectWeapon(savedEnemy.WeaponIndex);
                    }
                }
                else
                {
                    newEnemy.GetComponent<EnemyBoss>().RestoreHealth((int)savedEnemy.CurrentHp);
                }
            }
        }

        // 3. СНАРЯДЫ (Восстановление типов)
        foreach (var pData in data.Projectiles)
        {
            GameObject finalPrefab = null;
            Transform target = null;

            // ПРОВЕРКА: Это магия игрока?
            if (playerMagicPrefab != null && playerMagicPrefab.name == pData.Type)
            {
                finalPrefab = playerMagicPrefab;
                target = null; // Для игрока таргет не нужен
            }
            else
            {
                // ПРОВЕРКА: Если не игрока, ищем во врагах из массива
                foreach (var magic in _allMagicAttacks)
                {
                    if (magic.name == pData.Type)
                    {
                        finalPrefab = magic;
                        target = _playerObject.transform; // Вражеской магии нужен таргет
                        break;
                    }
                }
            }

            // Спавним шар и запускаем Restore
            if (finalPrefab != null)
            {
                GameObject newBall = Instantiate(finalPrefab, pData.Position, Quaternion.identity);
                if (newBall.TryGetComponent(out MagicAttackBehaivour magicBeh))
                {
                    magicBeh.Restore(target, pData.Direction);
                }
                else
                {
                    newBall.GetComponent<MushroomBallBehaviour>().Restore(target, pData.Direction);
                }
            }
        }

        Debug.Log("[Load] Все данные восстановлены автоматически!");
    }

}
