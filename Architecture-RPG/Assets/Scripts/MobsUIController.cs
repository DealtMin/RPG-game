using UnityEngine;
using UnityEngine.UI;

public class MobsUIController : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void ReduceHealth(int currHealth)
    {
        healthBar.fillAmount = currHealth / 100;
    }
}
