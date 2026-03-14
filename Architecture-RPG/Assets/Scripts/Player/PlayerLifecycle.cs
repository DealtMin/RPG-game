using UnityEngine;

public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    private PlayerUIController _playerUIController;
    [SerializeField] private int health;

    private void Awake()
    {
        _playerUIController = GetComponent<PlayerUIController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
        WeaponSOViewer weapon = other.GetComponent<WeaponSOViewer>();
        if (weapon)
        {
            Damage(weapon.damage);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        _playerUIController.ReduceHealth(health);
    }

    public void Death()
    {
        _playerUIController.Death();
    }
}
