using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float timeToLive;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int health;
    private List<GameObject> hits = new List<GameObject>();

    private Animator animator;

    public Vector3 direction;

    private bool hit = false;

    void Awake() {
        Destroy(gameObject, timeToLive);
        animator = GetComponent<Animator>();
    }

    public void Initialize(Vector3 newDirection, int newDamage, int newHealth) {
        direction = newDirection;
        damage = newDamage;
        health = newHealth;
    }

    void Update() {
        transform.position += direction * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy" && !hits.Contains(collision.gameObject)) {
            collision.gameObject.GetComponent<EnemyController>().ReceiveDamage(damage);
            hits.Add(collision.gameObject);
            health--;
            if (health <= 0) {
                direction = Vector3.zero;
                animator.Play("BulletFade");
            }
        }
    }
}
