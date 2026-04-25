using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IMobController
{
    [SerializeField] private GameObject magicAttack;
    [SerializeField] private Transform magicSpawmPoint;
    public GameObject MagicAttackPrefab => magicAttack;
    public Animator Animator { get; private set; }
    [SerializeField] bool isRange = true;
    [SerializeField] private int MaxHealth = 120;
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    [SerializeField] private float damageInvincibility = 1.5f;
    private EnemyStateMachine _stateMachine;
    private Transform target;
    private NavMeshAgent _agent;
    private bool attackReady = true;
    private bool _canDamage = true;
    private BossHealthController healthController;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    private IAudioService _audio;
    public void Construct(Transform transform) => target=transform;
    
    void Awake()
    {
        healthController = new BossHealthController(MaxHealth);
        _audio = ServiceLocator.Get<IAudioService>();
        _agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        _stateMachine = new EnemyStateMachine(this);
        healthController.DeathEvent += Death;
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
    }
    void Oestroy() => healthController.DeathEvent -= Death;
    public void Damage(int damage)
    {
        if (_canDamage)
        {
            _canDamage = false;
            StartCoroutine(DamageCountDown(damageInvincibility));
            damageParticles.Play();
            _audio.PlaySound(hitClip);
            healthController.Damage(damage);
            Debug.Log($"Босс получил {damage} урона. HP: {healthController.GetHealth()}/{MaxHealth}");
        }
    }
    public void Attack() {}
    public bool IsPlayerInView()
    {
        return Vector3.Distance(target.position, transform.position) < noticeDistance; ;
    }
    public bool IsPlayerNear()
    {
        return Vector3.Distance(target.position, transform.position) < _agent.stoppingDistance;
    }
    public bool IsAttackReady() => attackReady;
    public void SetAttackCoolDown()
    {
        if (attackReady)
        {
            attackReady = false;
            StartCoroutine(AttackPermission(attackCoolDown));
        }
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
        _stateMachine.ChangeState(new EnemyDeathState(_stateMachine));
        damageParticles.Play();
        
    }

    public IHealthController GetHealthController()
    {
        return healthController;
    }
    private IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;
    }
}
