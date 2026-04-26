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
    private EnemyHealthController healthController;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    private IAudioService _audio;
    private ISettingsLoader settings;
    private AbstractEnemyState initialState;
    public void Construct(Transform transform) => target=transform;
    
    void Awake()
    {
        healthController = new EnemyHealthController(MaxHealth);
        _audio = ServiceLocator.Get<IAudioService>();
        settings = ServiceLocator.Get<ISettingsLoader>();
        _agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        _stateMachine = CreateStateMachine(settings.GetPlayModeIndex());
        healthController.DeathEvent += Death;
    }
    EnemyStateMachine CreateStateMachine(int index)
    {
        if (index == 0) return new EnemyPassiveStateMachine(this);
        return new EnemyStateMachine(this);
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
    }
    void OnDestroy() => healthController.DeathEvent -= Death;
    public void Damage(int damage)
    {
        if (_canDamage)
        {
            _canDamage = false;
            StartCoroutine(DamageCountDown(damageInvincibility));
            damageParticles.Play();
            _audio.PlaySound(hitClip);
            healthController.Damage(damage);
        }
    }
    public void Attack()
    {
        if (isRange)
        {
            GameObject newMagicBall = Instantiate(magicAttack, magicSpawmPoint.position, Quaternion.identity);
            MagicAttackBehaivour magicBeh = newMagicBall.GetComponent<MagicAttackBehaivour>();
            magicBeh.Construct(target, gameObject.transform);
        }
    }
    public void Flee()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + direction * noticeDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(fleePosition, out hit, noticeDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
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
