using UnityEngine;

public class SaveSystem : MonoBehaviour, ISaveSystem
{
    private GameObject _playerObject;
    private GameObject[] _enemies;

    public void Construct(GameObject player, GameObject[] enemies)
    {
        _playerObject = player;
        _enemies = enemies;
    }
    
    
    public void SaveGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        var playerLC = _playerObject.GetComponent<PlayerLifecycle>();

        PlayerData data = new PlayerData();
        data.Position = _playerObject.transform.position;
        data.Hp = playerLC.GetHealth();

        // 1. СОХРАНЯЕМ МОБОВ
        data.Enemies.Clear();
        Enemy[] sceneMobs = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var mob in sceneMobs)
        {
            if (mob.GetHealthController().GetHealth() > 0)
            {
                data.Enemies.Add(new EnemySaveData
                {
                    Type = mob.gameObject.name.Replace("(Clone)", "").Trim(),
                    Position = mob.transform.position,
                    CurrentHp = mob.GetHealthController().GetHealth()
                });
            }
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

        interactor.SaveGame(data);
        Debug.Log($"[Save] Сохранено! Живых мобов: {data.Enemies.Count}, Летящей магии: {data.Projectiles.Count}");
    }

    public void LoadGame()
    {
        var interactor = ServiceLocator.Get<GameInteractor>();
        
        PlayerData data = interactor.LoadGame();

        if (data == null || data.Position == Vector3.zero) return;

        // --- ОЧИСТКА ---
        foreach (var m in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None)) Destroy(m.gameObject);
        foreach (var p in Object.FindObjectsByType<MagicAttackBehaivour>(FindObjectsSortMode.None))
            Destroy(p.gameObject);

        // 1. ИГРОК
        _playerObject.transform.position = data.Position;
        _playerObject.GetComponent<PlayerLifecycle>().RestoreHealth((int)data.Hp);

        // Достаем префаб магии игрока для сравнения
        GameObject playerMagicPrefab = _playerObject.GetComponent<PlayerCombat>().MagicAttackPrefab;

        // 2. МОБЫ
        foreach (var savedEnemy in data.Enemies)
        {
            GameObject prefab = System.Array.Find(_enemies, e => e.name == savedEnemy.Type);
            if (prefab != null)
            {
                GameObject newEnemy = Instantiate(prefab, savedEnemy.Position, Quaternion.identity);
                Enemy enemy = newEnemy.GetComponent<Enemy>();
                enemy?.Construct(_playerObject.transform);
                enemy.GetHealthController().RestoreHealth((int)savedEnemy.CurrentHp);
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
                foreach (var ePrefab in _enemies)
                {
                    var ai = ePrefab.GetComponent<Enemy>();
                    if (ai != null && ai.MagicAttackPrefab != null && ai.MagicAttackPrefab.name == pData.Type)
                    {
                        finalPrefab = ai.MagicAttackPrefab;
                        target = _playerObject.transform; // Вражеской магии нужен таргет
                        break;
                    }
                }
            }

            // Спавним шар и запускаем Restore
            if (finalPrefab != null)
            {
                GameObject newBall = Instantiate(finalPrefab, pData.Position, Quaternion.identity);
                newBall.GetComponent<MagicAttackBehaivour>().Restore(target, pData.Direction);
            }
        }

        Debug.Log("[Load] Все данные восстановлены автоматически!");
    }

}
