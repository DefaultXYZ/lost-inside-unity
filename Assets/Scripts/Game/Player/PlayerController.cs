using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private bool sprintEnabled = true;

    [SerializeField]
    private bool jumpEnabled = true;

    [Header("Ground")]
    [SerializeField]
    private LayerMask excludeGroundCheckLayers;

    [Header("Other")]
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Transform meshParent;

    [SerializeField]
    private Transform head;

    private const float Sensitivity = 20f;
    private const float LookVerticalAngleBounds = 60f;
    private const float WalkSpeed = 5f;
    private const float SprintSpeed = 10f;
    private const float GroundDistance = .5f;
    private const float JumpForce = 5f;

    private Rigidbody rb;

    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private Vector2 inputLook;
    private Vector3 inputMove;
    private bool isGrounded;
    private float currentSpeed = WalkSpeed;
    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;

        lookAction = InputSystem.actions.FindAction(InputActionNames.Look);
        moveAction = InputSystem.actions.FindAction(InputActionNames.Move);
        jumpAction = InputSystem.actions.FindAction(InputActionNames.Jump);
        sprintAction = InputSystem.actions.FindAction(InputActionNames.Sprint);
    }

    private void OnEnable()
    {
        lookAction.Enable();
        moveAction.Enable();
        sprintAction.Enable();
        jumpAction.performed += JumpActionPerformed;
    }

    private void OnDisable()
    {
        lookAction.Disable();
        moveAction.Disable();
        sprintAction.Disable();
        jumpAction.performed -= JumpActionPerformed;
    }

    private void JumpActionPerformed(InputAction.CallbackContext obj)
    {
        if (jumpEnabled && isGrounded)
        {
            var velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        ReadPlayerInput();
    }

    private void LateUpdate()
    {
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
    }

    private void ReadPlayerInput()
    {
        var inputMoveValue = moveAction.ReadValue<Vector2>();
        inputMove = new Vector3(inputMoveValue.x, 0f, inputMoveValue.y);

        inputLook = lookAction.ReadValue<Vector2>() * Time.deltaTime * Sensitivity;
    }

    private void RotatePlayer()
    {
        yRotation += inputLook.x;

        xRotation -= inputLook.y;
        xRotation = Mathf.Clamp(xRotation, -LookVerticalAngleBounds, LookVerticalAngleBounds);

        head.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        meshParent.rotation = orientation.rotation;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, GroundDistance, ~excludeGroundCheckLayers);
    }

    private void MovePlayer()
    {
        var movementDirection = orientation.forward * inputMove.z + orientation.right * inputMove.x;
        if (sprintEnabled && isGrounded)
        {
            currentSpeed = sprintAction.IsPressed() ? SprintSpeed : WalkSpeed;
        }

        var velocity = movementDirection * currentSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
}
