using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player info
    public int health;
    public int maxHealth;

    [SerializeField] private float speed;
    [SerializeField] private float attackInterval;
    [SerializeField] private float iInterval; //invincibility interval

    [SerializeField] private List<int> playerSkills = new List<int>();
    /* skills stored with a specific index, may use dictionary instead:
    0 = damage - how much extra damage to deal; implemented
    1 = shot count - how many extra shots to shoot;
    3 = aura count - how many balls spin around you;  */
    
    private float attackTimer;
    private float iTimer; //invincibility timer
    private int currentAuraCount;

    public int level = 1;
    public int xp;
    public int requiredXp;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteFlash flashEffect;

    private InputAction moveAction;
    private InputAction attackAction;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject auraPrefab;

    [SerializeField] private Transform auraParent;

    [SerializeField] private GameObject LevelUpScreen;

    private Vector3 mousePosition;

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flashEffect = GetComponent<SpriteFlash>();

        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];

        health = maxHealth;
        
        requiredXp = GetXpForLevel(level);
    }

    public void Shoot() {
        for (int i = 0; i < playerSkills[1] + 1; i++) {
            float range = 30f; // shot max cone size;
            float degreesOffset = i*(range/(playerSkills[1] + 1)) + (range/(playerSkills[1] + 1))/2f + 90 - (range/2);
            float degrees = Vector3.SignedAngle(Vector3.up, (mousePosition - transform.position).normalized, Vector3.forward);
            Vector3 direction = new Vector3(Mathf.Cos((degrees + degreesOffset) * Mathf.Deg2Rad), Mathf.Sin((degrees + degreesOffset) * Mathf.Deg2Rad), 0f).normalized;
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().Initialize(direction, 1 + playerSkills[0], 1 + playerSkills[3]);
            Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        animator.Play("Attack");
        attackTimer = attackInterval;
    }

    public void ReceiveDamage(int damage) {
        if (iTimer <= 0f) {
            health -= damage;
            if (health <= 0) {
                Destroy(this.gameObject);
            }
            else {
                flashEffect.Flash();
            }
            iTimer = iInterval;
        }
    }

    void SyncAuraCount() {
        Vector3 auraPosition = transform.position;
        auraPosition.y += 1f;
        for (int i = 0; i < playerSkills[2] - currentAuraCount; i++) {
            GameObject newAura = Instantiate(auraPrefab, transform.position, Quaternion.identity, auraParent.transform);
            newAura.GetComponentInChildren<AuraController>().Initialize(playerSkills[0] + 1);
        }
        currentAuraCount = playerSkills[2];
        for (int i = 0; i < auraParent.childCount; i++) {
            auraParent.GetChild(i).transform.rotation = Quaternion.Euler(0f, 0f, (360f/currentAuraCount) * i);
        }
    }

    void LevelUp() {
        xp = 0;
        level++;
        requiredXp = GetXpForLevel(level);
        LevelUpScreen.SetActive(true);
        Pause();
    }

    int GetXpForLevel(int currentLevel) {
        if (currentLevel <= 0) {
            return 0;
        }
        return ((currentLevel * 20) + GetXpForLevel(currentLevel - 1));
    }

    void Pause() {
        Time.timeScale = 0f;
    }

    public void Unpause() {
        Time.timeScale = 1f;
    }

    public void SkillUp(int i) {
        playerSkills[i]++;
        SyncAuraCount();
    } 

    public void Heal(int i) {
        health += i;
        if (health > maxHealth) health = maxHealth;
    }

    void Update() {
        Vector2 newMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.x = newMousePosition.x;
        mousePosition.y = newMousePosition.y;

        Vector2 input = moveAction.ReadValue<Vector2>();
        rb.velocity = new Vector2(input.x * speed, input.y * speed);

        if (mousePosition.x - transform.position.x > 0f) {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        attackTimer -= Time.deltaTime;
        iTimer -= Time.deltaTime;

        if (attackTimer <= 0f) {
            Shoot();
        }

        auraParent.Rotate(Vector3.forward * 0.1f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "XP") {
            xp++;
            if (xp >= requiredXp) {
                LevelUp();
            }
            Destroy(collision.gameObject);
        }
    }
}
