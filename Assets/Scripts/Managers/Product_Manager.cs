using System;
using UnityEngine;

public class Product_Manager : MonoBehaviour
{
    [Tooltip("The max Quantity of material that can stay in the crates")] 
    public int maxQuant;

    public GameObject[] Material = new GameObject[3];

    [HideInInspector] public int product;
    [HideInInspector] public int[] leftQuantity = new int[3];
    [HideInInspector] public int[] currentOrder = new int[3];
    [HideInInspector] public int[] OrderSended = new int[3];

    public static event Action OnQuantityLeft;

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
