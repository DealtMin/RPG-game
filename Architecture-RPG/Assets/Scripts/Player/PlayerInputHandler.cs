using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
        
    private InputActionMap _playerActionMap;
     private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _attackAction;
    private InputAction _magicattackAction;

        
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _sprintPressed;

        
    public Action<Vector2> OnMoveInput;
    public Action<Vector2> OnLookInput;

    public Action OnSprintPressed;
    public Action OnSprintReleased;
    public Action OnInteractPressed;
    public Action OnAttackPressed;
    public Action OnMagicAttackPressed;


        
    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;
    public bool SprintPressed => _sprintPressed;
  
        
    private void Awake()
    {
        InitializeInput();
    }
        
    private void OnEnable()
    {
        EnableInput();
    }
        
    private void OnDisable()
    {
        DisableInput();
    }

    private void InitializeInput()
    {
        if (!inputActions)
        {
            return;
        }
            
        _playerActionMap = inputActions.FindActionMap("Player");
        if (_playerActionMap == null)
        {
            return;
        }
            
        _moveAction = _playerActionMap.FindAction("Move");
        _lookAction = _playerActionMap.FindAction("Look");
        _sprintAction = _playerActionMap.FindAction("Sprint");
    

            
        if (_moveAction != null)
        {
            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
        }
            
        if (_lookAction != null)
        {
            _lookAction.performed += OnLook;
            _lookAction.canceled += OnLook;
        }
            
        if (_sprintAction != null)
        {
            _sprintAction.performed += OnSprint;
            _sprintAction.canceled += SprintReleased;
        }

        _attackAction = _playerActionMap.FindAction("Attack");
        _magicattackAction=_playerActionMap.FindAction("MagicAttack");

        if (_attackAction != null)
        {
            _attackAction.performed += OnAttack;

        }
        if (_magicattackAction != null)
        {
            _magicattackAction.performed += OnMagicAttack;

        }
            

    }
        
    private void EnableInput()
    {
        _playerActionMap?.Enable();
    }
        
    private void DisableInput()
    {
        _playerActionMap?.Disable();
    }
        
    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(_moveInput);
    }
        
    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
        OnLookInput?.Invoke(_lookInput);
    }
        
    private void OnSprint(InputAction.CallbackContext context)
    {
        _sprintPressed = true;
        OnSprintPressed?.Invoke();
    }
        
    private void SprintReleased(InputAction.CallbackContext context)
    {
        _sprintPressed = false;
        OnSprintReleased?.Invoke();
    }
        
    private void OnAttack(InputAction.CallbackContext context)
    {

        OnAttackPressed?.Invoke();
    }

    private void OnMagicAttack(InputAction.CallbackContext context)
    {
        OnMagicAttackPressed?.Invoke();
    }


        

        
    public InputActionAsset GetInputActionAsset()
    {
        return inputActions;
    }
        
    public InputActionMap GetPlayerActionMap()
    {
        return _playerActionMap;
    }
}


