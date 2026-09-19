using UnityEngine;
using TMPro;
using System.Data;

[System.Serializable]
public class ShopUIRow
{
    [Header("A que color pertenece esta fila")]
    public EColorType eTipo;

    [Header("Textos de esta fila")]
    public TMP_Text txtDamage;
    public TMP_Text txtDamageCost;
    public TMP_Text txtProduction;
    public TMP_Text txtProductionCost;
    public TMP_Text txtBuyCost;
}
public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopUIRow[] arrRows;

    void Update()
    {
        if (ShopManager.Instance == null) return;

        foreach (ShopUIRow row in arrRows)
        {
            if (row.txtDamage != null)
                row.txtDamage.text = ShopManager.Instance.GetDamage(row.eTipo).ToString();

            if (row.txtDamageCost != null)
                row.txtDamageCost.text = ShopManager.Instance.GetDamageUpgradeCost(row.eTipo).ToString();

            if (row.txtProduction != null)
                row.txtProduction.text = "x" + ShopManager.Instance.GetEggsPerPurchase(row.eTipo);

            if (row.txtProductionCost != null)
                row.txtProductionCost.text = ShopManager.Instance.GetProductionUpgradeCost(row.eTipo).ToString();

            if (row.txtBuyCost != null)
                row.txtBuyCost.text = ShopManager.Instance.GetBuyEggCost(row.eTipo).ToString();
        }
    }
}
