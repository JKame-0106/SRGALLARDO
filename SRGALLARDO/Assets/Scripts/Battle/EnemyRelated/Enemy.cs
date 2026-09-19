using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int iHealth = 10;
    [SerializeField] private float fMoveSpeed = 2f;
    [SerializeField] private EColorType eEnemyType = EColorType.Rosado;
    [SerializeField] private int iCornDrop = 5;

    [Header("Limites")]
    [SerializeField] private float fDespawnY = -6f;

    [Header("Sprites por tipos: R-A-V")]
    [SerializeField] private Sprite[] arrSpritesPerType;

    private SpriteRenderer spriteRenderer;
    private bool bIsHere = false;
    private float fSpeedMultiplier = 1f;
    private float fHealthMultiplier = 1f;
    private float fCornMultiplier = 1f;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyVisualColor();
    }
    void Update()
    {
        transform.position += Vector3.down * fMoveSpeed * Time.deltaTime;

        if(transform.position.y <= fDespawnY)
        {
            ReachedCoop();
        }
    }

    public void TakeDamage(int iAmount)
    {
        iHealth -= iAmount;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayImpact();
        }

        if (iHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        EggLauncher launcher = other.GetComponent<EggLauncher>();
        if (launcher != null)
        {
            ReachedCoop();
        }
    }

    public void SetEnemyType(EColorType eNewType)
    {
        eEnemyType = eNewType;
        ApplyVisualColor();
    }

    void ReachedCoop()
    {
        if (bIsHere) return;
        bIsHere = true;

        Debug.Log(eEnemyType + " llego al granero!");

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.RegisterEnemyReachedCoop();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayReachedCoop();
        }

        Destroy(gameObject);
    }
    void Die()
    {
        int iCornFinal= Mathf.RoundToInt(iCornDrop * fCornMultiplier);

        Debug.Log(eEnemyType + " derrotado. Suelta " + iCornDrop + " de maiz.");

        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.AddCorn(iCornFinal);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDefeat(eEnemyType);
        }
        Destroy(gameObject);
    }

    void ApplyVisualColor()
    {
        if (spriteRenderer == null || arrSpritesPerType == null) return;

        int iIndex = (int)eEnemyType;
        if (iIndex < arrSpritesPerType.Length)
        {
            spriteRenderer.sprite = arrSpritesPerType[iIndex];
        }
    }

    public void ApplySpeedMultiplier(float fMultiplier)
    {
        fSpeedMultiplier = fMultiplier;
    }

    public void ApplyHealthMultiplier(float fMultiplier)
    {
        fHealthMultiplier = fMultiplier;
    }

    public void ApplyCornMultiplier(float fMultiplier)
    {
        fCornMultiplier = fMultiplier;
    }

    public EColorType GetEnemyType()=> eEnemyType;
    public int GetCornDrop() => iCornDrop;
}
