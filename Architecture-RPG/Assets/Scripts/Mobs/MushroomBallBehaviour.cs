using System;
using UnityEngine;

public class MushroomBallBehaviour : MagicAttackBehaivour
{
    [SerializeField] private GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && _rb.linearVelocity.magnitude>=0.5)
        {
            Destroy(gameObject);
        }
        else
        {
            particles.SetActive(true);
        }
    }
}
