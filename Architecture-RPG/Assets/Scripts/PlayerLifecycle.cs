using UnityEngine;

[RequireComponent(typeof(PlayerUIController))]
public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    private PlayerUIController _playerUIController;
    public int Health { get; set; }

    private void Awake()
    {
        _playerUIController = GetComponent<PlayerUIController>();
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Death();
        }
        _playerUIController.ReduceHealth(Health);
    }

    public void Death()
    {
        _playerUIController.Death();
    }
}
