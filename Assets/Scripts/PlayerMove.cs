using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 여기 고칠 필요 있음. 각속도가 고정이 안되는 것 같음음

public class PlayerMove : MonoBehaviour
{
    [SerializeField] InputAction move;
    Rigidbody rb;
    float speed = 10f;
    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
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
        Vector3 direction = move.ReadValue<Vector3>().normalized;

        if (move.IsPressed())
        {
            controller.StateMachine.TransitionTo(controller.StateMachine.walkState);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);

            Vector3 currentPos = rb.position;
            Vector3 newPos = currentPos + direction * (speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

        }
        else
        {
            if (controller.StateMachine.CurrentState == controller.StateMachine.suckState || controller.StateMachine.CurrentState == controller.StateMachine.blowState) return;
            controller.StateMachine.TransitionTo(controller.StateMachine.idleState);
        }
    }
}

