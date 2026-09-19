using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
 [Header("Referenciass")]
 [SerializeField] private GameObject goEnemyPrefab;

 [Header("Configuracion de carril")]
 [SerializeField] private float[] fLaneSpawnPositionsX = {-4f, 0f, 4f};
 [SerializeField] private float fSpawnY = 6f;

 [Header("Spawn Rate")]
 //[SerializeField] private float fSpawnCooldown = 2f;
 //[SerializeField] private bool bActiveSpawn = true;

 [Header("Escalado (DayNightManager)")]
 [SerializeField] private float fHealthMultiplier = 1f;
 [SerializeField] private float fSpeedMultiplier = 1f;
 [SerializeField] private float fCornMultiplier = 1f;

 [Header("Ritmo de aparicion")]
[SerializeField] private float fBaseSpawnInterval = 2f; 
[SerializeField] private float fMinSpawnInterval = 0.4f; 
[SerializeField] private bool bSpawnActivo = true;

 private float fTimer = 0f;

 void Update()
    {
        if (!bActiveSpawn) return;

        fTimer -= Time.deltaTime;

        if (fTimer <= 0f)
        {
            SpawnEnemy();
            fTimer = GetCurrentSpawnInterval();
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

            enemy.ApplyHealthMultiplier(fHealthMultiplier);
            enemy.ApplySpeedMultiplier(fSpeedMultiplier);
            enemy.ApplyCornMultiplier(fCornMultiplier);
        }
    }

    public void SetActiveSpawn(bool bActive)
    {
        bActiveSpawn = bActive;
    }

    public void SetHealthMultiplier(float fMultiplier)
    {
        fHealthMultiplier = fMultiplier;
    }

    public void SetSpeedMultiplier(float fMultiplier)
    {
        fSpeedMultiplier = fMultiplier;
    }

    public void SetCornMultiplier(float fMultiplicador)
    {
        fCornMultiplier = fMultiplicador;
    }

    float GetCurrentSpawnInterval()
    {
        float fScaleInterval = fBaseSpawnInterval / fSpeedMultiplier;
        return Mathf.Max(fScaleInterval, fMinSpawnInterval);
    }
}
