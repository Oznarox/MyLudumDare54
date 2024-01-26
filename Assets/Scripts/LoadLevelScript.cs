using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelScript : MonoBehaviour
{

    public string levelName;
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName); 
    }
}