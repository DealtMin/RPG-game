using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerUIController playerUI;
    public void AfterDeathUI()
    {
        Time.timeScale = 0f;
        playerUI.Death();
    }
}
