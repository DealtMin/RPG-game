using UnityEngine;
using UnityEngine.UI;

public class MobsUIController : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private IHealthController _mobsLifecycle;
    private IUIService _uiService;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        _mobsLifecycle = GetComponent<IMobController>().GetHealthController();
        _uiService = ServiceLocator.Get<IUIService>();  
        _mobsLifecycle.TakeDamage += ReduceHealth; 
        ReduceHealth(_mobsLifecycle.GetHealth(), _mobsLifecycle.GetMaxHealth());     
    }

    public void ReduceHealth(int currHealth, int maxHealth)
    {
        _uiService.SetFillAmountImage(healthBar, currHealth, maxHealth);
    }
    void OnDestroy()
    {
        _mobsLifecycle.TakeDamage -= ReduceHealth;
    }
}
