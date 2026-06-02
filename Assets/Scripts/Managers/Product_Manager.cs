using System;
using UnityEngine;

public class Product_Manager : MonoBehaviour
{
    public int product;
    public int maxQuant;
    public int[] leftQuantity = new int[3];
    public int[] currentOrder = new int[3];
    public GameObject[] Material = new GameObject[3];


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
    private void Start()
    {
        leftQuantity[0] = maxQuant;
        leftQuantity[1] = maxQuant;
        leftQuantity[2] = maxQuant;
        OnQuantityLeft?.Invoke();
    }
}
