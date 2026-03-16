using System;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private UIControllerBase uiBase;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private float uiScaleFactor = 1.07f;

    private void Awake() // !!!!!!!! убрать при бустстрапе
    {
        Time.timeScale = 1;
    }

    public void LoadGame()
    {
        uiBase.OpenSceneByName("Main");
    }

    public void OpenUIPanel(GameObject panel)
    {
        uiBase.ShowHideElement(settingsPanel, settingsPanel==panel);
        uiBase.ShowHideElement(mainPanel, mainPanel==panel);
    }
    
    public void OnHoverEnter(Transform obj)
    {
        uiBase.IncreaseScale(obj, uiScaleFactor);
    }
    
    public void OnHoverExit(Transform obj)
    {
        uiBase.DecreaseScale(obj, uiScaleFactor);
    }
}
