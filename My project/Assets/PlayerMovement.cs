using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 100f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!GameManager.Instance || !GameManager.Instance.IsPlaying())
            return;

        // WASD movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * v + transform.right * h;

        // Q/E rotation
        float turn = 0f;
        if (Input.GetKey(KeyCode.Q)) turn = -1f;
        if (Input.GetKey(KeyCode.E)) turn = 1f;
        transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);

        controller.Move(move * speed * Time.deltaTime);
    }

    public bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
               Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;
    }
}
