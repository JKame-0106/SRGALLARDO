using UnityEngine;

public class EggLauncher : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite spriteInactive;
    [SerializeField] private Sprite spriteActive;

    [Header("Punto de Disparo")]
    [SerializeField] private Transform tFirePoint;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetActive(false);
    }

    public void SetActive(bool bActive)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = bActive ? spriteActive : spriteInactive;
    }

    public Transform GetFirePoint() => tFirePoint;
}
