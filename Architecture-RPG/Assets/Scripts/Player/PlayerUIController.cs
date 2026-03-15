using System;
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
    [SerializeField] private UIControllerBase uiBase;
    

    public void Pause(bool pauseOn)
    {
        uiBase.ShowHideElement(pausePanel, pauseOn);
    }

    public void Death()
    {
        uiBase.ShowHideElement(pausePanel, false);
        uiBase.ShowHideElement(deathPanel, true);
    }
    
    public void ReduceHealth(int health)
    {
        uiBase.SetFillAmountImage(healthBar, health);
        uiBase.SetTMPRoText(hpText, health);
    }
    
    public void MagicTimerUI(float coolDown)
    {
        shootTimer.fillAmount =0;
        uiBase.StartFillCoroutine(shootTimer, coolDown);
    }
}
