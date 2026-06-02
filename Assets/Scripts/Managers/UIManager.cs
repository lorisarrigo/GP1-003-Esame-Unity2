using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    

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
        Worker_Controller.OnQuantityLeft += LeftQuantity;
        Product_Manager.OnQuantityLeft += LeftQuantity;
        GameManager.OnOrderSelected += Order;
        Worker_Controller.OnSend += NoOrder;
    }
    private void OnDisable()
    {
        Worker_Controller.UpdateEnergy -= EnergyBar;
        Worker_Controller.OnQuantityLeft -= LeftQuantity;
        Product_Manager.OnQuantityLeft -= LeftQuantity;
        GameManager.OnOrderSelected -= Order;
        Worker_Controller.OnSend -= NoOrder;
    }
    private void EnergyBar()
    {
        energyBar.fillAmount = Worker_Controller.instance.currentEnergy / Worker_Controller.instance.maxEnergy;
    }
    private void LeftQuantity()
    {
        quantityLeft_Txt.text = Product_Manager.instance.leftQuantity[0] + "     " + Product_Manager.instance.leftQuantity[1] + "     " + Product_Manager.instance.leftQuantity[2];
    }
    private void Order()
    {
        if (Product_Manager.instance.product == 0)
        {
            orderA.SetActive(true);

            quantity_Txt.text = cubeQuantityA + "\n  \n" + sphereQuantityA + "\n  \n" + cylinderQuantityA;
            Product_Manager.instance.currentOrder[0] = cubeQuantityA;
            Product_Manager.instance.currentOrder[1] = sphereQuantityA;
            Product_Manager.instance.currentOrder[2] = cylinderQuantityA;
            return;
        }
        if (Product_Manager.instance.product == 1)
        {
            orderB.SetActive(true);

            quantity_Txt.text = cubeQuantityB + "\n  \n" + sphereQuantityB + "\n  \n" + cylinderQuantityB;
            Product_Manager.instance.currentOrder[0] = cubeQuantityB;
            Product_Manager.instance.currentOrder[1] = sphereQuantityB;
            Product_Manager.instance.currentOrder[2] = cylinderQuantityB;
            return;
        }
        if (Product_Manager.instance.product == 2)
        {
            orderC.SetActive(true);

            quantity_Txt.text = cubeQuantityC + "\n  \n" + sphereQuantityC + "\n  \n" + cylinderQuantityC;
            Product_Manager.instance.currentOrder[0] = cubeQuantityC;
            Product_Manager.instance.currentOrder[1] = sphereQuantityC;
            Product_Manager.instance.currentOrder[2] = cylinderQuantityC;
            return;
        }
    }
    void NoOrder()
    {
        orderA.SetActive(false);
        orderB.SetActive(false);
        orderC.SetActive(false);
        quantity_Txt.text = 0 + "\n  \n" + 0 + "\n  \n" + 0;
    }
}
