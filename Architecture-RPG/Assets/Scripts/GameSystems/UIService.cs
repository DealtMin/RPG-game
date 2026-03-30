using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class UIService : MonoBehaviour, IUIService
{
   
    public void SetFillAmountImage(Image fillImage, int value)
    {
        fillImage.fillAmount = value * 0.01f;
    }

    public void SetTMPRoText<T>(TMP_Text targetText, T value)
    {
        targetText.text = ""+value;
    }

    public void OpenSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    
    public void StartFillCoroutine(Image fillImage, float coolDown)
    {
        StartCoroutine(FillAmountImage(fillImage, coolDown));
    }

    public void ShowHideElement(GameObject element, bool visible)
    {
        element.SetActive(visible);
    }
    
    
    private IEnumerator FillAmountImage(Image fillImage, float coolDown)
    {
        for (int i=0; i<100; i++)
        {
            fillImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(coolDown/100f);
        }
    }
    

    public void IncreaseScale(Transform obj, float scaleFactor)
    {
        obj.localScale *= scaleFactor;
    }

    public void DecreaseScale(Transform obj, float scaleFactor)
    {
        obj.localScale /= scaleFactor;
    }
    
    
}
