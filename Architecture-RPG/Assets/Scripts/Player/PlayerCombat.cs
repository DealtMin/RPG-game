using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Physical Attack (ЛКМ)")]
    [SerializeField] private float physicalCooldown = 0.5f;

    [Header("Magic Attack (ПКМ)")]
    [SerializeField] private float magicCooldown = 2f;

    private float _lastPhysicalTime;
    private float _lastMagicTime;

    private bool _physicalOnCooldown => Time.time < _lastPhysicalTime + physicalCooldown;
    private bool _magicOnCooldown => Time.time < _lastMagicTime + magicCooldown;

    private void OnEnable()
    {
        if (inputHandler == null) return;

        inputHandler.OnAttackPressed += HandlePhysicalAttack;
        inputHandler.OnMagicAttackPressed += HandleMagicAttack;
    }

    private void OnDisable()
    {
        if (inputHandler == null) return;

        inputHandler.OnAttackPressed -= HandlePhysicalAttack;
        inputHandler.OnMagicAttackPressed -= HandleMagicAttack;
    }

    public void SetProjectileSpawnPoint(Transform point)
    {
        projectileSpawnPoint = point;
    }

    private void HandlePhysicalAttack()
    {
        if (_physicalOnCooldown) return;

        _lastPhysicalTime = Time.time;

        Debug.Log("[Physical Attack] ЛКМ — обычная атака");
    }

    private void HandleMagicAttack()
    {
        if (_magicOnCooldown) return;

        _lastMagicTime = Time.time;

        Debug.Log("[Magic Attack] ПКМ — магическая атака");


    }
}