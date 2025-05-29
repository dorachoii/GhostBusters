using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] InputAction attack;
    float suckRange = 8f;
    float suckPower = 5f;
    float suckAngle = 60f;

    public Transform hand;
    public GameObject[] FX;

    bool isFXInstantiated = false;


    void OnEnable()
    {
        attack.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }

    private void Attack()
    {
        float direction = attack.ReadValue<float>();
        if (attack.IsPressed())
        {
            if (direction < 0) Suck();
            else Blow();
        }
        else
        {
            OffFX();
        }
    }

    private void Suck()
    {
        ApplyVacuum(1);
        OnFX(0);
    }



    private void Blow()
    {
        ApplyVacuum(-1);
        OnFX(1);
    }

    private void OnFX(int idx)
    {
        if (!isFXInstantiated)
        {
            if (idx == 0)
            {
                Instantiate(FX[idx], hand.position + hand.transform.forward * 2, hand.rotation, hand);
            }
            else
            {
                Instantiate(FX[idx], hand.position, hand.rotation, hand);
            }
            
            isFXInstantiated = true;
        }
    }

    private void OffFX()
    {
        foreach (Transform child in hand)
        {
            Destroy(child.gameObject);
        }
        isFXInstantiated = false;
    }


    private void ApplyVacuum(float direction)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, suckRange, LayerMask.GetMask("Item"));

        Vector3 forward = transform.forward;
        float halfFOV = suckAngle * 0.5f;

        foreach (var target in targets)
        {
            if (target.attachedRigidbody != null)
            {
                Vector3 dir = (transform.position - target.transform.position).normalized;
                float angle = Vector3.Angle(forward, dir);
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < 0.7f)
                {
                    target.attachedRigidbody.linearVelocity = Vector3.zero;
                    continue;
                }

                if (angle < halfFOV) continue;

                float power = Mathf.Lerp(suckPower, 0, distance / suckRange);
                target.attachedRigidbody.AddForce(direction * dir * power, ForceMode.Impulse);
            }
        }
    }
    
        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.darkRed;
        Gizmos.DrawWireSphere(transform.position, suckRange);

        Vector3 forward = transform.forward;
        float halfFOV = suckAngle * 0.5f;

        Quaternion leftRotation = Quaternion.Euler(0, -halfFOV, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfFOV, 0);

        Vector3 leftDir = leftRotation * forward;
        Vector3 rightDir = rightRotation * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftDir * suckRange);
        Gizmos.DrawRay(transform.position, rightDir * suckRange);
    }
}
