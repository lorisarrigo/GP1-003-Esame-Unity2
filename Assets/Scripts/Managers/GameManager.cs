using UnityEngine;

public class GameManager : MonoBehaviour
{
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
}
