using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{
    int damage;

    public void Initialize(int newDamage) {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().ReceiveDamage(damage);
        }
    }
}
