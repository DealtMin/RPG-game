using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[] _enemies;
    private int[] _enemiesCount;
    private Transform _maxBound;
    private Transform _minBound;

    private Transform _playerTransform;

    private GameObject bossPrefab;
    private Transform _bossSpawnPoint;
    private IGameEventService _eventService;
    private bool _bossSpawned = false;
    public void Construct(GameObject[] enemies, int[] enemiesCount, Transform maxBound, Transform minBound, Transform playerTransform, Transform bossSpawnPoint, GameObject bossPrefab)
    {
        _enemies = enemies;
        _enemiesCount = enemiesCount;
        _maxBound = maxBound;
        _minBound = minBound;
        _bossSpawnPoint = bossSpawnPoint;
        this.bossPrefab = bossPrefab;

        _eventService = ServiceLocator.Get<IGameEventService>();
        _eventService.OnBossShouldSpawn += SpawnBoss;
        
    }
    

    public void Spawn()
    {
        var settings = ServiceLocator.Get<ISettingsLoader>();
        if (_playerTransform == null)
        {
            _playerTransform = FindAnyObjectByType<PlayerController>().transform;
        }
        for (int i=0; i<_enemies.Length; i++)
        {
            for (int j = 0; j < _enemiesCount[i]; j++)
            {
                Vector3 newpos = RandomPosition(_minBound, _maxBound);
                
                GameObject newEnemy = Instantiate(_enemies[i], newpos, Quaternion.identity);
                Enemy enemy = newEnemy.GetComponent<Enemy>();
                enemy.Construct(_playerTransform, settings.LoadPlayMode());
                if (enemy.TryGetComponent(out MultipleMobsWeaponsController weaponsController))
                {
                    weaponsController.SelectWeapon(-1);
                }
            }
        }
    }

    private void SpawnBoss()
    {
        if (_bossSpawned || bossPrefab == null) return;

        _bossSpawned = true;
        //случайный спавн
        //Vector3 spawnPos = RandomPosition(_minBound, _maxBound);


        Instantiate(bossPrefab, _bossSpawnPoint.position, Quaternion.identity);
        Debug.Log("Босс заспавнен!");
    }


    Vector3 RandomPosition(Transform minBound, Transform maxBound)
    {
        float x = RandomBetween(minBound.position.x, maxBound.position.x);
        float y = RandomBetween(minBound.position.y, maxBound.position.y);
        float z = RandomBetween(minBound.position.z, maxBound.position.z);
        return new Vector3(x, y, z);
    }

    float RandomBetween(float min, float max)
    {
        return Random.Range(min, max);
    }

    private void OnDestroy()
    {
        if (_eventService != null)
            _eventService.OnBossShouldSpawn -= SpawnBoss;
    }
}
