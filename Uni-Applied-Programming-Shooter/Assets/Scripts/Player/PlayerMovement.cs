using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    private Rigidbody rb;


    private InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 moveForce = new Vector3(inputVector.x, 0f, inputVector.y);
        rb.AddForce(moveForce * moveSpeed);
    }

}
