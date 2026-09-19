using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneMovement : MonoBehaviour
{
    [Header("Lanzahuevos")]
    [SerializeField] private EggLauncher[] arrLaunchers;

    [Header("Configuracion")]
    [SerializeField] private float fMoveCooldown = 0.15f;

    private int iCurrentLane = 1;
    private float fCooldownTimer =0f;

    void Start()
    {
        NewActiveLauncher();
    }

    void Update()
    {
        HandleInput();

        if (fCooldownTimer > 0f)
            fCooldownTimer -= Time.deltaTime;
    }

    void HandleInput()
    {
        if (fCooldownTimer > 0f) return;
        if (Keyboard.current == null) return;

        if(Keyboard.current.aKey.wasPressedThisFrame)
            MoveLane(-1);

        if (Keyboard.current.dKey.wasPressedThisFrame)
            MoveLane(1);
    }

    void MoveLane(int iDirection)
    {
        int iNewLane = Mathf.Clamp(iCurrentLane + iDirection, 0, arrLaunchers.Length -1);
        if (iNewLane != iCurrentLane)
        {
            iCurrentLane = iNewLane;
            fCooldownTimer = fMoveCooldown;
            NewActiveLauncher();
        }
    }

    void NewActiveLauncher()
    {
        for (int i = 0; i < arrLaunchers.Length; i++)
        {
            arrLaunchers[i].SetActive(i == iCurrentLane);
        }
    }
    public int GetCurrentLane() => iCurrentLane;
    public Transform GetCurrentFirePoint() => arrLaunchers[iCurrentLane].GetFirePoint();
}
