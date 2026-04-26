using UnityEngine;

public class MultipleMobsWeaponsController : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    private void Awake()
    {
        int weaponInx = RandomBetween(0, weapons.Length);
        for (int i = weapons.Length-1; i >=0; i--)
        {
            if (i==weaponInx) weapons[i].SetActive(true);
            else Destroy(weapons[i]);
        }
    }
    
    
    int RandomBetween(int min, int max)
    {
        return Random.Range(min, max);
    }
}
