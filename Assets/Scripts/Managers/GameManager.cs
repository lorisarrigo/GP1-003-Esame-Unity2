using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void X2()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 3.0f;
            Debug.Log("Time scale 3");
            return;
        }
        else if (Time.timeScale == 3)
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
}
