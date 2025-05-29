using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] InputAction attack;
    float suckRange = 8f;
    float suckPower = 10f;
    

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
    }

    private void Suck()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, suckRange,LayerMask.GetMask("Item"));
        
        foreach (var target in targets)
        {
            if (target.attachedRigidbody != null)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < 0.7f)
                {
                    target.attachedRigidbody.linearVelocity = Vector3.zero;
                    continue;
                }

                Vector3 dir = (transform.position - target.transform.position).normalized;
                float power = Mathf.Lerp(suckPower, 0, distance / suckRange);
                target.attachedRigidbody.AddForce(dir * power, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.darkRed;
        Gizmos.DrawWireSphere(transform.position, suckRange);
    }

    private void Blow()
    {

    }
}
