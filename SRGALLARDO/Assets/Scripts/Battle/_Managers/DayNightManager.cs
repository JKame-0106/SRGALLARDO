using UnityEngine;
using System.Collections;

public enum EGamePhase
{
    Day,
    Night,
    Meantime,
    Victory,
    GameOver
}

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance { get; private set;}

    [Header("Referencias")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Dias")]
    [SerializeField] private int iDaysTotal = 5;
    [SerializeField] private float[] arrHealthMultiplierPerDay = {1f, 1.25f, 2f, 3f, 5f};
    [SerializeField] private float[] arrCornMultiplierPerDay = {1f, 1.25f, 1.5f, 2f, 3f};

    [Header("Waves")]
    [SerializeField] private int iWavesPerDay = 5;
    [SerializeField] private float fWaveDuration = 20f;
    [SerializeField] private float fMeantimeDuration = 10f;
    [SerializeField] private float fSpeedMultiplierStart = 1f;
    [SerializeField] private float fSpeedMultiplierFinal = 2f;
    [SerializeField] private UIScreen uiScreen;

    private int _iActualDay = 1;
    private int iActualDay
    {
        get { return _iActualDay; }
        set
        {
            _iActualDay = value;
            uiScreen.dayCount.text = "Day " + _iActualDay;
        }
    }
    private int iActualWave = 0;
    private EGamePhase eActualPhase = EGamePhase.Day;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        iActualDay = 1;
        StartPhaseDay();
    }

    void StartPhaseDay()
    {
        eActualPhase = EGamePhase.Day;
        iActualWave = 0;

        if (enemySpawner != null)
            enemySpawner.SetActiveSpawn(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDayPhase();

        Debug.Log(" Dia" + iActualDay + " - Fase de compras");
    }

    public void StartNight()
    {
        Debug.Log("StartNight() llamado. Fase actual: " + eActualPhase + " | TimeScale: " + Time.timeScale);
        uiScreen.isSleeping = true;
        if (eActualPhase != EGamePhase.Day) return;
        StartCoroutine(NightSecuence());
    }

    IEnumerator NightSecuence()
    {
        eActualPhase = EGamePhase.Night;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayNightPhase();

        float fHealthMultiplier = arrHealthMultiplierPerDay[iActualDay - 1];
        float fCornMultiplier = arrCornMultiplierPerDay[iActualDay - 1];
        if (enemySpawner != null)
        {
            enemySpawner.SetHealthMultiplier(fHealthMultiplier);
            enemySpawner.SetCornMultiplier(fCornMultiplier);
        }

        Debug.Log("Dia " + iActualDay + " - Comienza la noche (vida enemiga x" + fHealthMultiplier + ")--");

        for (int iWave = 1; iWave <= iWavesPerDay; iWave++)
        {
            iActualWave = iWave;
                yield return StartCoroutine(ExecuteWave(iWave));

            if (iWave < iWavesPerDay)
                yield return StartCoroutine(Meantime());
        }

        EndNight();
    }

    IEnumerator ExecuteWave(int iWave)
    {
        Debug.Log("--- Oleada " + iWave + "/" + iWavesPerDay + " ---");

        if (enemySpawner != null)
            enemySpawner.SetActiveSpawn(true);

        float fTimer = 0f;
        while (fTimer < fWaveDuration)
        {
            fTimer += Time.deltaTime;
            float fProgreso = fTimer / fWaveDuration;
            float fCurrentSpeed = Mathf.Lerp(fSpeedMultiplierStart, fSpeedMultiplierFinal, fProgreso);

            if (enemySpawner != null)
                enemySpawner.SetSpeedMultiplier(fCurrentSpeed);

            yield return null;
        }

        if (enemySpawner != null)
            enemySpawner.SetActiveSpawn(false);
    }

    IEnumerator Meantime()
    {
        eActualPhase = EGamePhase.Meantime;
        Debug.Log("Pausa entre oleadas - puedes comprar mejoras (" + fMeantimeDuration + "s)");

        yield return new WaitForSeconds(fMeantimeDuration);

        eActualPhase = EGamePhase.Night;
    }

     void EndNight()
    {
        Debug.Log("=== DIA " + iActualDay + " completado ===");

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.HealCoop(1);
        }

        if (iActualDay >= iDaysTotal)
        {
            Victory();
        }
        else
        {
            iActualDay++;
            StartPhaseDay();
        }
    }

    void Victory()
    {
        eActualPhase = EGamePhase.Victory;
        Debug.Log("VICTORIA - El Senor Gallardo sobrevivio los " + iDaysTotal + " dias!");

        if (enemySpawner != null)
            enemySpawner.SetActiveSpawn(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayVictory();
    }

    public void ForceGameOver()
    {
        StopAllCoroutines();
        eActualPhase = EGamePhase.GameOver;

        if (enemySpawner != null)
            enemySpawner.SetActiveSpawn(false);
    }

    public EGamePhase GetActualPhase() => eActualPhase;
    public int GetActualDay() => iActualDay;
    public int GetActualWave() => iActualWave;
}
