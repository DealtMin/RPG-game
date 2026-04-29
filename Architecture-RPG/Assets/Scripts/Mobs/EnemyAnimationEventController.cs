using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] Collider[] colliders;
    public void DestroyAfterAnimation()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
    public void ColliderControl()
    {
        foreach (Collider collider in colliders)
            collider.enabled = false;
    }
}
