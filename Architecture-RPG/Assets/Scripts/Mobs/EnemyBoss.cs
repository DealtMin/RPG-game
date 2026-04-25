using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour, IDamagable, IMobController
{

    [SerializeField] private GameObject magicAttack;
    [SerializeField] private Transform magicSpawmPoint;
    [SerializeField] private float secondPhaseStoppingDistance = 2f;
    public GameObject MagicAttackPrefab => magicAttack;
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
    private BossHealthController healthController;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    private IAudioService _audio;
    void Awake()
    {
        healthController = new BossHealthController(MaxHealth);
        _audio = ServiceLocator.Get<IAudioService>();
        target = FindAnyObjectByType<PlayerLifecycle>().transform;
        _agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        _stateMachine = new BossStateMachine(this);
        healthController.SecondPhase += SecondPhase;
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
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
    public void RangeAttack()
    {
        GameObject newMagicBall = Instantiate(magicAttack, magicSpawmPoint.position, Quaternion.identity);
        MushroomBallBehaviour mushroomBall = newMagicBall.GetComponent<MushroomBallBehaviour>();

        mushroomBall.Construct(target, gameObject.transform);
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
    private void SetFightStateMachine(BossStateMachine fightSM)
    {
        _stateMachine = fightSM;
        _stateMachine.Initialize();
    }
    public void SecondPhase()
    {
        _agent.stoppingDistance = secondPhaseStoppingDistance;
        SetFightStateMachine(new Phase2BossFightStateMachine(this));
    }
}
