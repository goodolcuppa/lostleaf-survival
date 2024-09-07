using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Enemy info
    [SerializeField] private int health;
    [SerializeField] private float attackInterval;
    [SerializeField] private float speed;

    private float attackTimer;
    private float waitTime = 0.5f;

    private Transform targetTransform;

    private Rigidbody2D rb;
    private AudioSource audio;
    private SpriteFlash flashEffect;

    [SerializeField] private GameObject xpPrefab;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        flashEffect = GetComponent<SpriteFlash>();
    }

    public void Initialize(Transform newTargetTransform) {
        targetTransform = newTargetTransform;
    }

    public void ReceiveDamage(int damage) {
        health -= damage;
        audio.Play();
        if (health < 0) {
            GameObject xpDrop = Instantiate(xpPrefab, transform.position, Quaternion.identity);
            xpDrop.GetComponent<XpController>().Initialize(targetTransform);
            Destroy(this.gameObject);
        }
        else {
            flashEffect.Flash();
        }
    }

    void Update() {
        if (targetTransform != null && waitTime <= 0f) {
            Vector2 direction = (targetTransform.position - transform.position).normalized;
            if (direction.x > 0f) {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            rb.velocity = direction * speed;
        }

        attackTimer -= Time.deltaTime;
        if (waitTime > 0f) {
            waitTime -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && attackTimer <= 0) {
            collision.gameObject.GetComponent<PlayerController>().ReceiveDamage(1);
            attackTimer = attackInterval;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && attackTimer <= 0) {
            collision.gameObject.GetComponent<PlayerController>().ReceiveDamage(1);
            if (GetComponent<Animator>() != null) {
                GetComponent<Animator>().Play("BlobAttack");
            }
            attackTimer = attackInterval;
        }
    }
}
