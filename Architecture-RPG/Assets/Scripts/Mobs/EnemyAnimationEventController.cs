using UnityEngine;

public class EnemyAnimationEventController : MonoBehaviour
{
    public void DestroyAfterAnimation()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
