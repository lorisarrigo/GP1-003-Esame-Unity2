using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int product;
    //public int[] leftQuantity = new int[3];

    [Header("Product A")]
    [SerializeField] GameObject orderA;
    [SerializeField] int cubeQuantityA, sphereQuantityA, cylinderQuantityA;

    [Header("Product B")]
    [SerializeField] GameObject orderB;
    [SerializeField] int cubeQuantityB, sphereQuantityB, cylinderQuantityB;

    [Header("Product C")]
    [SerializeField] GameObject orderC;
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
    private void OnEnable()
    {
        Worker_Controller.UpdateEnergy += EnergyBar;
        Worker_Controller.OnSelectProduct += UpdateOrder;
    }
    private void OnDisable()
    {
        Worker_Controller.UpdateEnergy -= EnergyBar;
        Worker_Controller.OnSelectProduct -= UpdateOrder;
    }
    private void EnergyBar()
    {
        energyBar.fillAmount = Worker_Controller.instance.currentEnergy / Worker_Controller.instance.maxEnergy;
    }
    private void UpdateOrder()
    {
        if(product == 0)
        {
            orderA.SetActive(true);
            orderB.SetActive(false);
            orderC.SetActive(false);
            quantity_Txt.text = cubeQuantityA + "\n  \n" + sphereQuantityA + "\n  \n" + cylinderQuantityA;
        }
        if(product == 1)
        {
            orderA.SetActive(false);
            orderB.SetActive(true);
            orderC.SetActive(false);
            quantity_Txt.text = cubeQuantityB + "\n  \n" + sphereQuantityB + "\n  \n" + cylinderQuantityB;
        }
        if(product == 2)
        {
            orderA.SetActive(false);
            orderB.SetActive(false);
            orderC.SetActive(true);
            quantity_Txt.text = cubeQuantityC + "\n  \n" + sphereQuantityC + "\n  \n" + cylinderQuantityC;
        }
    }
}
