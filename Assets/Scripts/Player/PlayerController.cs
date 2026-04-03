using UnityEngine;

// PlayerController is responsible for handling player movement and camera control in a first-person shooter game.
public class PlayerController : MonoBehaviour
{
    // Player movement variables
    public float moveSpeed = 5f; // Normal movement speed
    public float sprintSpeed = 10f; // Sprinting speed
    public float jumpForce = 5f; // Jump force
    public float mouseSensitivity = 100f; // Camera look sensitivity
    public Transform playerBody; // Reference to the player's body
    public AudioSource footstepAudio; // Audio source for footstep sounds

    private float xRotation = 0f; // Store the x rotation for camera
    private bool isSprinting = false; // Is the player sprinting?
    private bool isCrouching = false; // Is the player crouching?
    private CharacterController characterController; // Character controller component

    // Initialize variables
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    // Update is called once per frame
    void Update()
    {
        // Call methods to handle movement and camera control
        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
    }

    // Handle player movement
    private void HandleMovement()
    {
        // Get input axes: WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Determine current speed
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Move the player
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
        }

        // Play footstep sounds
        if (characterController.isGrounded && move.magnitude > 0) // Only play footstep sounds when moving
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
        }
        else
        {
            footstepAudio.Stop(); // Stop sound if not moving
        }
    }

    // Handle mouse look functionality
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // invert mouse look
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // clamp the vertical rotation

        playerBody.Rotate(Vector3.up * mouseX); // Rotate player body
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera
    }

    // Handle crouch functionality
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Use Left Ctrl to crouch
        {
            isCrouching = !isCrouching; // Toggle crouching
            characterController.height = isCrouching ? 1f : 2f; // Change character height based on crouch state
        }
    }
}