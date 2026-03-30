using UnityEngine;
using UnityEngine.UI;
using TMPro;
public interface IUIService
{
    void SetFillAmountImage(Image fillImage, int value);


    public void SetTMPRoText<T>(TMP_Text targetText, T value);


    public void OpenSceneByName(string sceneName);


    public void ShowHideElement(GameObject element, bool visible);


    public void IncreaseScale(Transform obj, float scaleFactor);


    public void DecreaseScale(Transform obj, float scaleFactor);

    void StartFillCoroutine(Image fillImage, float coolDown);

}
