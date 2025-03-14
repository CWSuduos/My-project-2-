using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sensitivity = 2f;
    public Transform cameraTransform;
    private float rotationX = 0f;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private Transform playerBody;
    private bool isJoystickMoving = false;

    void Start()
    {
        if (cameraTransform == null) Debug.LogError("CameraLook: Не задана камера!");
        else playerBody = cameraTransform.parent;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
        isJoystickMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;
    }

    void HandleTouchInput()
    {
        if (!isJoystickMoving && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width * 0.3f)
            {
                if (touch.phase == TouchPhase.Began) { lastTouchPosition = touch.position; isDragging = true; }
                else if (touch.phase == TouchPhase.Moved && isDragging) { Vector2 delta = touch.position - lastTouchPosition; RotateCamera(delta.x, delta.y); lastTouchPosition = touch.position; }
                else if (touch.phase == TouchPhase.Ended) isDragging = false;
            }
        }
    }

    void HandleMouseInput()
    {
        if (!isJoystickMoving)
        {
            if (Input.GetMouseButtonDown(0)) { isDragging = true; lastTouchPosition = Input.mousePosition; }
            else if (Input.GetMouseButton(0) && isDragging) { Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition; RotateCamera(delta.x, delta.y); lastTouchPosition = Input.mousePosition; }
            else if (Input.GetMouseButtonUp(0)) isDragging = false;
        }
    }

    void RotateCamera(float deltaX, float deltaY)
    {
        if (cameraTransform == null || playerBody == null) return;
        playerBody.Rotate(Vector3.up * deltaX * sensitivity / Screen.width);
        rotationX -= deltaY * sensitivity / Screen.height;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
