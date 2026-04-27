using UnityEngine;

public class MultipleMobsWeaponsController : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    public int weaponInx { get; private set; }

    public void SelectWeapon(int indx)
    {
        if (indx == -1)
        {
            weaponInx = RandomBetween(0, weapons.Length);
        }
        else
        {
            weaponInx = indx;
        }

        for (int i = weapons.Length - 1; i >= 0; i--)
        {
            if (i == weaponInx) weapons[i].SetActive(true);
            else Destroy(weapons[i]);
        }

    }


    int RandomBetween(int min, int max)
    {
        return Random.Range(min, max);
    }
}
