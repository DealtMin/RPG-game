using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    public void DestroyAfterAnimation()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
