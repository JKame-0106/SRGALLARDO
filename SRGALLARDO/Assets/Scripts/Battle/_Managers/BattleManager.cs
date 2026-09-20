using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set;}

    [Header("Configuración de Derrota")]
    [SerializeField] private int iMaxEnemiesReachedCoop = 5;
    [SerializeField] UIScreen uiScreen;

    private int iEnemiesReachedCoop = 0;
    private bool bGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterEnemyReachedCoop()
    {
        iEnemiesReachedCoop++;
        
    if (uiScreen != null)
    {
        uiScreen.HurtGallardo();
    }
    else
    {
        Debug.LogWarning("[BattleManager] UIScreen no está asignado en el Inspector, no se puede hacer la animación de daño.");
    }

        if (iEnemiesReachedCoop >= iMaxEnemiesReachedCoop)
        {
            GameOver();
        }
    }

    public void HealCoop(int iAmount)
    {
        if (bGameOver) return;

        iEnemiesReachedCoop = Mathf.Max(0, iEnemiesReachedCoop - iAmount);
        Debug.Log("Se ha encontrado un infiltrado!! Enemigos en el gallinero: " + iEnemiesReachedCoop + "/" + iMaxEnemiesReachedCoop);
    }

    void GameOver()
    {
        bGameOver = true;
        Debug.Log("GAME OVER - Gallardo ha perdido todo...");

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameOver();

        if (DayNightManager.Instance != null)
        {
            DayNightManager.Instance.ForceGameOver();
        }

        Time.timeScale = 0f;
    }

    public bool IsGameOver() => bGameOver;
}