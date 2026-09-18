using UnityEngine;

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

        if (iHealth <= 0)
        {
            Die();
        }
    }

    public void SetEnemyType(EColorType eNewType)
    {
        eEnemyType = eNewType;
        ApplyVisualColor();
    }

    void ReachedCoop()
    {
        Debug.Log(eEnemyType + " llego al granero!");
        Destroy(gameObject);
    }
    void Die()
    {
        Debug.Log(eEnemyType + " derrotado. Suelta " + iCornDrop + " de maiz.");
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

    public EColorType GetEnemyType()=> eEnemyType;
    public int GetCornDrop() => iCornDrop;
}
