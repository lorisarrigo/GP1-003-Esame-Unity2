using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int product;
    public int[] leftQuantity = new int[3];
    public int[] currentOrder = new int[3];

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
        GameManager.OnOrderSelected += Order;
        Worker_Controller.OnSend += NoOrder;
    }
    private void OnDisable()
    {
        Worker_Controller.UpdateEnergy -= EnergyBar;
        GameManager.OnOrderSelected -= Order;
        Worker_Controller.OnSend -= NoOrder;
    }
    private void EnergyBar()
    {
        energyBar.fillAmount = Worker_Controller.instance.currentEnergy / Worker_Controller.instance.maxEnergy;
    }
    private void Order()
    {
        if (product == 0)
        {
            orderA.SetActive(true);

            quantity_Txt.text = cubeQuantityA + "\n  \n" + sphereQuantityA + "\n  \n" + cylinderQuantityA;
            currentOrder[0] = cubeQuantityA;
            currentOrder[1] = sphereQuantityA;
            currentOrder[2] = cylinderQuantityA;

        }
        if (product == 1)
        {
            orderB.SetActive(true);

            quantity_Txt.text = cubeQuantityB + "\n  \n" + sphereQuantityB + "\n  \n" + cylinderQuantityB;
            currentOrder[0] = cubeQuantityB;
            currentOrder[1] = sphereQuantityB;
            currentOrder[2] = cylinderQuantityB;
        }
        if (product == 2)
        {
            orderC.SetActive(true);

            quantity_Txt.text = cubeQuantityC + "\n  \n" + sphereQuantityC + "\n  \n" + cylinderQuantityC;
            currentOrder[0] = cubeQuantityC;
            currentOrder[1] = sphereQuantityC;
            currentOrder[2] = cylinderQuantityC;
        }
    }
    void NoOrder()
    {
        orderA.SetActive(false);
        orderB.SetActive(false);
        orderC.SetActive(false);
    }
}
