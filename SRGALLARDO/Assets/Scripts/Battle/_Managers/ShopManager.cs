using UnityEngine;
using System;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set;}

    [Header("Damage Boost")]
    [SerializeField] private int iBaseDamage = 10;
    [SerializeField] private int iDamagePerLevel = 5;
    [SerializeField] private int iChipDamage = 2;

    [Header("Production Boost")]
    [SerializeField] private int iBaseEggsPerPurchase = 5;
    [SerializeField] private int iEggsAddedPerLevel = 5;

    [Header("Corn Costs")]
    [SerializeField] private int iBaseEggCost = 10;
    [SerializeField] private int iBaseDamageUpgradeCost = 30;
    [SerializeField] private int iBaseProductionUpgradeCost = 30;
    [SerializeField] private float fCostMultiplierPerLevel = 1.5f;

    private int[] arrDamageLevel;
    private int[] arrProductionLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        int iTypeQuantity = Enum.GetValues(typeof(EColorType)).Length;
        arrDamageLevel = new int[iTypeQuantity];
        arrProductionLevel = new int [iTypeQuantity];
    }

    public bool TryBuyEggs(EColorType eColorType)
    {
        int iCost = GetBuyEggCost(eColorType);

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("[ShopManager] ¡ERROR CRÍTICO! ResourceManager.Instance no existe en la escena. Asegúrate de que el script ResourceManager esté en algún GameObject.");
            return false;
        }
        
        if (!ResourceManager.Instance.SpendCorn(iCost))
            return false;

        ResourceManager.Instance.AddAmmo(eColorType, GetEggsPerPurchase(eColorType));
        return true;
    }

    public int GetBuyEggCost(EColorType eColorType) => iBaseEggCost;

    public int GetEggsPerPurchase(EColorType eColorType)
    {
        int iLevel = arrProductionLevel[(int)eColorType];
        return iBaseEggsPerPurchase + (iLevel * iEggsAddedPerLevel);
    }


    public bool TryUpgradeDamage(EColorType eColorType)
    {
        int iIndex = (int)eColorType;
        int iCost = GetUpgradeCost(iBaseDamageUpgradeCost, arrDamageLevel[iIndex]);

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("[ShopManager] ¡ERROR CRÍTICO! ResourceManager.Instance no existe en la escena para comprar la mejora.");
            return false;
        }

        if (!ResourceManager.Instance.SpendCorn(iCost))
            return false;

        arrDamageLevel[iIndex]++;
        return true;
    }

    public int GetDamage(EColorType eColorType)
    {
        int iLevel = arrDamageLevel[(int)eColorType];
        return iBaseDamage + (iLevel * iDamagePerLevel);
    }

    public int GetChipDamage() => iChipDamage;

    public int GetDamageUpgradeCost(EColorType eColorType) => GetUpgradeCost(iBaseDamageUpgradeCost, arrDamageLevel[(int)eColorType]);

    public bool TryUpgradeProduction(EColorType eColorType)
    {
        int iIndex = (int)eColorType;
        int iCost = GetUpgradeCost(iBaseProductionUpgradeCost, arrProductionLevel[iIndex]);

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("[ShopManager] ¡ERROR CRÍTICO! ResourceManager.Instance no existe en la escena para comprar la mejora.");
            return false;
        }

        if (!ResourceManager.Instance.SpendCorn(iCost))
            return false;

        arrProductionLevel[iIndex]++;
        return true;
    }

        public int GetProductionUpgradeCost(EColorType eColorType) => GetUpgradeCost(iBaseProductionUpgradeCost, arrProductionLevel[(int)eColorType]);

        int GetUpgradeCost(int iBaseCost, int iActualLevel)
    {
        return Mathf.RoundToInt(iBaseCost * Mathf.Pow(fCostMultiplierPerLevel, iActualLevel));
    }
    
    }