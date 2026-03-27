using UnityEngine;
using UnityEngine.UI;

public class MobsUIController : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private MobsLifecycle _mobsLifecycle;
    private IUIService _uiService;

    private void Start()
    {
        _mobsLifecycle = GetComponent<MobsLifecycle>();
        _mobsLifecycle.EnemyTakeDamage += ReduceHealth;
        _uiService = ServiceLocator.Get<IUIService>();
    }

    public void ReduceHealth(int currHealth)
    {
        _uiService.SetFillAmountImage(healthBar, currHealth);
    }
    void OnDestroy()
    {
        _mobsLifecycle.EnemyTakeDamage -= ReduceHealth;
    }
}
