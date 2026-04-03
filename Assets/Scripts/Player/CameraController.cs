using UnityEngine;

// CameraController manages the first-person camera behavior including look, head bob, and FOV changes.
public class CameraController : MonoBehaviour
{
    // Camera settings
    public float mouseSensitivity = 100f;
    public float headBobAmount = 0.05f;
    public float headBobSpeed = 5f;
    public float defaultFOV = 60f;
    public float sprintFOV = 75f;

    private float xRotation = 0f;
    private float headBobTimer = 0f;
    private Vector3 cameraStartPos;
    private Camera mainCamera;
    private PlayerInput playerInput;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        playerInput = GetComponentInParent<PlayerInput>();
        cameraStartPos = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to screen center
    }

    void Update()
    {
        HandleMouseLook();
        HandleHeadBob();
        HandleFOV();
    }

    // Handle mouse look
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    // Handle head bob animation while moving
    private void HandleHeadBob()
    {
        if (playerInput.IsMoving())
        {
            headBobTimer += Time.deltaTime * headBobSpeed;
            float newY = cameraStartPos.y + Mathf.Sin(headBobTimer) * headBobAmount;
            transform.localPosition = new Vector3(cameraStartPos.x, newY, cameraStartPos.z);
        }
        else
        {
            headBobTimer = 0f;
            transform.localPosition = cameraStartPos;
        }
    }

    // Handle FOV changes during sprint
    private void HandleFOV()
    {
        float targetFOV = playerInput.isSprinting ? sprintFOV : defaultFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * 5f);
    }

    // Apply camera shake for impacts or explosions
    public void ShakeCamera(float duration, float intensity)
    {
        StartCoroutine(CameraShakeCoroutine(duration, intensity));
    }

    private System.Collections.IEnumerator CameraShakeCoroutine(float duration, float intensity)
    {
        float elapsed = 0f;
        Vector3 originalPos = transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}