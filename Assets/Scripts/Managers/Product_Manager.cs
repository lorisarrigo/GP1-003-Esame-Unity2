using System;
using UnityEngine;

public class Product_Manager : MonoBehaviour
{
    //una classe Manager che gestisce le informazioni dei prodotti e dei materiali

    [Tooltip("The max Quantity of material that can stay in the crates")] 
    public int maxQuant;

    [Tooltip("The Products")] 
    public GameObject[] products;

    [Tooltip("The materials available")] 
    public GameObject[] Material = new GameObject[3];

    [Tooltip("The materials needed to craft the products")] 
    public int[] QuantityA, QuantityB, QuantityC;

    //Array nascosti che servono a mantenere le informazioni che cambiano costantemente
    [HideInInspector] public int product;
    [HideInInspector] public int[] leftQuantity, currentOrder, OrderSended = new int[3];

    //Evento
    public static event Action OnQuantityLeft;

    /*Singleton
     * la classe viene viene richiamata in:
        * GameManager per:
            * scegliere randomicamente un prodotto;
            * far apparire l'oggetto scelto sui tavoli;
        * UIManager per:
            * aggiornare le farie quantità dei materiali e degli ordini presenti nei vari Txt;
        * Worker_Controller per:
            * controllare se dobbiamo prendere o meno i materiali;
            * decidere se o meno andare a rifornire le casse;
            * rifornirle;
            * scalare le quantità di materiali quando vengono prese;
     */
    public static Product_Manager instance;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    void Start()
    {
        leftQuantity[0] = maxQuant;
        leftQuantity[1] = maxQuant;
        leftQuantity[2] = maxQuant;
        OnQuantityLeft?.Invoke();
    }
}
