using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    }

    public void EnergyBar()
    {
        energyBar.fillAmount = (float)Worker_Controller.instance.currentEnergy / (float)Worker_Controller.instance.maxEnergy;
    }
}
