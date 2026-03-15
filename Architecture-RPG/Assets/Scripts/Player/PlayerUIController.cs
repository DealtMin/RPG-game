using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shootTimer;
    [SerializeField] private TMP_Text hpText;

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
        healthBar.fillAmount = currHealth * 0.01f;
        hpText.text = "" + currHealth;
    }

    public void MagicTimerUI(float coolDown)
    {
        shootTimer.fillAmount =0;
        StartCoroutine(CoolMagic(coolDown));
    }
    
    private IEnumerator CoolMagic(float coolDown)
    {
        for (int i=0; i<100; i++)
        {
            shootTimer.fillAmount += 0.01f;
            yield return new WaitForSeconds(coolDown/100f);
        }
    }
}
