using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int[] enemiesCount;
    [SerializeField] private Transform maxBound;
    [SerializeField] private Transform minBound;


    private void Awake()
    {
        for (int i=0; i<enemies.Length; i++)
        {
            for (int j = 0; j < enemiesCount[i]; j++)
            {
                Vector3 newpos = RandomPosition(minBound, maxBound);
               Instantiate(enemies[i], newpos, Quaternion.identity);
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
