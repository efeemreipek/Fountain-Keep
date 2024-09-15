using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8.0f;

    [Space(10),Header("Jump and Gravity")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float jumpGravityMultiplier = 1.5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private AudioClip[] jumpStartSounds;

    private bool _isPlayerGrounded;
    private bool _isJumpRequested;

    [SerializeField] private float coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    [Space(10), Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private Transform cameraTransform;

    [Space(10), Header("Footstep")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float walkStepInterval = 0.5f;
    //[SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 0.2f;

    [Space(10)]
    [SerializeField] private AudioClip[] pickupSounds;
    [SerializeField] private Animator animator;

    private CharacterController _characterController;
    private Vector3 _playerVelocity;

    private float nextStepTime;
    private bool isMoving;
    private int lastPlayedIndex = -1;


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!GameManager.Instance.GetIsInputActive()) return;

        CheckIsGrounded();
        Vector3 move = GetMovementInput();
        HandleMovementAndRotation(move);
        HandleJumpAndGravity(move);
        HandleFootsteps();
    }

    private void CheckIsGrounded()
    {
        // Check if the player is grounded using raycast for more accurate detection
        _isPlayerGrounded = IsGrounded();
        animator.SetBool("isGrounded", _isPlayerGrounded);

        // If grounded, reset the coyote time counter
        if (_isPlayerGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            // Countdown coyote time when not grounded
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }
    private Vector3 GetMovementInput()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;
        return move;
    }
    private void HandleMovementAndRotation(Vector3 move)
    {
        isMoving = IsMoving();

        // Convert movement to camera-relative direction
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            move = (forward * move.z + right * move.x).normalized;
        }

        // Move the character
        _characterController.Move(move * speed * Time.deltaTime);

        animator.SetBool("isMoving", isMoving);

        // Rotate the player to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void HandleJumpAndGravity(Vector3 move)
    {
        // Handle jump input
        if (Input.GetButtonDown("Jump"))
        {
            _isJumpRequested = true;
        }

        // Jumping logic with coyote time
        if (_isJumpRequested && _coyoteTimeCounter > 0f)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            _isJumpRequested = false;  // Reset jump request after jumping
            PlaySounds(jumpStartSounds);
        }

        // Apply gravity
        if (_playerVelocity.y > 0)
        {
            _playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            _playerVelocity.y += gravityValue * jumpGravityMultiplier * Time.deltaTime;
        }

        // Apply vertical velocity to movement
        _characterController.Move(_playerVelocity * Time.deltaTime);

        // Stop player instantly when no input
        if (move == Vector3.zero && _isPlayerGrounded)
        {
            _playerVelocity.x = 0f;
            _playerVelocity.z = 0f;
        }
    }

    private void HandleFootsteps()
    {
        float currentStepInterval = /*_sprintAction.ReadValue<float>() > 0f ? sprintStepInterval : */ walkStepInterval;

        isMoving = IsMoving();

        if (_isPlayerGrounded && isMoving && Time.time > nextStepTime && _characterController.velocity.magnitude > velocityThreshold)
        {
            PlaySounds(footstepSounds);
            nextStepTime = Time.time + currentStepInterval;
        }
    }
    private void PlaySounds(AudioClip[] sounds)
    {
        int randomIndex;
        if (sounds.Length == 1)
        {
            randomIndex = 0;
        }
        else
        {
            randomIndex = Random.Range(0, sounds.Length - 1);
            if (randomIndex > lastPlayedIndex)
            {
                randomIndex++;
            }
        }

        lastPlayedIndex = randomIndex;
        audioSource.clip = sounds[randomIndex];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
    private bool IsMoving() => GetMovementInput() != Vector3.zero;
    private bool IsGrounded() => Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPickable pickable))
        {
            pickable.Pick();
            PlaySounds(pickupSounds);
        }
    }
}
