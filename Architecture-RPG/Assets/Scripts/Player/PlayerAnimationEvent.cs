using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerUIController playerUI;
    public void AfterDeathUI()
    {
        playerUI.Death();
    }
    public void StartDeath()
    {
        playerUI.ReduceHealth(0);
    }
}
