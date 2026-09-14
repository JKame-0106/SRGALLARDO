using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject goProjectilePrefab;
    [SerializeField] private Transform tFirePoint;
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

        bool bWantsToShoot = false;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        bWantsToShoot = true;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        bWantsToShoot = true;

        bool bCanShoot = laneMovement == null || laneMovement.IsInLanePosition();

        if (bWantsToShoot && fFireCooldown <= 0f)
        {
            Shoot();
            fFireCooldown = fFireRate;
        }
    }

    void Shoot()
    {
        Instantiate(goProjectilePrefab, tFirePoint.position, Quaternion.identity);
    }
}
