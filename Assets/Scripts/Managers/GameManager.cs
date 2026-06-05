using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*il GameManager, gestisce:
        * la scelta randomica del prodotto;
        * il posizionamento sui tavoli dei prodotti;
        * le funzione di speedUp (X5) e Quit
     */

    //posizione di Placing dei prodotti (si trovano sopra i 2 tavoli)
    #region Random product
    [SerializeField] Transform workbenchTable;
    [SerializeField] Transform sendingTable;

    //Evento
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
        GameObject prod = Product_Manager.instance.products[Product_Manager.instance.product];
        Product_Manager.instance.product = UnityEngine.Random.Range(0, Product_Manager.instance.products.Length);
        OnOrderSelected?.Invoke();
    }
    void PlaceObj()
    {
        GameObject prod = Product_Manager.instance.products[Product_Manager.instance.product];
        prod.transform.position = workbenchTable.position;
        prod.SetActive(true);
    }
    void SendingObj()
    {
        StartCoroutine(MoveProductRoutine());
    }
    //in questa routine viene cambiata la posizione dell'oggetto;
    //l'effetto di flash quando deve sparire è generato dal Settaggio false/true in mezzo alla routine.
    IEnumerator MoveProductRoutine()
    {
        GameObject prod = Product_Manager.instance.products[Product_Manager.instance.product];
        yield return new WaitForSeconds(1.5f);
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
