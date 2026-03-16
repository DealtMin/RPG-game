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
    [SerializeField] private float uiScaleFactor=1.07f;
    

    public void Pause(bool pauseOn)
    {
        uiBase.ShowHideElement(pausePanel, pauseOn);
    }

    public void GoToMenu()
    {
        uiBase.OpenSceneByName("Menu");
    }
    
    public void ReloadGame()
    {
        uiBase.OpenSceneByName("Main");
    }

    public void SaveData()
    {
        
    }


    public void LoadData()
    {
        
    }
    
  
    public void OnHoverEnter(Transform obj)
    {
        uiBase.IncreaseScale(obj, uiScaleFactor);
    }
    
    public void OnHoverExit(Transform obj)
    {
        uiBase.DecreaseScale(obj, uiScaleFactor);
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
