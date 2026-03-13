using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }
}
