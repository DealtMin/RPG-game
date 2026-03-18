using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    private Vector3 _startScale;
    [SerializeField] private float scaleFactor;
    [SerializeField] private AudioClip onHoverClip;
    [SerializeField] private AudioClip onClickClip;
    [SerializeField] private AudioSource source;

    public void OnHover(Transform obj)
    {
        _startScale = obj.localScale;
        obj.localScale *= scaleFactor;
        source.PlayOneShot(onHoverClip);
    }

    public void ExitHover(Transform obj)
    {
        obj.localScale = _startScale;
    }
    
    public void OnClick(Transform obj)
    {
        obj.localScale = _startScale;
        source.PlayOneShot(onClickClip);
    }
}
