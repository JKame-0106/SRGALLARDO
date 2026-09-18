using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject goProjecilePrefab;
    [SerializeField] private float fFireRate = 0.3f;

    private LaneMovement laneMovement;
    private float fFireCooldown = 0f;

    void Awake()
    {
        laneMovement = GetComponent<LaneMovement>();
    }

    void Update()
    {
        if (fFireCooldown > 0f)
        fFireCooldown -= Time.deltaTime;

        if (Keyboard.current == null) return;
        if (fFireCooldown > 0f) return;

        if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
            Shoot(EColorType.Rosado);
        else if(Keyboard.current.upArrowKey.wasPressedThisFrame)
            Shoot(EColorType.Azul);
        else if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
            Shoot(EColorType.Verde);
    }

    void Shoot(EColorType eEggType)
    {
        Transform tFirePoint = laneMovement.GetCurrentFirePoint();
        if (tFirePoint == null) return;

        GameObject goProjectile = Instantiate(goProjecilePrefab, tFirePoint.position, Quaternion.identity);

        Projectile projectile = goProjectile.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetAmmoType(eEggType);
        }

        fFireCooldown = fFireRate;
    }

}
