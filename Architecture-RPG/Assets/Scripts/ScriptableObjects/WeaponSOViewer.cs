using UnityEngine;

public class WeaponSOViewer : MonoBehaviour
{
    [SerializeField] private Weapon_SO weaponItem;
    public int damage { get; private set; }
    public bool isMelee { get; private set; } //это оружие ближнего боя
    public float initScaleFactor { get; private set; }
    private void Awake()
    {
        damage = weaponItem.weaponDamage;
        isMelee = weaponItem.weaponIsMelee;
        initScaleFactor = weaponItem.weaponInitScaleFactor;
    }

}
