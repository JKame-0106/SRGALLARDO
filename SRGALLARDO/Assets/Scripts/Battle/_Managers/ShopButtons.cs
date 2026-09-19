using UnityEngine;

public enum EShopAction
{
    BuyEggs,
    UpgradeDamage,
    UpgradeProduction
}

public class ShopButtons : MonoBehaviour
{
    [Header("COnfiguracion del boton")]
    [SerializeField] private EColorType eColorType;
    [SerializeField] private EShopAction eAction;

    public void OnClick()
    {
         Debug.Log("Boton presionado: " + eAction + " - " + eColorType);
        if (ShopManager.Instance == null) return;

        bool bSucces = false;

        switch(eAction)
        {
            case EShopAction.BuyEggs:
            bSucces = ShopManager.Instance.TryBuyEggs(eColorType);
            break;
        }
         switch(eAction)
        {
            case EShopAction.UpgradeDamage:
            bSucces = ShopManager.Instance.TryUpgradeDamage(eColorType);
            break;
        }
         switch(eAction)
        {
            case EShopAction.UpgradeProduction:
            bSucces = ShopManager.Instance.TryUpgradeProduction(eColorType);
            break;
        }

        if (!bSucces)
        {
            Debug.LogFormat ("Eri pobre, tu no tienes maiz suficiente para " + eAction + " (" + eColorType + ")");
        }
    }

}
