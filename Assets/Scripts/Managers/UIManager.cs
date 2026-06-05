using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* Lo UIManager, gestisce:
        * l'aggiornamento della fillbar;
        * l'aggiornamento dei vari Txt;
     */
    [Header("Product A")]
    [SerializeField] GameObject orderA;

    [Header("Product B")]
    [SerializeField] GameObject orderB;

    [Header("Product C")]
    [SerializeField] GameObject orderC;

    [Header("Txt")]
    [SerializeField] TMP_Text quantity_Txt;
    [SerializeField] TMP_Text quantityLeft_Txt;
    [SerializeField] TMP_Text productSended_Txt;

    [Header("Energy")]
    [SerializeField] Image energyBar;
    void OnEnable()
    {
        Worker_Controller.OnUpdateEnergy += EnergyBar;
        Worker_Controller.OnQuantityLeft += LeftQuantity;
        Product_Manager.OnQuantityLeft += LeftQuantity;
        GameManager.OnOrderSelected += Order;
        Worker_Controller.OnSend += NoOrder;
        Worker_Controller.OnSend += OrderSent;
    }
    void OnDisable()
    {
        Worker_Controller.OnUpdateEnergy -= EnergyBar;
        Worker_Controller.OnQuantityLeft -= LeftQuantity;
        Product_Manager.OnQuantityLeft -= LeftQuantity;
        GameManager.OnOrderSelected -= Order;
        Worker_Controller.OnSend -= NoOrder;
        Worker_Controller.OnSend -= OrderSent;
    }
    void EnergyBar()
    {
        energyBar.fillAmount = Worker_Controller.instance.currentEnergy / Worker_Controller.instance.maxEnergy;
    }
    void LeftQuantity()
    {
        quantityLeft_Txt.text = Product_Manager.instance.leftQuantity[0] + "     " + Product_Manager.instance.leftQuantity[1] + "     " + Product_Manager.instance.leftQuantity[2];
    }
    void Order()
    {
        if (Product_Manager.instance.product == 0)
        {
            orderA.SetActive(true);
            Product_Manager.instance.currentOrder = Product_Manager.instance.QuantityA;
            
            quantity_Txt.text = Product_Manager.instance.QuantityA[0] + "\n  \n" + Product_Manager.instance.QuantityA[1] + "\n  \n" + Product_Manager.instance.QuantityA[2];
            return;
        }
        if (Product_Manager.instance.product == 1)
        {
            orderB.SetActive(true);

            Product_Manager.instance.currentOrder = Product_Manager.instance.QuantityB;
            
            quantity_Txt.text = Product_Manager.instance.QuantityB[0] + "\n  \n" + Product_Manager.instance.QuantityB[1] + "\n  \n" + Product_Manager.instance.QuantityB[2];
            return;
        }
        if (Product_Manager.instance.product == 2)
        {
            orderC.SetActive(true);

            Product_Manager.instance.currentOrder = Product_Manager.instance.QuantityC;
            
            quantity_Txt.text = Product_Manager.instance.QuantityC[0] + "\n  \n" + Product_Manager.instance.QuantityC[1] + "\n  \n" + Product_Manager.instance.QuantityC[2];
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
    void OrderSent()
    {
        if (Product_Manager.instance.product == 0)
        {
            Product_Manager.instance.OrderSended[0]++;
            productSended_Txt.text = Product_Manager.instance.OrderSended[0] + "     " + Product_Manager.instance.OrderSended[1] + "     " + Product_Manager.instance.OrderSended[2];
            return;
        }
        if (Product_Manager.instance.product == 1)
        {
            Product_Manager.instance.OrderSended[1]++;
            productSended_Txt.text = Product_Manager.instance.OrderSended[0] + "     " + Product_Manager.instance.OrderSended[1] + "     " + Product_Manager.instance.OrderSended[2];
            return;
        }
        if (Product_Manager.instance.product == 2)
        {
            Product_Manager.instance.OrderSended[2]++;
            productSended_Txt.text = Product_Manager.instance.OrderSended[0] + "     " + Product_Manager.instance.OrderSended[1] + "     " + Product_Manager.instance.OrderSended[2];
            return;
        }
    }
}
