using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{
    public static ResourceManager Instance {get; private set;}

    [Header("Maiz")]
    [SerializeField] private int iCorn = 50;

    [Header("Municion inicial por tipo")]
    [SerializeField] private int iStartingAmmoPerType = 30;

    private int[] arrAmmoPerType;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        int iTypeQuantity = Enum.GetValues(typeof(EColorType)).Length;
        arrAmmoPerType = new int[iTypeQuantity];

        for (int i = 0; i < iTypeQuantity; i++)
        {
            arrAmmoPerType[i] = iStartingAmmoPerType;
        }
    }

    public void AddCorn(int iAmount)
    {
        iCorn += iAmount;
    }

    public bool SpendCorn(int iAmount)
    {
        if (iCorn < iAmount) return false;
        iCorn -= iAmount;
        return true;
    }

    public int GetCorn() => iCorn;

    public bool HasAmmo(EColorType eColorType)
    {
        return arrAmmoPerType[(int)eColorType] > 0;
    }

    public void AddAmmo(EColorType eColorType, int iAmount)
    {
        arrAmmoPerType[(int)eColorType] += iAmount;
    }

    public bool TryUseAmmo(EColorType eColorType)
    {
        int iIndex = (int)eColorType;

        if (arrAmmoPerType[iIndex] <= 0) return false;

        arrAmmoPerType[iIndex]--;
        return true;
    }

    public int GetAmmo(EColorType eColorType) => arrAmmoPerType[(int)eColorType];
}

