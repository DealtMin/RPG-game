using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Image healthBar;

    public void Pause(bool pauseOn)
    {
        pausePanel.SetActive(pauseOn);
    }

    public void Death()
    {
        pausePanel.SetActive(false);
        deathPanel.SetActive(true);
    }

    public void ReduceHealth(int currHealth)
    {
        healthBar.fillAmount = currHealth / 100;
    }
}
