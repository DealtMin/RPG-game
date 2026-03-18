using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon_SO : ScriptableObject
{
    [SerializeField] private float initScaleFactor;
    [SerializeField] private int damage;
    [SerializeField] private bool isMelee; //это оружие ближнего боя
    
    public float weaponInitScaleFactor => initScaleFactor; 
    public bool weaponIsMelee => isMelee;
    public int weaponDamage => damage; 
}
