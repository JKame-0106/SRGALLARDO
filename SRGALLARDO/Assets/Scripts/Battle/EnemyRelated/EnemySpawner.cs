using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
 [Header("Referenciass")]
 [SerializeField] private GameObject goEnemyPrefab;

 [Header("Configuracion de carril")]
 [SerializeField] private float[] fLaneSpawnPositionsX = {-4f, 0f, 4f};
 [SerializeField] private float fSpawnY = 6f;

 [Header("Spawn Rate")]
 [SerializeField] private float fSpawnCooldown = 2f;
 [SerializeField] private bool bActiveSpawn = true;

 private float fTimer = 0f;

 void Update()
    {
        if (!bActiveSpawn) return;

        fTimer -= Time.deltaTime;

        if (fTimer <= 0f)
        {
            SpawnEnemy();
            fTimer = fSpawnCooldown;
        }
    }   

void SpawnEnemy()
    {
        int iLane = Random.Range(0, fLaneSpawnPositionsX.Length);
        Debug.Log("Spawneando en carril " + iLane + " (X = " + fLaneSpawnPositionsX[iLane] + ")");

        Vector3 vSpawnPos = new Vector3(fLaneSpawnPositionsX[iLane], fSpawnY, 0f);

        GameObject goEnemy = Instantiate(goEnemyPrefab, vSpawnPos, Quaternion.identity);

        Enemy enemy = goEnemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            int iTypeQuantity = System.Enum.GetValues(typeof(EColorType)).Length;
            EColorType eRandomType = (EColorType)Random.Range(0, iTypeQuantity);
            enemy.SetEnemyType(eRandomType);
        }
    }

    public void SetActiveSpawn(bool bActive)
    {
        bActiveSpawn = bActive;
    }
}
