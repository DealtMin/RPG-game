using System;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private float uiScaleFactor = 1.07f;
    private IAudioService _audio;
    private IUIService _uiService;
    

    private void Start()
    {
        _audio = ServiceLocator.Get<IAudioService>();
        _uiService = ServiceLocator.Get<IUIService>();

        Time.timeScale = 1;
    }

    public void LoadGame()
    {
        _uiService.OpenSceneByName("Main");
    }

    public void OpenUIPanel(GameObject panel)
    {
        _uiService.ShowHideElement(settingsPanel, settingsPanel==panel);
        _uiService.ShowHideElement(mainPanel, mainPanel==panel);
    }
    
    public void OnHoverEnter(Transform obj)
    {
        _uiService.IncreaseScale(obj, uiScaleFactor);
    }
    
    public void OnHoverExit(Transform obj)
    {
        _uiService.DecreaseScale(obj, uiScaleFactor);
    }
}
