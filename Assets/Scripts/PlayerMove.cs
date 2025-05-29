using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] InputAction move;
    Rigidbody rb;
    float speed = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        move.Enable();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var direction = move.ReadValue<Vector3>().normalized;

        if (move.IsPressed())
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);

            rb.linearDamping = 0f;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearDamping = 4f;
        }
    }
}

