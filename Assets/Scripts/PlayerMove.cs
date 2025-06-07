using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動を制御するクラスです。
/// 物理演算を使用して滑らかな移動と回転を実現します。
/// 
/// 実装の特徴:
/// - Rigidbodyを使用した物理ベースの移動
/// - スムーズな回転補間
/// - 状態パターンとの連携による移動状態の管理
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private InputAction move;
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
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    /// <summary>
    /// 物理演算の更新処理を行います。
    /// FixedUpdateを使用することで、安定した物理演算を実現します。
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// プレイヤーの移動と回転を制御します。
    /// 入力に応じて移動方向を決定し、物理演算を使用して移動を実行します。
    /// 移動中はWalkStateに、停止時はIdleStateに遷移します。
    /// </summary>
    private void Move()
    {
        Vector3 direction = move.ReadValue<Vector3>().normalized;

        if (move.IsPressed())
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

            // 物理演算を使用して移動
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

