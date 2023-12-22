using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotSpeed = 2f;
    public float fastMul = 3f;
    public float sloMul = 0.25f;

    float rotationX;
    float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * rotSpeed;
        rotationY += Input.GetAxis("Mouse Y") * rotSpeed;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        float currentMoveSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) currentMoveSpeed *= fastMul;
        if (Input.GetKey(KeyCode.LeftControl)) currentMoveSpeed *= sloMul;

        float horizontal = Input.GetAxis("Horizontal") * currentMoveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * currentMoveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(horizontal, 0, vertical));

        if (Input.GetKey(KeyCode.Q)) transform.Translate(Vector3.down * currentMoveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
    }
}
