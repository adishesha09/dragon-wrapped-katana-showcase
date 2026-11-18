using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    [Header("Rotation")]
    public float dragSpeed = 0.4f;     
    public float rollSpeed = 90f;      
    public float damping = 8f;         

    [Header("Zoom")]
    public Camera viewCamera;          
    public float zoomSpeed = 0.5f;
    public float minDistance = 0.8f;
    public float maxDistance = 8f;

    private Quaternion startRotation;
    private Vector3 startCamPos;
    private Vector2 velocity;          
    private Vector3 lastMouse;

    void Start()
    {
        if (viewCamera == null) viewCamera = Camera.main;
        startRotation = transform.rotation;
        if (viewCamera != null) startCamPos = viewCamera.transform.position;
    }

    void Update()
    {
        HandleDrag();
        HandleRoll();
        HandleZoom();

        if (Input.GetKeyDown(KeyCode.R)) ResetView();
    }

    void HandleDrag()
    {
        Vector2 delta = Vector2.zero;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            delta = Input.GetTouch(0).deltaPosition;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            lastMouse = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            delta = (Vector2)(Input.mousePosition - lastMouse);
            lastMouse = Input.mousePosition;
        }

        if (delta != Vector2.zero) velocity = delta * dragSpeed;
        else velocity = Vector2.Lerp(velocity, Vector2.zero, damping * Time.deltaTime);

        if (velocity.sqrMagnitude < 0.0001f) return;

        Transform cam = viewCamera != null ? viewCamera.transform : Camera.main.transform;

        transform.Rotate(cam.up, -velocity.x, Space.World);
        transform.Rotate(cam.right, velocity.y, Space.World);
    }

    void HandleRoll()
    {
        Transform cam = viewCamera != null ? viewCamera.transform : Camera.main.transform;
        float roll = 0f;
        if (Input.GetKey(KeyCode.Q)) roll += 1f;
        if (Input.GetKey(KeyCode.E)) roll -= 1f;
        if (roll != 0f)
            transform.Rotate(cam.forward, roll * rollSpeed * Time.deltaTime, Space.World);
    }

    void HandleZoom()
    {
        if (viewCamera == null) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Input.touchCount == 2)
        {
            Touch a = Input.GetTouch(0), b = Input.GetTouch(1);
            float prev = ((a.position - a.deltaPosition) - (b.position - b.deltaPosition)).magnitude;
            float now = (a.position - b.position).magnitude;
            scroll = (now - prev) * 0.01f;
        }
        if (Mathf.Approximately(scroll, 0f)) return;

        Vector3 toModel = transform.position - viewCamera.transform.position;
        float dist = toModel.magnitude;
        float newDist = Mathf.Clamp(dist - scroll * zoomSpeed, minDistance, maxDistance);
        viewCamera.transform.position = transform.position - toModel.normalized * newDist;
    }

    public void ResetView()
    {
        transform.rotation = startRotation;
        velocity = Vector2.zero;
        if (viewCamera != null) viewCamera.transform.position = startCamPos;
    }
}