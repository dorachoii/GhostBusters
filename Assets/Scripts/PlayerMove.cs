using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Playerの移動を制御するクラスです。
/// </summary>

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private InputAction moveInput;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float linearDamping = 3f;

    private Rigidbody rb;
    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDamping;
    }

    private void OnEnable()
    {
        moveInput.Enable();
    }

    private void OnDisable()
    {
        moveInput.Disable();
    }

    /// FixedUpdateを使用することで、安定した物理演算を実現
    private void FixedUpdate()
    {
        Move();
    }

    /// プレイヤーの移動と回転を制御
    private void Move()
    {
        Vector3 direction = moveInput.ReadValue<Vector3>().normalized;

        if (moveInput.IsPressed())
        {
            // 移動状態に遷移
            controller.StateMachine.TransitionTo(controller.StateMachine.walkState);

            // 移動方向に向かって滑らかに回転
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                Time.fixedDeltaTime * rotationSpeed
            );

            // rigidbody物理演算を使用して移動
            Vector3 currentPos = rb.position;
            Vector3 newPos = currentPos + direction * (moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            // 攻撃中でない場合のみ待機状態に遷移
            if (controller.StateMachine.CurrentState != controller.StateMachine.suckState && 
                controller.StateMachine.CurrentState != controller.StateMachine.blowState)
            {
                controller.StateMachine.TransitionTo(controller.StateMachine.idleState);
            }
        }
    }
}