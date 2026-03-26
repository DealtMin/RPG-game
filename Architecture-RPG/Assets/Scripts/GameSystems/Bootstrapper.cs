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
    
    private void Awake()
    {
        ServiceLocator.Register<IAudioService>(new AudioService(source, slider, mixer));
        // UI base
        // Saver
        
        GameObject spawnerObj = new GameObject("EnemiesSpawner");
        EnemySpawner spawner = spawnerObj.AddComponent<EnemySpawner>();
        spawner.Construct(enemies, enemiesCount, maxBound, minBound, playerObject.transform);
        spawner.Spawn();
        
        
        LoadGameScene();
    }
    
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}