using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Random product
    [SerializeField] GameObject[] products;

    public static event Action OnOrderSelected;

    private void OnEnable()
    {
        Worker_Controller.OnSelectProduct += RandomProduct;
    }
    private void OnDisable()
    {
        Worker_Controller.OnSelectProduct -= RandomProduct;
    }
    private void RandomProduct()
    {
        UIManager.instance.product = UnityEngine.Random.Range(0, products.Length);
        Debug.Log("ID prodotto: " + UIManager.instance.product);
        OnOrderSelected?.Invoke();
    }
    #endregion
    #region BTNs
    public void X5()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 5.0f;
            Debug.Log("Time scale 5");
            return;
        }
        else if (Time.timeScale == 5)
        {
            Time.timeScale = 1.0f;
            Debug.Log("Time scale 1");
        }
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quited");
    }
    #endregion
}
