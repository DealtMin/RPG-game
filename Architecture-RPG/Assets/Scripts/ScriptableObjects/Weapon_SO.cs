using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon_SO : ScriptableObject
{
    [SerializeField] private Vector3 initScale;
    [SerializeField] private int damage;
    [SerializeField] private bool isMelee; //это оружие ближнего боя
    
    public Vector3 weaponInitScale => initScale; 
    public bool weaponIsMelee => isMelee;
    public int weaponDamage => damage; 
}
