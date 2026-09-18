using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float fSpeed = 12f;
    [SerializeField] private float fLifeTime = 3f;

    [Header("Combate")] //Por ahora esta vaina es generica, luego hay que ligarlo con el tipo de municion...
    [SerializeField] private EColorType eAmmoType = EColorType.Rosado;
    [SerializeField] private int iMaxDamage = 10;
    [SerializeField] private int iChipDamage = 2;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        Destroy(gameObject, fLifeTime);
         ApplyVisualColor();
    }

    void Update()
    {
       /* Debug.Log("Projectile Update corriendo, Y actual: " + transform.position.y);*/
        transform.position += Vector3.up * fSpeed * Time.deltaTime;
    }

    //Por ahora ignoren esto, era para el enemigo pero eso lo veo ahora con calma
    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            int iDamageFinal = (eAmmoType == enemy.GetEnemyType()) ? iMaxDamage : iChipDamage;
            enemy.TakeDamage(iDamageFinal);
            Destroy(gameObject);
        }
    }
    public void SetAmmoType(EColorType eNewType)
    {
        eAmmoType = eNewType;
        ApplyVisualColor();
    }

    void ApplyVisualColor()
    {
        if (spriteRenderer == null) return;

        switch (eAmmoType)
        {
           case EColorType.Rosado:
                spriteRenderer.color = new Color(1f, 0.2f, 0.6f);
                break;
            case EColorType.Azul:
                spriteRenderer.color = new Color(0.3f, 0.9f, 1f);
                break;
            case EColorType.Verde:
                spriteRenderer.color = new Color(0.3f, 0.85f, 0.2f);
                break;
        }
    }
}
