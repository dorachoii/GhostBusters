using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStats stats;

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
        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
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
            controller.StateMachine.TransitionTo(controller.StateMachine.idleState);
        }
    }

    private void Suck()
    {
        controller.StateMachine.TransitionTo(controller.StateMachine.suckState);
        ApplyVacuum(1);
        OnFX(0);
    }



    private void Blow()
    {
        controller.StateMachine.TransitionTo(controller.StateMachine.blowState);
        ReleaseItems();
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
                    if (!target.attachedRigidbody.isKinematic)
                    {
                        StoreItem(target);
                    }
                    continue;
                }

                if (angle < halfFOV) continue;

                float power = Mathf.Lerp(suckPower, 0, distance / suckRange);
                target.attachedRigidbody.AddForce(direction * dir * power, ForceMode.Impulse);
            }
        }
    }

    private void StoreItem(Collider target)
    {
        target.attachedRigidbody.linearVelocity = Vector3.zero;

        if (target.gameObject.CompareTag("Item"))
        {
            if (transform.GetChild(1).childCount > stats.maxItem) return;
            Debug.Log("d" + transform.GetChild(1).childCount);
            target.attachedRigidbody.isKinematic = true;
            target.transform.SetParent(transform.GetChild(1), true);
            stats.GainItem();
        }
        else if (target.gameObject.CompareTag("Heart"))
        {
            target.gameObject.GetComponent<HealItem>().Heal(stats);
        }
        
    }

    private void ReleaseItems()
    {
        Transform pocket = transform.GetChild(1);
        Vector3 forward = transform.forward;

        List<Transform> itemsToRelease = new List<Transform>();

        foreach (Transform child in pocket)
        {
            itemsToRelease.Add(child);
        }

        foreach (Transform child in itemsToRelease)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                child.SetParent(null);
                child.position += forward * 0.5f;
                rb.isKinematic = false;
                rb.AddForce(forward * 10f + Vector3.up * 3f, ForceMode.Impulse);
                stats.LoseItem();
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
