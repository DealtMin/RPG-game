using UnityEngine;
using Unity.Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;
    [SerializeField] private float verticalLookLimit = 80f;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private float yRotationAdditive;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Camera mainCamera;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private Transform cameraLookAtTarget;

    private float _xRotation = 10f;
    private float _yRotation;

    private void Awake()
    {
        SetupOrientation();
        SetupCameraTargets();
        SetupMainCamera();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _yRotation = transform.eulerAngles.y;
    }

    private void SetupOrientation()
    {
        if (!orientation)
        {
            GameObject orientationObj = new GameObject("Orientation");
            orientationObj.transform.SetParent(transform);
            orientationObj.transform.localPosition = Vector3.zero;
            orientation = orientationObj.transform;
        }
    }

    private void SetupCameraTargets()
    {
        if (!cameraFollowTarget)
        {
            GameObject followObj = new GameObject("CameraFollowTarget");
            followObj.transform.SetParent(transform);
            followObj.transform.localPosition = Vector3.up * 1.6f;
            cameraFollowTarget = followObj.transform;
        }

        if (!cameraLookAtTarget)
        {
            GameObject lookAtObj = new GameObject("CameraLookAtTarget");
            lookAtObj.transform.SetParent(transform);
            lookAtObj.transform.localPosition = Vector3.up * 1.6f;
            cameraLookAtTarget = lookAtObj.transform;
        }
    }

    private void SetupMainCamera()
    {
        if (!mainCamera)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");

            if (camObj)
            {
                mainCamera = camObj.GetComponent<Camera>();
            }
            else
            {
                mainCamera = Camera.main;
            }
        }
    }

    private void Update()
    {
        if (!inputHandler) return;

        float deltaTime = Mathf.Min(Time.deltaTime, 0.033f);
        HandleCameraRotation(deltaTime);
    }

    private void LateUpdate()
    {
        ApplyRotations();
    }

    private void HandleCameraRotation(float deltaTime)
    {
        Vector2 lookInput = inputHandler.LookInput;

        float mouseX = lookInput.x * deltaTime * mouseSensitivityX;
        float mouseY = lookInput.y * deltaTime * mouseSensitivityY;

        _yRotation += mouseX;
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -verticalLookLimit, verticalLookLimit);
    }

    private void ApplyRotations()
    {
        if (orientation)
        {
            orientation.rotation = Quaternion.Euler(0, _yRotation + yRotationAdditive, 0);
        }

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation + yRotationAdditive, 0);

        if (cameraFollowTarget)
        {
            cameraFollowTarget.rotation = Quaternion.Euler(_xRotation, _yRotation + yRotationAdditive, 0);
        }

        if (cameraLookAtTarget)
        {
            cameraLookAtTarget.rotation = Quaternion.Euler(_xRotation, _yRotation + yRotationAdditive, 0);
        }

        if (mainCamera)
        {
            mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        }
    }

    public Transform GetOrientation() => orientation;
    public Transform GetCameraFollowTarget() => cameraFollowTarget;
}