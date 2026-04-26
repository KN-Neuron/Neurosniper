using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private const float gravity = -9.81f;
    private float yawSensitivity = 0.1f;
    private float pitchSensitivity = 0.1f;
    private bool isGrounded;
    public Vector3 velocity;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleRotation();
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    private void HandleMovement()
    {
        Vector2 input = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = transform.right * input.x + transform.forward * input.y;
        float speed = GameInput.Instance.IsRunning() ? runSpeed : walkSpeed;

        characterController.Move(moveDir * speed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector2 lookInput = GameInput.Instance.GetLookVector();
        float yawDelta = lookInput.x * yawSensitivity;
        transform.Rotate(Vector3.up, yawDelta, Space.Self);
        float pitchDelta = lookInput.y * pitchSensitivity;
        cameraRoot.Rotate(Vector3.right, pitchDelta, Space.Self);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
