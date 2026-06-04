using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Random product
    [SerializeField] GameObject[] products;
    [SerializeField] Transform workbenchTable;
    [SerializeField] Transform sendingTable;

    public static event Action OnOrderSelected;

    void OnEnable()
    {
        Worker_Controller.OnRandomProduct += RandomProduct;
        Worker_Controller.OnPlaceWb += PlaceObj;
        Worker_Controller.OnDeplace += SendingObj;
    }
    void OnDisable()
    {
        Worker_Controller.OnRandomProduct -= RandomProduct;
        Worker_Controller.OnPlaceWb -= PlaceObj;
        Worker_Controller.OnDeplace -= SendingObj;
    }
    void RandomProduct()
    {
        Product_Manager.instance.product = UnityEngine.Random.Range(0, products.Length);
        Debug.Log("ID prodotto: " + Product_Manager.instance.product);
        OnOrderSelected?.Invoke();
    }
    void PlaceObj()
    {
        GameObject prod = products[Product_Manager.instance.product];
        prod.transform.position = workbenchTable.position;
        prod.SetActive(true);
    }
    void SendingObj()
    {
        StartCoroutine(MoveProductRoutine());
    }
    IEnumerator MoveProductRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject prod = products[Product_Manager.instance.product];
        prod.SetActive(false);
        prod.transform.position = sendingTable.position;
        prod.SetActive(true);
        yield return new WaitForSeconds(1f);
        prod.SetActive(false);
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
