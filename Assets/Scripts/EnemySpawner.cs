using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Oluşturulacak düşman prefabı
    public string playerTag = "Player"; // Oyuncunun tag'i
    public float spawnDistance = 5f;    // Oyuncuya olan mesafe
    public float spawnInterval = 1f;    // Oluşturma aralığı (saniye)
    
    private GameObject player; // Oyuncu nesnesi
    private float timer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Update()
    {
        if (player == null)
        {
            return; // Oyuncu nesnesi bulunamadıysa çık
        }

        // Belirli aralıklarla düşman oluştur
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        Vector3 spawnPosition = player.transform.position + randomDirection * spawnDistance;
        spawnPosition.y = 0f;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
