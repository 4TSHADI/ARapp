using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The object to orbit around
    public float rotationSpeed = 100f; // Speed of rotation
    public float zoomSpeed = 2f; // Speed of zoom
    public float minZoom = 2f; // Minimum zoom distance
    public float maxZoom = 20f; // Maximum zoom distance
    [SerializeField] private FixedJoystick joystick;

    private float distance = 10f; // Current distance from the target
    private float currentX = 0f; // Current X-axis rotation
    private float currentY = 0f; // Current Y-axis rotation

    public float rotationSmoothTime = 0.1f; // Smaller value = more smoothing
    private float targetX;
    private float targetY;
    private float smoothX;
    private float smoothY;

    void Update()
    {
        // Capture target angles from joystick input
        targetX += joystick.Horizontal * rotationSpeed;
        targetY -= joystick.Vertical * rotationSpeed;
        targetY = Mathf.Clamp(targetY, -80f, 80f);

        // Smoothly interpolate current rotation values toward target
        currentX = Mathf.Lerp(currentX, targetX, rotationSmoothTime);
        currentY = Mathf.Lerp(currentY, targetY, rotationSmoothTime);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

}
