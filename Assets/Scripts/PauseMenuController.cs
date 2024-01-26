using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; //  Pause Menu UI GameObject
    private bool isPaused = false; // a j�t�k pauz�l�si �llapot�t t�rolja

    void Update()
    {
        // a felhaszn�l� megnyomta-e az Escape gombot
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // folytatja a j�t�kot, ha az sz�neteltetve volt
            }
            else
            {
                PauseGame(); // sz�netelteti a j�t�kot, ha az nem volt sz�neteltetve
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true); // aktiv�lja a Pause Menu UI-t
        Time.timeScale = 0f; // meg�ll�tja az id�t
        isPaused = true; // be�ll�tja a j�t�k �llapot�t sz�neteltetettre
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // deaktiv�lja a Pause Menu UI-t
        Time.timeScale = 1f; // vissza�ll�tja az id�t a norm�l sebess�gre
        isPaused = false; // be�ll�tja a j�t�k �llapot�t folytatottra
    }
}