using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneMovement : MonoBehaviour
{
    [Header("Configuracion de carriles")]
    [SerializeField] private float [] fLanePositionsX = {-4f, 0f, 4f};// Cada uno se refiere a la linea
    [SerializeField] private float fMoveSpeed = 15f;
    [SerializeField] private float fMoveCooldown = 0.15f;
    [SerializeField] private float fArrivalThreshold = 0.05f;

    private int iCurrentLane = 1;
    private float fCooldownTimer = 0f;
    private bool bIsInLanePosition = true;

    void Update() 
    {
        HandleInput();

        Vector3 targetPos = new Vector3(fLanePositionsX[iCurrentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, fMoveSpeed * Time.deltaTime);

        float fDistanceToTarget = Mathf.Abs(transform.position.x - fLanePositionsX[iCurrentLane]);
        bIsInLanePosition = fDistanceToTarget <= fArrivalThreshold;

        if (fCooldownTimer > 0f)
        fCooldownTimer -= Time.deltaTime;
    }

    void HandleInput()
    {
        if (fCooldownTimer > 0f) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("Tecla A detectada, carril actual: " + iCurrentLane);
            MoveLane(-1);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Debug.Log("Tecla D detectada, carril actual: " + iCurrentLane);
            MoveLane(1);
        }
    }

    void MoveLane(int iDirection)
    {
        int iNewLane = Mathf.Clamp(iCurrentLane + iDirection, 0, fLanePositionsX.Length -1);
        if (iNewLane != iCurrentLane)
        {
            iCurrentLane = iNewLane;
            fCooldownTimer = fMoveCooldown;
        }
    }

    public int GetCurrentLane() => iCurrentLane;
    public bool IsInLanePosition() => bIsInLanePosition;//Este y donde se menciona es para tratar de no disparar huevos en la mitad de las lineas y a pesar de que se haga menos... si calculas bien lo puedes hacer...
}
