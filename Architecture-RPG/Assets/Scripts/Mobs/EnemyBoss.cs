using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour, IDamagable, IMobController
{
    public Animator Animator { get; protected set; }
    public int MaxHealth { get; private set; } = 100;
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    [SerializeField] private float damageInvincibility = 1.5f;
    private BossStateMachine _stateMachine;
    private Transform target;
    private NavMeshAgent _agent;
    private bool attackReady = true;
    private bool _canDamage;
    private IHealthController healthController;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    private IAudioService _audio;
    void Awake()
    {
        healthController = new BossHealthController(MaxHealth);
        _audio = ServiceLocator.Get<IAudioService>();
        target = FindAnyObjectByType<PlayerLifecycle>().transform;
        _agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        _stateMachine = new BossStateMachine(this);
    }
    public void Damage(int damage)
    {
        _canDamage = false;
        StartCoroutine(DamageCountDown(damageInvincibility));
        damageParticles.Play();
        _audio.PlaySound(hitClip);
        healthController.Damage(damage);
        Debug.Log($"Босс получил {damage} урона. HP: {healthController.GetHealth()}/{MaxHealth}");
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
    }
    public void Say(string message) =>
    Debug.Log($"Босс: \"{message}\"");
    public bool IsPlayerInView()
    {
        return Vector3.Distance(target.position, transform.position) < noticeDistance; ;
    }
    public bool IsPlayerNear()
    {
        return Vector3.Distance(target.position, transform.position) < _agent.stoppingDistance;
    }
    public bool IsAttackReady()
    {
        if (attackReady)
        {
            attackReady = false;
            StartCoroutine(AttackPermission(attackCoolDown));
            return !attackReady;
        }
        return attackReady;
    }
    private IEnumerator AttackPermission(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        attackReady = true;
    }
    public void ChasePlayer()
    {
        _agent.destination = target.position;
    }
    public void SetChaising(bool flag)
    {
        _agent.isStopped = !flag;
    }
    private void Death()
    {
        damageParticles.Play();
    }

    public IHealthController GetHealthController()
    {
        return healthController;
    }
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;
    }
}
