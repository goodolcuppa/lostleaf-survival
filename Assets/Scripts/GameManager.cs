using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    // Prefabs
    [SerializeField] private GameObject blobPrefab;
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private Transform enemyParent;

    // UI
    [SerializeField] private RectTransform fullHearts;
    [SerializeField] private RectTransform halfHearts;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    private int enemiesSpawned;
    private float spawnCooldown;

    private void SpawnEnemy(GameObject prefab) {
        GameObject newEnemy = Instantiate(prefab, GetEnemySpawn(), Quaternion.identity, enemyParent);
        newEnemy.GetComponent<EnemyController>().Initialize(player.transform);
        enemiesSpawned++;
    }

    private Vector3 GetEnemySpawn() {
        return Random.insideUnitCircle.normalized * 12f;
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }

    void Update() {
        float fullLength = (player.health / 2);
        fullHearts.sizeDelta = new Vector2(fullLength * 64f, 64f);
        halfHearts.sizeDelta = new Vector2((fullLength + (player.health % 2)) * 64f, 64f);
        xpSlider.value = player.xp / (float)player.requiredXp;
        levelText.text = "Exp Lv " + player.level.ToString();

        if (player.health <= 0) {
            scoreText.text = "You Died\n\nScore " + enemiesSpawned.ToString();
            deathScreen.SetActive(true);
        }

        if (enemyParent.childCount == 0) {
            Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, enemyParent);
        }

        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown <= 0f) {
            for (int i = 0; i < 5; i++) {
                if (enemiesSpawned % 5 == 0)
                    SpawnEnemy(bossPrefab);
                else {
                    SpawnEnemy(blobPrefab);
                }
            }
            spawnCooldown = 3f;
        }
    }
}
