using UnityEngine;

public class MobsLifecycle : MonoBehaviour, IDamagable
{
    private MobsUIController _mobsUIController;
    public int Health { get; set; }

    private void Awake()
    {
        _mobsUIController = GetComponent<MobsUIController>();
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Death();
        }
        _mobsUIController.ReduceHealth(Health);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
