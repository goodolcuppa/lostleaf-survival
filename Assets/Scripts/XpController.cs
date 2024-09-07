using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpController : MonoBehaviour
{
    private Rigidbody2D rb;

    private Transform targetTransform;

    public void Initialize(Transform newTargetTransform) {
        rb = GetComponent<Rigidbody2D>();
        targetTransform = newTargetTransform;
    }

    void Update() {
        if (targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) < 3f) {
            Vector2 direction = (targetTransform.position - transform.position).normalized;
            rb.velocity = direction * 4f;
        }
    }
}
