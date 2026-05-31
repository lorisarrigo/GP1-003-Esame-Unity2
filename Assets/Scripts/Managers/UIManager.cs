using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Worker_Controller wc;

    [Header("Product A")]
    [SerializeField] Image orderA;
    [SerializeField] int cubeQuantityA, sphereQuantityA, cylinderQuantityA;

    [Header("Product B")]
    [SerializeField] Image orderB;
    [SerializeField] int cubeQuantityB, sphereQuantityB, cylinderQuantityB;

    [Header("Product C")]
    [SerializeField] Image orderC;
    [SerializeField] int cubeQuantityC, sphereQuantityC, cylinderQuantityC;

    [Header("Txt")]
    [SerializeField] TMP_Text quantity_Txt;
    [SerializeField] TMP_Text quantityLeft_Txt;

    [Header("Energy")]
    [SerializeField] Image energyBar;

    public static UIManager instance;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;

        wc.TryGetComponent<Worker_Controller>(out wc);
    }

    public void EnergyBar()
    {
        energyBar.fillAmount = (float)wc.currentEnergy / (float)wc.maxEnergy;
    }
}
