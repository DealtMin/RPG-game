using UnityEngine;
using System.Collections;

public class MagicAttackBehaivour : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float scalingTime=0.2f;
    private Rigidbody _rb;
    private Vector3 _direction;
    private Transform _parentObject;

    public void SetParams(Transform inputPlayerTransform, Transform parentObject)
    {
        _playerTransform = inputPlayerTransform;
        _parentObject = parentObject;
        Preapre();
        StartCoroutine(Scaler(scalingTime));

    }
    
    private IEnumerator Scaler(float sclaingTime)
    {
        Vector3 scaleStep = gameObject.transform.localScale * 0.01f;
        gameObject.transform.localScale = Vector3.zero;
        for (int i=0; i<100; i++)
        {
            gameObject.transform.localScale += scaleStep;
            yield return new WaitForSeconds(sclaingTime * 0.01f);
        }
    }
    
    void Preapre()
    {
        _rb = GetComponent<Rigidbody>();
        if (_playerTransform)
            _direction = Vector3.Normalize(_playerTransform.position - transform.position);
        else _direction = _parentObject.forward;
        transform.forward = _direction;
    }
    
    void FixedUpdate()
    {
        _rb.AddForce(_direction * speed, ForceMode.Force);
    }
    
    void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
