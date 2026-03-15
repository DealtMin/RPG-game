using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int[] enemiesCount;
    [SerializeField] private Transform maxBound;
    [SerializeField] private Transform minBound;

    [SerializeField] private Transform playerTransform;


    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = FindAnyObjectByType<PlayerController>().transform;
        }
        for (int i=0; i<enemies.Length; i++)
        {
            for (int j = 0; j < enemiesCount[i]; j++)
            {
                Vector3 newpos = RandomPosition(minBound, maxBound);
                
                GameObject newEnemy = Instantiate(enemies[i], newpos, Quaternion.identity);
                EnemyAI enemyai = newEnemy.GetComponent<EnemyAI>();
                enemyai.SetParams(playerTransform);
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
