using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [Header("Texto de maiz")]
    [SerializeField] private TMP_Text txtCorn;

    [Header("Textos de municion R-A-V")]
    [SerializeField] private TMP_Text[] arrTxtAmmo;

    void Update()
    {
        if (ResourceManager.Instance == null) return;

        if (txtCorn != null)
        {
            txtCorn.text = "x " + ResourceManager.Instance.GetCorn();
        }

        for(int i = 0; i < arrTxtAmmo.Length; i++)
        {
            if (arrTxtAmmo[i] != null)
            {
                arrTxtAmmo[i].text = "x " + ResourceManager.Instance.GetAmmo((EColorType)i);
            }
        }
    }
}
