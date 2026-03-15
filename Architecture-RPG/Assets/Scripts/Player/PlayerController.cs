using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform modelRoot;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Components")]
    private PlayerMovement _movement;
    private PlayerInputHandler _inputHandler;
    private PlayerCombat _combat;

    [Header("Camera")]
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float yRotationOffset = 0f;

    private Camera _mainCamera;

    public PlayerMovement Movement => _movement;
    public PlayerInputHandler InputHandler => _inputHandler;
    public PlayerCombat Combat => _combat;
    public Transform ProjectileSpawnPoint => projectileSpawnPoint;

    private void Awake()
    {
        InitializeComponents();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cameraController != null)
        {
            UpdateAimTarget();
        }

        HandleInput();
    }

    private void LateUpdate()
    {
        UpdateModelRotation();
    }

    private void InitializeComponents()
    {
        _movement     = GetComponent<PlayerMovement>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _combat       = GetComponent<PlayerCombat>();

        if (cameraController == null)
        {
            cameraController = GetComponent<PlayerCameraController>()
                ?? gameObject.AddComponent<PlayerCameraController>();
        }

        if (_combat != null)
        {
            _combat.SetProjectileSpawnPoint(projectileSpawnPoint);
        }
    }

    private void HandleInput()
    {
        if (_inputHandler == null) return;

        _movement.SetMoveInput(_inputHandler.MoveInput);
        _movement.SetSprintInput(_inputHandler.SprintPressed);

        if (cameraController != null)
        {
            Transform orient = cameraController.GetOrientation();

            if (orient != null)
            {
                _movement.SetOrientation(orient);
            }

            _movement.SetCameraTransform(_mainCamera?.transform);
        }
    }

    private void UpdateModelRotation()
    {
        if (modelRoot == null || cameraController == null) return;

        Transform orient = cameraController.GetOrientation();
        if (orient == null) return;

        float targetY = orient.eulerAngles.y + yRotationOffset;
        modelRoot.rotation = Quaternion.Euler(0f, targetY, 0f);
    }

    private void UpdateAimTarget()
    {
        if (_mainCamera == null || aimTarget == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(
            new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)
        );

        aimTarget.position = ray.GetPoint(15f);
    }
    
}