using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[] _enemies;
    private int[] _enemiesCount;
    private Transform _maxBound;
    private Transform _minBound;

    private Transform _playerTransform;

    public void Construct(GameObject[] enemies, int[] enemiesCount, Transform maxBound, Transform minBound, Transform playerTransform)
    {
        _enemies = enemies;
        _enemiesCount = enemiesCount;
        _maxBound = maxBound;
        _minBound = minBound;
    }

    public void Spawn()
    {
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
                EnemyAI enemyai = newEnemy.GetComponent<EnemyAI>();
                enemyai.Construct(_playerTransform);
            }
        }
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
}
