using UnityEngine;
using UnityEngine.UI;

public class MobsUIController : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private IUIService _uiService;

    private void Start()
    {
        _uiService = ServiceLocator.Get<IUIService>();
    }

    public void ReduceHealth(int currHealth)
    {
        _uiService.SetFillAmountImage(healthBar, currHealth);
    }
}
