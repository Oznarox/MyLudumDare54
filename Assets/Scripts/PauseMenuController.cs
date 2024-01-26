using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; //  Pause Menu UI GameObject
    private bool isPaused = false; // a játék pauzálási állapotát tárolja

    void Update()
    {
        // a felhasználó megnyomta-e az Escape gombot
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // folytatja a játékot, ha az szüneteltetve volt
            }
            else
            {
                PauseGame(); // szünetelteti a játékot, ha az nem volt szüneteltetve
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true); // aktiválja a Pause Menu UI-t
        Time.timeScale = 0f; // megállítja az idõt
        isPaused = true; // beállítja a játék állapotát szüneteltetettre
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // deaktiválja a Pause Menu UI-t
        Time.timeScale = 1f; // visszaállítja az idõt a normál sebességre
        isPaused = false; // beállítja a játék állapotát folytatottra
    }
}