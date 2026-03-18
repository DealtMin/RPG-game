using UnityEngine;
using UnityEngine.UI;

public class MobsUIController : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private UIControllerBase uiBase;


    public void ReduceHealth(int currHealth)
    {
       uiBase.SetFillAmountImage(healthBar, currHealth);
    }
}
